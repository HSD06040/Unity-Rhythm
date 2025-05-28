using FMOD;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : Manager<GameManager>
{
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private GameObject longPrefab;

    public BGM bgm;
    public bool onMusicStart;
    public bool isEdit;
    [SerializeField] private bool isLong;
    public int BPM;
    public float time => GetMusicMs(); // 테스트용
    public float tickTime => (60f / BPM) * 1000;
    public float speed;
    public float resSpeed;
    public float startDelay;
    public float endTime;
    public float musicOffsetDistance;
    public float distnace;

    private int currentTick;
    private float fixedScrollSpeed;

    private int idx;
    public List<NoteData> noteSpawnList = new List<NoteData>();
    public List<double> scrollPos = new();
    public float scrollSpeed = 1;

    public ObjectPool<GameObject> barPool;
    public ObjectPool<GameObject> notePool;
    public ObjectPool<GameObject> longNotePool;

    public Transform mapNoteParent;
    private Transform noteParent;
    private Transform longNoteParent;

    public Transform spawnLine;
    public Transform judgeLine;
    private GameObject currentBar;

    protected override void Awake()
    {
        base.Awake();
        Init();

        if(!isEdit)
        {
            if (isLong)
            {
                for (int i = 1; i < 50; i++)
                {
                    int pos = UnityEngine.Random.Range(0, 4);
                    noteSpawnList.Add(new NoteData
                    {
                        startTime = 1000 * i,
                        keyPos = pos,
                        endTime = 1000,
                        longNoteData = new NoteData { startTime = 1000 * i + 1000, keyPos = pos }
                    });
                }
            }
            else
            {
                for (int i = 1; i < 50; i++)
                {
                    int pos = UnityEngine.Random.Range(0, 4);
                    noteSpawnList.Add(new NoteData
                    {
                        startTime = 1000 * i,
                        keyPos = pos,
                        endTime = 1000,
                    });
                }
            }
        }     
    }

    private void Update()
    {
        SetNote();

        if(!isEdit)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                GameStart(bgm);
        }        
    }

    public void GameStart(BGM _bgm)
    {
        spawnLine = GameObject.Find("SpawnLine").transform;
        judgeLine = GameObject.Find("JudgeLine").transform;

        AudioManager.Instance.PlayBGM(_bgm, 1);
        onMusicStart = true;
        noteSpawnList = MapParser.LoadMap(_bgm);
    }

    public uint GetMusicMs()
    {
        // 노래의 진행도를 반환
        AudioManager.Instance.musicChannel.getPosition(out uint pos, FMOD.TIMEUNIT.MS);
        return pos;
    }

    private void SetFixedSpeed()
    {
        float pivot = 2000f;
        fixedScrollSpeed = pivot / tickTime;
    }

    private void SetSpeed()
    {
        speed = tickTime * fixedScrollSpeed / scrollSpeed;
        resSpeed = fixedScrollSpeed / scrollSpeed;
    }

    private void ScrollPos()
    {
        float noteStartTime = 1000f;
        for (float i = 0.1f; i < 100f; i += 0.1f)
        {
            double _time = noteStartTime - (tickTime * i) + tickTime;
            double yPos = (_time - noteStartTime) / (tickTime * i);
            scrollPos.Add(yPos);
        }
    }

    private void Init()
    {
        BPM = 120;
        notePool = CreatePool(notePrefab, noteParent, 10);
        longNotePool = CreatePool(longPrefab, longNoteParent, 10);
        SetFixedSpeed();
        SetSpeed();
        ScrollPos();
        JudgeManager.Instance.SetJudgeMs();
    }

    private void SetNote()
    {
        if(onMusicStart)
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
}
