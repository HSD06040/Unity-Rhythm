using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public float startY => GameManager.Instance.spawnLine.position.y;
    public float endY => GameManager.Instance.judgeLine.position.y;
    public int gridCount = 10;
    public float endRatio => (startY - endY) / gridCount; // 한 칸의 Y

    [SerializeField] private bool musicPlay;
    [SerializeField] private BGM bgm;
    [SerializeField] private int BPM;
    [SerializeField] private string artist;
    public string keySound;
    private float currentTime => GameManager.Instance.time / 1000;
    [SerializeField] private TMP_Text currentTimeText;

    public GameObject linePrefab;
    public GameObject notePrefab;
    public GameObject longPrefab;
    public GameObject barPrefab;

    private GameObject curNote;
    private List<GridLine> lines = new List<GridLine>();
    private GridLine headLine;

    private Transform bar;
    [SerializeField] private Transform gridParent;
    [SerializeField] private float[] offset;

    private int longCount;
    private bool isLongNote;
    private float spawnY;
    private Camera cam;
    Vector3 worldPos;

    private void Awake()
    {
        cam = Camera.main;
        bar = Instantiate(barPrefab, GameManager.Instance.judgeLine.position, Quaternion.identity).transform;
        bar.GetComponent<EditBar>().Init(endY, startY - endY);

        for (int i = 1; i < 3000; i++)
        {
            spawnY = endY + (endRatio * i);
            GameObject line = Instantiate(linePrefab, new Vector3(GameManager.Instance.judgeLine.position.x, spawnY,0), Quaternion.identity, gridParent);

            if(i % 10 == 0)
                line.GetComponent<SpriteRenderer>().color = Color.red;
            else if (i % 5 == 0)
                line.GetComponent<SpriteRenderer>().color = Color.yellow;

            GridLine gridLine = line.GetComponent<GridLine>();
            gridLine.Init(i);
            lines.Add(gridLine);
        }
    }

    private void Update()
    {
        if(musicPlay)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, bar.position.y, cam.transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            ChangeMusicStartStop();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            RewindMusic(-1);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            RewindMusic(1);

        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.Instance.PlayBGM(bgm, 1);
            musicPlay = true;
        }

        currentTimeText.text = currentTime.ToString();

        Vector3 mPos = Input.mousePosition;
        mPos.z = -cam.transform.position.z;
        worldPos = cam.ScreenToWorldPoint(mPos);

        Debug.DrawRay(worldPos, cam.transform.forward * 2, Color.red, 0.2f);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, cam.transform.forward, 2f);

        if (hit.transform == null)
            return;

        if (hit.transform.CompareTag("Grid"))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GridLine line = hit.transform.GetComponent<GridLine>();

                float minDist = float.MaxValue;
                int idx = 0;

                for (int i = 0; i < offset.Length; i++)
                {
                    float offsetX = line.transform.position.x + offset[i];
                    float dist = Mathf.Abs(worldPos.x - offsetX);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        idx = i;
                    }
                }

                CreateNote(line, idx);
            }
        }
    }

    public void SetShortNote()
    {
        curNote = notePrefab;
    }

    public void SetLongNote()
    {
        curNote = longPrefab;
    }    

    public void ChangeMusicStartStop()
    {
        if (musicPlay)
        {
            StopMusic();
        }
        else
        {
            RestartMusic();
        }
    }

    public void RewindMusic(float seconds)
    {
        AudioManager.Instance.RewindBGM(seconds);
    }

    public void StopMusic()
    {
        musicPlay = false;
        AudioManager.Instance.StopBGM();
    }

    public void RestartMusic()
    {
        musicPlay = true;
        AudioManager.Instance.RestartBGM();
    }

    private void CreateNote(GridLine gridLine, int idx)
    {
        Vector3 spawnPos = new Vector3(gridLine.transform.position.x + offset[idx], gridLine.transform.position.y);

        if(curNote != null)
        {
            if (curNote == notePrefab)
            {
                gridLine.AddNoteData
                (
                    new NoteData
                    {
                        noteType = 0,
                        keyPos = idx,
                        startTime = gridLine.startTime,
                        keySound = keySound,
                        keySoundVolume = 1,
                    }
                );
            }
            else if (curNote == longPrefab)
            {
                if (longCount == 0)
                {
                    headLine = gridLine;
                    longCount++;
                }
                else if (longCount == 1)
                {
                    if(gridLine.idx - headLine.idx <= 0)
                    {
                        Debug.Log("롱 노트의 끝은 시작보다 높아야 합니다!");
                        return;
                    }

                    headLine.AddNoteData
                        (
                            new NoteData
                            {
                                noteType = 1,
                                keyPos = idx,
                                startTime = headLine.startTime,
                                endTime = gridLine.startTime - headLine.startTime,
                                keySound = keySound,
                                keySoundVolume = 1,
                            }
                        );

                    SpriteRenderer longbody = 
                        Instantiate(longPrefab, headLine.transform.position, Quaternion.identity).
                        GetComponent<SpriteRenderer>();

                    longbody.transform.position += new Vector3(offset[idx], (gridLine.transform.position.y - headLine.transform.position.y) / 2);

                    float height = longbody.sprite.bounds.size.y;

                    float distance = endRatio * (gridLine.idx - headLine.idx);
                    float result = distance / height;

                    longbody.transform.localScale = new Vector3(longbody.transform.localScale.x, -result, longbody.transform.localScale.z);

                    // 롱 바디 생성 후 크기 지정

                    longCount = 0;
                }
            }

            Instantiate(notePrefab, spawnPos, Quaternion.identity);
        }
        else
            Debug.Log("노트가 설정되어 있지 않습니다! 노트를 먼저 설정해 주세요.");  
    }

    public void Save()
    {
        List<NoteData> noteDatas = new List<NoteData>();

        foreach (var line in lines)
        {
            foreach (var note in line.noteList)
            {
                noteDatas.Add(note);
            }
        }

        MapData map = new MapData();

        map.musicData = new MusicData
        {
            musicName = bgm.ToString(),
            BPM = BPM,
            artistName = artist
        };

        map.notes = noteDatas;

        MapParser.SaveMap(map, bgm);
    }

    public void Load()
    {

    }
}
