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
        comboList.Clear();
        rateCount = 0;
        totalRate = 0;
        score.Value = 0;
        comboCount.Value = 0;
        judgeResult = new int[(int)Judge.Miss + 1];
    }

    private void AddScore(int amount)
    {
        comboCount.Value++;      
        float multiplier = 1 + comboCount.Value / 100;
        score.Value += Mathf.RoundToInt(amount * multiplier);
    }

    public void AddJudgeResult(Judge judge, int idx)
    {
        judgeResult[(int)judge]++;

        if (judge != Judge.Miss)
        {
            AddScore(judge switch
            {
                Judge.M100 => 100,
                Judge.M90 => 80,
                Judge.M80 => 63,
                Judge.M70 => 50,
                Judge.M60 => 40,
                Judge.M50 => 33,
                Judge.M40 => 30,
                Judge.M30 => 29,
                Judge.M20 => 28,
                Judge.M10 => 25,
                Judge.M1 => 1,
                _ => 0
            });

            totalRate += judge switch
            {
                Judge.M100 => 100,
                Judge.M90 => 80,
                Judge.M80 => 63,
                Judge.M70 => 50,
                Judge.M60 => 40,
                Judge.M50 => 33,
                Judge.M40 => 30,
                Judge.M30 => 29,
                Judge.M20 => 28,
                Judge.M10 => 25,
                Judge.M1 => 1,
                _ => 0
            };

            Vector3 effectPos = GameManager.Instance.judgeLine.position + new Vector3(JudgeManager.Instance.offset[idx], 0, 0);
            EffectManager.Instance.CreateEffect("Judge", effectPos, null);
        }
        else
        {
            ResetComboCount();
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

            m100 = judgeResult[0],
            m90 = judgeResult[1],
            m80 = judgeResult[2],
            m70 = judgeResult[3],
            m60 = judgeResult[4],
            m50 = judgeResult[5],
            m40 = judgeResult[6],
            m30 = judgeResult[7],
            m20 = judgeResult[8],
            m10 = judgeResult[9],
            m1 = judgeResult[10],
            miss = judgeResult[11],
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
