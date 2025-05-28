using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class JudgeManager : Manager<JudgeManager>
{
    [SerializeField] private KeyCode[] onKey;
    [SerializeField] private float[] judgeMs;
    [SerializeField] private float[] defaultJudgeMs;
    public NoteLane[] noteLanes;
    private float checkMs;
    private YieldInstruction delay;

    protected override void Awake()
    {
        base.Awake();

        onKey = new KeyCode[] { KeyCode.D, KeyCode.F, KeyCode.H, KeyCode.J };
        noteLanes = new NoteLane[onKey.Length];

        for (int i = 0; i < noteLanes.Length; i++)
        {
            noteLanes[i] = new NoteLane();
        }
    }

    private void Update()
    {
        Judgement(0);
        Judgement(1);
        Judgement(2);
        Judgement(3);
    }

    public void SetJudgeMs()
    {
        judgeMs = new float[] 
        { 
            defaultJudgeMs[0] / GameManager.Instance.scrollSpeed,
            defaultJudgeMs[1] / GameManager.Instance.scrollSpeed,
            defaultJudgeMs[2] / GameManager.Instance.scrollSpeed,
        };
    }

    private void Judgement(int idx)
    {
        if (Input.GetKeyDown(onKey[idx]))
        {
            Note note = noteLanes[idx].GetNote();

            if (note == null || note.data.isJudgeDone || note.data.keyPos != idx) return;

            checkMs = note.data.startTime - GameManager.Instance.time;

            if (checkMs > 200 / GameManager.Instance.scrollSpeed)
                return;

            if (note.data.longNoteData != null)
            {
                if (note.data.isLongNoteStart)
                {                    
                    if (checkMs <= judgeMs[(int)Judge.Good] && checkMs >= -judgeMs[(int)Judge.Good])
                    {
                        ScoreManager.Instance.AddJudgeResult(Judge.Perfect);

                        GameManager.Instance.ReleaseNote(noteLanes[idx].DequeueNote()); // Start 제거
                        StartCoroutine(LongBodyJudge(noteLanes[idx].GetNextLongBody()));
                    }
                    else
                    {
                        ScoreManager.Instance.AddJudgeResult(Judge.Miss);
                        GameManager.Instance.ReleaseNote(noteLanes[idx].DequeueNote()); // Start 제거
                        GameManager.Instance.ReleaseNote(noteLanes[idx].DequeueNote()); // Body 제거
                        GameManager.Instance.ReleaseNote(noteLanes[idx].DequeueNote()); // End 제거
                    }

                    note.data.isJudgeDone = true;
                }
            }
            else
            {
                Judgement(note);
            }
        }
    }

    private IEnumerator LongBodyJudge(Note note)
    {
        float interval = 100f; // ms
        
        delay ??= new WaitForSeconds(interval / 1000f);

        float holdEndTime = note.data.longNoteData.startTime; // startTime = 노트가 적중할 타이밍
    
        float nextJudgeTime = note.data.startTime + interval; // 첫 노트가 시작할 타이밍

        // GameManager.Instance.time // 노래의 진행도

        int judgeTick = 1;
        int judgeMaxTick = (int)(note.data.endTime / interval);

        while (Input.GetKey(onKey[note.data.keyPos]) && GameManager.Instance.time <= holdEndTime)
        {
            if (GameManager.Instance.time >= nextJudgeTime)
            {
                ScoreManager.Instance.AddJudgeResult(Judge.Perfect);
                nextJudgeTime += interval;
                judgeTick++;

                if (judgeMaxTick == judgeTick)
                {
                    break;                
                }
            }
            yield return delay;
        }

        note.data.isJudgeDone = true;
        GameManager.Instance.ReleaseNote(noteLanes[note.data.keyPos].DequeueNote()); // Body 삭제

        Note endNote = noteLanes[note.data.keyPos].GetNote(); // End 가져옴
        endNote.data.isJudgeDone = true;
        GameManager.Instance.ReleaseNote(noteLanes[note.data.keyPos].DequeueNote());

        if (GameManager.Instance.time + 150 >= holdEndTime - 160)
        {
            if (judgeMaxTick != judgeTick)
            {
                for (int i = 0; i < judgeMaxTick - judgeTick; i++)
                {
                    ScoreManager.Instance.AddJudgeResult(Judge.Perfect);
                }
            }
            
            ScoreManager.Instance.AddJudgeResult(Judge.Perfect);
        }
        else
        {
            ScoreManager.Instance.AddJudgeResult(Judge.Miss);
        }        
    }

    private void Judgement(Note note)
    {
        checkMs = note.data.startTime - GameManager.Instance.time;

        note.data.isJudgeDone = true;

        if (checkMs < judgeMs[(int)Judge.Perfect] && checkMs >= -judgeMs[(int)Judge.Perfect])
        {
            ScoreManager.Instance.AddJudgeResult(Judge.Perfect);
        }
        else if (checkMs <= judgeMs[(int)Judge.Great] && checkMs >= -judgeMs[(int)Judge.Great])
        {
            ScoreManager.Instance.AddJudgeResult(Judge.Great);
        }
        else if (checkMs <= judgeMs[(int)Judge.Good] && checkMs >= -judgeMs[(int)Judge.Good])
        {
            ScoreManager.Instance.AddJudgeResult(Judge.Good);
        }
        else
        {
            ScoreManager.Instance.AddJudgeResult(Judge.Miss);
        }

        GameManager.Instance.ReleaseNote(noteLanes[note.data.keyPos].DequeueNote());
    }

    public void AddNoteList(int idx, Note note)
    {
        noteLanes[idx].AddNote(note);
    }

    public void Miss(Note note)
    {
        ScoreManager.Instance.AddJudgeResult(Judge.Miss);
        GameManager.Instance.ReleaseNote(noteLanes[note.data.keyPos].DequeueNote());
    }
}
