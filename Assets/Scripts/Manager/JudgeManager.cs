using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class JudgeManager : Manager<JudgeManager>, ISavable
{
    [Header("Inputs")]
    public InputActionAsset inputActions;
    public InputAction[] lanes;
    [Space]

    public NoteLane[] noteLanes;
    [SerializeField] private float[] judgeMs;
    [SerializeField] private float[] defaultJudgeMs;
    public float[] offset = new float[] { -1.8f, -.6f, .6f, 1.8f };

    private float checkMs;

    private YieldInstruction delay;

    protected override void Awake()
    {
        base.Awake();
        judgeMs = new float[12];

        var playerMap = inputActions.FindActionMap("Player");

        lanes = new InputAction[4];

        lanes[0] = playerMap.FindAction("Lane1");
        lanes[1] = playerMap.FindAction("Lane2");
        lanes[2] = playerMap.FindAction("Lane3");
        lanes[3] = playerMap.FindAction("Lane4");

        noteLanes = new NoteLane[4];
        for (int i = 0; i < noteLanes.Length; i++)
            noteLanes[i] = new NoteLane();
    }

    private void OnEnable()
    {
        lanes[0].Enable();
        lanes[1].Enable();
        lanes[2].Enable();
        lanes[3].Enable();
    }

    private void Update()
    {
        if (UI_Manager.Instance.isPause || UI_Manager.Instance.isMenu) return;

        Judgement(0);
        Judgement(1);
        Judgement(2);
        Judgement(3);
    }

    public void SetJudgeMs()
    {
        for (int i = 0; i < defaultJudgeMs.Length; i++)
        {
            judgeMs[i] = defaultJudgeMs[i];
        }
    }

    public void InitLanes()
    {
        foreach (var noteLane in noteLanes)
        {
            noteLane.Clear();
        }
    }

    private void Judgement(int idx)
    {
        if (lanes[idx].WasPressedThisFrame())
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
                    Judge judge = GetJudgement(note);

                    if (judge == Judge.Miss)
                    {
                        ScoreManager.Instance.AddJudgeResult(judge, note.data.keyPos);
                        GameManager.Instance.ReleaseNote(noteLanes[idx].DequeueNote()); // Start 제거
                        GameManager.Instance.ReleaseNote(noteLanes[idx].DequeueNote()); // Body 제거
                        GameManager.Instance.ReleaseNote(noteLanes[idx].DequeueNote()); // End 제거
                        return;
                    }

                    ScoreManager.Instance.AddJudgeResult(judge, note.data.keyPos);

                    GameManager.Instance.ReleaseNote(noteLanes[idx].DequeueNote()); // Start 제거
                    StartCoroutine(LongBodyJudge(noteLanes[idx].GetNextLongBody(), judge));


                    note.data.isJudgeDone = true;
                }
            }
            else
            {
                Judgement(note);
            }
        }
    }

    private IEnumerator LongBodyJudge(Note note, Judge judge)
    {
        float interval = 100f; // ms

        delay ??= new WaitForSeconds(interval / 1000f);

        float holdEndTime = note.data.longNoteData.startTime; // startTime = 노트가 적중할 타이밍

        float nextJudgeTime = note.data.startTime + interval; // 첫 노트가 시작할 타이밍

        // GameManager.Instance.time // 노래의 진행도

        int judgeTick = 1;
        int judgeMaxTick = (int)(note.data.endTime / interval);

        while (lanes[note.data.keyPos].IsPressed() && GameManager.Instance.time <= holdEndTime)
        {
            if (GameManager.Instance.time >= nextJudgeTime)
            {
                ScoreManager.Instance.AddJudgeResult(judge, note.data.keyPos);
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

        if (GameManager.Instance.time + 150 >= holdEndTime - 150)
        {
            if (judgeMaxTick != judgeTick)
            {
                for (int i = 0; i < judgeMaxTick - judgeTick; i++)
                {
                    ScoreManager.Instance.AddJudgeResult(judge, note.data.keyPos);
                }
            }

            ScoreManager.Instance.AddJudgeResult(judge, note.data.keyPos);
        }
        else
        {
            ScoreManager.Instance.AddJudgeResult(Judge.Miss, note.data.keyPos);
        }
    }

    private void Judgement(Note note)
    {
        checkMs = note.data.startTime - GameManager.Instance.time;

        note.data.isJudgeDone = true;

        for (int i = 0; i < judgeMs.Length; i++)
        {
            if (checkMs <= judgeMs[i] && checkMs >= -judgeMs[i])
            {
                ScoreManager.Instance.AddJudgeResult((Judge)i, note.data.keyPos);
                GameManager.Instance.ReleaseNote(noteLanes[note.data.keyPos].DequeueNote());
                return;
            }
        }        
    }

    private Judge GetJudgement(Note note)
    {
        checkMs = note.data.startTime - GameManager.Instance.time;

        note.data.isJudgeDone = true;

        for (int i = 0; i < judgeMs.Length; i++)
        {
            if (checkMs <= judgeMs[i] && checkMs >= -judgeMs[i])
            {                
                return (Judge)i;
            }
        }

        return Judge.Miss;
    }

    public void AddNoteList(int idx, Note note)
    {
        noteLanes[idx].AddNote(note);
    }

    public void Miss(Note note)
    {
        ScoreManager.Instance.AddJudgeResult(Judge.Miss, note.data.keyPos);
        GameManager.Instance.ReleaseNote(noteLanes[note.data.keyPos].DequeueNote());
    }

    public void Save(ref GameData data)
    {
        data.keyBindingJson = inputActions.SaveBindingOverridesAsJson();
    }

    public void Load(GameData data)
    {
        if (string.IsNullOrEmpty(data.keyBindingJson)) return;

        inputActions.LoadBindingOverridesFromJson(data.keyBindingJson);
    }
}
