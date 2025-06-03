using FMOD;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>, ISavable
{
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private GameObject longPrefab;

    public BGM bgm;
    public bool onMusicPlaying;
    public bool isFirstPlaying;
    public bool isBusy;
    public PlayData currnetPlayData;
    public MusicData currentMusicData;

    public bool isEdit;
    public int BPM;
    public float time => GetMusicMs();
    public float tickTime => (60f / BPM) * 1000;

    public float startDelay;
    public float endTime;    
    public float distnace;

    public List<NoteData> noteSpawnList = new List<NoteData>();

    private float _scrollSpeed;
    public float scrollSpeed { get => _scrollSpeed; set { _scrollSpeed = value; onScrollSpeedChanged?.Invoke(_scrollSpeed); } }

    public ObjectPool<GameObject> barPool;
    public ObjectPool<GameObject> notePool;
    public ObjectPool<GameObject> longNotePool;
    
    private Transform noteParent;
    private Transform longNoteParent;

    public Transform spawnLine;
    public Transform judgeLine;

    public event Action<float> onScrollSpeedChanged;

    private Coroutine gameClearRoutine;
    private int idx;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        scrollSpeed = 1;
    }

    private void Update()
    {
        SetNote();

        if(!isEdit)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                GameClear();
        }        
    }

    private void InitPlayData()
    {
        idx = 0;
        notePool.Clear();
        longNotePool.Clear();
        EffectManager.Instance.Init();
        JudgeManager.Instance.SetJudgeMs();
        ScoreManager.Instance.InitPlayData();
    }

    public void GameStart(MusicData data)
    {
        bgm = data.bgm;
        BPM = data.BPM;
        currentMusicData = data;
        startDelay = data.startDelay;

        StartCoroutine(GameStartRoutine());
    }

    public void GameClear()
    {
        StartCoroutine(GameClearRoutine());
    }

    private IEnumerator GameStartRoutine()
    {
        UI_Manager.Instance.fadeScreen.EnterFade(FadeType.Defualt);
        isBusy = true;
        yield return new WaitForSeconds(2);
        UI_Manager.Instance.mvPlayer.StopVideo();
        AudioManager.Instance.StopBGM();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
            yield return null;

        asyncLoad.allowSceneActivation = true;

        yield return new WaitForSeconds(.5f);

        spawnLine = GameObject.Find("SpawnLine").transform;
        judgeLine = GameObject.Find("JudgeLine").transform;

        Init();

        UI_Manager.Instance.fadeScreen.ExitFade(FadeType.Defualt);

        yield return new WaitForSeconds(2);

        SetGameStart();
        isBusy = false;        
    }

    public void SetGameStart()
    {
        onMusicPlaying = true;
        InitPlayData();
        noteSpawnList = Parser.LoadMap(bgm);
        AudioManager.Instance.PlayBGM(bgm, 1);
        UI_Manager.Instance.mvPlayer.PlayMusicVideo(currentMusicData.videoURL);
    }

    private IEnumerator GameClearRoutine()
    {
        isBusy = true;
        yield return new WaitForSeconds(5);
        onMusicPlaying = false;
        UI_Manager.Instance.fadeScreen.EnterFade(FadeType.Defualt);

        yield return new WaitForSeconds(2);
        UI_Manager.Instance.mvPlayer.StopVideo();
        AudioManager.Instance.StopBGM();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ResultScene");
        asyncLoad.allowSceneActivation = false;

        ScoreManager.Instance.SavePlayData(bgm);

        InitPlayData();

        while (asyncLoad.progress < 0.9f)
            yield return null;

        asyncLoad.allowSceneActivation = true;

        AudioManager.Instance.PlayBGM(bgm, 1);
        UI_Manager.Instance.mvPlayer.PlayMusicVideo(currentMusicData.videoURL, false);

        yield return new WaitForSeconds(.5f);

        UI_Manager.Instance.fadeScreen.ExitFade(FadeType.Defualt);

        isBusy = false;
        gameClearRoutine = null;
    }

    public uint GetMusicMs()
    {
        // 노래의 진행도를 반환      
        AudioManager.Instance.musicChannel.getPosition(out uint pos, FMOD.TIMEUNIT.MS);
        return pos;
    }

    private void Init()
    {
        notePool = CreatePool(notePrefab, noteParent, 10);
        longNotePool = CreatePool(longPrefab, longNoteParent, 10);
    }

    private void SetNote()
    {
        if(onMusicPlaying)
        {
            if (noteSpawnList.Count > idx && time >= (noteSpawnList[idx].startTime - endTime))
            {
                NoteData data = new NoteData();

                if (noteSpawnList[idx].noteType == 1)
                {
                    noteSpawnList[idx].longNoteData = new NoteData
                    {
                        startTime = noteSpawnList[idx].startTime + noteSpawnList[idx].endTime,
                        keyPos = noteSpawnList[idx].keyPos,
                    };
                }

                if (noteSpawnList[idx].longNoteData != null)
                {                  
                    GameObject mainNote = notePool.Get();
                    mainNote.transform.position = transform.position;
                    Note note = mainNote.GetComponent<Note>();
                    noteSpawnList[idx].isLongNoteStart = true;
                    noteSpawnList[idx].isLongNoteBody = false;
                    noteSpawnList[idx].isLongNoteEnd = false;
                    note.InitData(noteSpawnList[idx]);
                    JudgeManager.Instance.AddNoteList(noteSpawnList[idx].keyPos, note);

                    GameObject longNoteBody = longNotePool.Get();
                    longNoteBody.transform.position = transform.position;
                    note = longNoteBody.GetComponent<Note>();
                    noteSpawnList[idx].isLongNoteStart = false;
                    noteSpawnList[idx].isLongNoteBody = true;
                    noteSpawnList[idx].isLongNoteEnd = false;
                    note.data.keySoundVolume = 0;
                    note.InitData(noteSpawnList[idx]);
                    JudgeManager.Instance.AddNoteList(noteSpawnList[idx].keyPos, note);

                    GameObject longNoteEnd = notePool.Get();
                    longNoteEnd.transform.position = transform.position;
                    note = longNoteEnd.GetComponent<Note>();
                    noteSpawnList[idx].longNoteData.isLongNoteStart = false;
                    noteSpawnList[idx].longNoteData.isLongNoteBody = false;
                    noteSpawnList[idx].longNoteData.isLongNoteEnd = true;
                    note.data.keySoundVolume = 0;
                    note.InitData(noteSpawnList[idx].longNoteData);
                    JudgeManager.Instance.AddNoteList(noteSpawnList[idx].keyPos, note);   
                }
                else
                {
                    noteSpawnList[idx].TransmitData(data);

                    GameObject mainNote = notePool.Get();
                    mainNote.transform.position = transform.position;
                    Note note = mainNote.GetComponent<Note>();
                    note.InitData(data);
                    JudgeManager.Instance.AddNoteList(data.keyPos, note);
                }
                idx++;
            }
            else if (idx >= noteSpawnList.Count && time >= (noteSpawnList[idx].startTime))
            {
                if(gameClearRoutine == null)
                    gameClearRoutine = StartCoroutine(GameClearRoutine());
            }
        }        
    }

    
    private ObjectPool<GameObject> CreatePool(GameObject prefab, Transform parent, int maxSize)
    {
        parent = new GameObject($"{prefab.name} parent").transform;

        return new ObjectPool<GameObject>
            (
                () =>
                {
                    GameObject go = GameObject.Instantiate(prefab);
                    go.transform.SetParent(parent, false);
                    return go;
                },
                obj =>
                {
                    obj.SetActive(true);
                    obj.transform.SetParent(null);
                },
                obj =>
                {
                    obj.SetActive(false);
                    obj.transform.SetParent(parent, false);
                },
                obj =>
                {
                    Destroy(obj.gameObject);
                },
                false,
                maxSize
            );
    }

    public void ReleaseNote(Note note)
    {
        if (!note.gameObject.activeSelf) return;

        if(note != null && note.data.longNoteData != null && note.data.isLongNoteBody)
        {
            longNotePool.Release(note.gameObject);
        }
        else
        {
            notePool.Release(note.gameObject);
        }
    }

    public void Save(ref GameData data)
    {
        data.isFirstPlaying = isFirstPlaying;
    }

    public void Load(GameData data)
    {
        isFirstPlaying = data.isFirstPlaying;
    }
}
