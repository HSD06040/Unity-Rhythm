using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ScoreManager : Manager<ScoreManager>
{
    public Property<int> score = new();
    public Property<int> comboCount = new();
    public int[] judgeResult;

    public float rate => totalRate / rateCount;
    private int rateCount;
    private float totalRate;

    public event Action<Judge> onJudged;
    private List<int> comboList = new();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitPlayData();
    }

    public void InitPlayData()
    {
        rateCount = 0;
        totalRate = 0;
        score.Value = 0;
        comboCount.Value = 0;
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

    public void ResetComboCount()
    {
        comboList.Add(comboCount.Value);
        comboCount.Value = 0;
    }

    public void SavePlayData(BGM bgm)
    {        
        PlayData playData = new PlayData
        {
            bgm = bgm,
            maxCombo = GetBestComboCount(),
            score = score.Value,
            rank = Rank.S,
            rate = rate,
            resSpeed = GameManager.Instance.scrollSpeed,

            perfect = judgeResult[0],
            greate = judgeResult[1],
            good = judgeResult[2],
            miss = judgeResult[3],
        };

        PlayData oldMostPlayData = Parser.LoadPlayData(bgm);

        if (oldMostPlayData == null || oldMostPlayData.score > score.Value)
        {
            GameManager.Instance.currnetPlayData = playData;
        }
        else
        {
            Parser.SavePlayData(playData);
            GameManager.Instance.currnetPlayData = playData;
        }            
    }

    private void GetRank() // return Rank
    {
        // TODO :: ·©Å© ±âÁØÀ¸·Î ·©Å© ¹ÝÈ¯
    }

    private int GetBestComboCount()
    {
        int max = 0;

        foreach (var combo in comboList)
        {
            if(combo > max)
            {
                max = combo;
            }
        }

        return max;
    }
}
