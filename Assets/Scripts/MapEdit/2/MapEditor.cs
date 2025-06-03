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

    [Header("Music Data")]
    [SerializeField] private float editTime;
    [SerializeField] private BGM bgm;    
    [Space]

    [SerializeField] private float resSpeed = .75f;
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
            AudioManager.Instance.musicChannel.setPitch(resSpeed);
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
            else if (Input.GetKeyDown(KeyCode.Mouse1))
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

                RemoveNote(line, idx);
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
                gridLine.AddNoteObj(idx, Instantiate(notePrefab, spawnPos, Quaternion.identity));
            }
            else if (curNote == longPrefab)
            {
                if (longCount == 0)
                {
                    headLine = gridLine;
                    headLine.longIdx = idx;
                    headLine.AddNoteObj(idx, Instantiate(notePrefab, spawnPos, Quaternion.identity));
                    longCount++;
                }
                else if (longCount == 1)
                {
                    if(gridLine.idx - headLine.idx <= 0)
                    {
                        Debug.Log("롱 노트의 끝은 시작보다 높아야 합니다!");
                        return;
                    }

                    if (headLine.longIdx != idx)
                    {
                        Debug.Log("롱 노트의 처음과 끝은 같은 자리여야 합니다.");
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

                    longbody.transform.position += new Vector3(offset[idx], gridLine.transform.position.y - headLine.transform.position.y);

                    float height = longbody.sprite.bounds.size.y;

                    float distance = endRatio * (gridLine.idx - headLine.idx);
                    float result = distance / height;

                    longbody.transform.localScale = new Vector3(longbody.transform.localScale.x, -result, longbody.transform.localScale.z);
                    
                    longCount = 0;

                    GameObject end = Instantiate(notePrefab, spawnPos, Quaternion.identity);

                    headLine.SetLongNote(idx, longbody.gameObject, end);
                }
            }  
        }
        else
            Debug.Log("노트가 설정되어 있지 않습니다! 노트를 먼저 설정해 주세요.");  
    }

    private void RemoveNote(GridLine gridLine, int idx)
    {
        NoteData data = gridLine.GetNoteData(idx);

        if(data != null)
        {
            if(data.noteType == 0)
            {
                gridLine.RemoveNote(idx);
            }
            else
            {
                gridLine.RemoveNote(idx);
                gridLine.RemoveLongNote(idx);
            }
        }
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

        map.bgm = bgm;

        map.notes = noteDatas;

        Parser.SaveMap(map);
    }

    public void Load()
    {
        List<NoteData> datas = Parser.LoadMap(bgm);

        for (int i = 0; i < lines.Count; i++)
        {
            foreach (var data in datas)
            {
                if (lines[i].startTime == data.startTime)
                {                    
                    lines[i].AddNoteData
                    (
                        new NoteData
                        {
                            noteType = data.noteType == 0 ? 0 : 1,
                            keyPos = data.keyPos,
                            startTime = data.startTime,
                            endTime = data.endTime,
                            keySound = keySound,
                            keySoundVolume = 1,
                        }
                    );

                    Vector3 spawnPos = new Vector3(lines[i].transform.position.x + offset[data.keyPos], lines[i].transform.position.y);

                    if(data.noteType == 0)
                    {
                        lines[i].AddNoteObj(data.keyPos, Instantiate(notePrefab, spawnPos, Quaternion.identity));
                    }
                    else
                    {
                        GridLine gridline = lines[i + (int)(data.endTime / 50)];

                        lines[i].AddNoteObj(data.keyPos, Instantiate(notePrefab, spawnPos, Quaternion.identity));

                        SpriteRenderer longbody =
                        Instantiate(longPrefab, lines[i].transform.position, Quaternion.identity).
                        GetComponent<SpriteRenderer>();

                        longbody.transform.position += new Vector3(offset[data.keyPos], gridline.transform.position.y - lines[i].transform.position.y);

                        float height = longbody.sprite.bounds.size.y;

                        float distance = endRatio * (gridline.idx - lines[i].idx);
                        float result = distance / height;

                        longbody.transform.localScale = new Vector3(longbody.transform.localScale.x, -result, longbody.transform.localScale.z);                        


                        GameObject end = Instantiate(notePrefab, new Vector3(gridline.transform.position.x + offset[data.keyPos], gridline.transform.position.y), 
                            Quaternion.identity);

                        lines[i].SetLongNote(data.keyPos, longbody.gameObject, end);                                                
                    }
                }
            }
        }
    }

    public void Copy()
    {
        foreach (var line in lines)
        {
            if(line.noteList.Count > 0)
            { 
                foreach(var note in line.noteList)
                {
                    note.startTime += editTime;
                }
            }
        }
    }
}
