using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Manager<ScoreManager>
{
    public Property<int> score = new();
    public Property<int> comboCount = new();
    public int[] judgeResult;

    public float rate => totalRate / rateCount;
    private int rateCount;
    private int totalRate;

    public event Action<Judge> onJudged;

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
            case Judge.Perfect: AddScore(100); totalRate += 100; break;
            case Judge.Great: AddScore(70); totalRate += 90;  break;
            case Judge.Good: AddScore(50); totalRate += 70; break;
            case Judge.Miss: ResetComboCount(); break;
        }

        rateCount++;
        onJudged?.Invoke(judge);
    }

    public void ResetComboCount() => comboCount.Value = 0;

    public void SavePlayData(BGM bgm)
    {
        PlayData playData = new PlayData
        {
            bgm = bgm,
            combo = comboCount.Value,
            score = score.Value,
            rank = Rank.S,
            rate = rate,
            resSpeed = GameManager.Instance.resSpeed,

            perfect = judgeResult[0],
            greate = judgeResult[1],
            good = judgeResult[2],
            miss = judgeResult[3],
        };

        Parser.SavePlayData(playData);
    }

    private void GetRank() // return Rank
    {
        // TODO :: ·©Å© ±âÁØÀ¸·Î ·©Å© ¹ÝÈ¯
    }
}
