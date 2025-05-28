using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Manager<ScoreManager>
{
    public Property<int> score = new();
    public Property<int> comboCount = new();
    public int[] judgeResult;

    public event Action<string> onJudged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        judgeResult = new int[4];
    }

    private void AddScore(int amount)
    {
        comboCount.Value++;
        float multiplier = 1 + comboCount.Value * 0.01f;
        score.Value += Mathf.RoundToInt(amount * multiplier);
    }

    public int GetJudgeResult(Judge judge)
    {
        return judgeResult[(int)judge];
    }

    public void AddJudgeResult(Judge judge)
    {
        judgeResult[(int)judge]++;

        switch (judge)
        {
            case Judge.Perfect: AddScore(100); break;
            case Judge.Great: AddScore(70); break;
            case Judge.Good: AddScore(50); break;
            case Judge.Miss: ResetComboCount(); break;
        }

        onJudged?.Invoke(judge.ToString());
    }

    public void ResetComboCount() => comboCount.Value = 0;
}
