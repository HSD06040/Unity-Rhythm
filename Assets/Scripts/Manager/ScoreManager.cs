using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ScoreManager : Manager<ScoreManager>
{
    public Property<int> score = new();
    public Property<int> comboCount = new();
    public Property<float> comboGauge = new(); 
    public int[] judgeResult;
    public ComboState[] comboState;
    public int noteCount;

    public float rate => totalRate / rateCount;
    private int rateCount;
    private float totalRate;

    public event Action<Judge> onJudged;
    public event Action<ComboState> onComboStateChanged;  

    private List<int> comboList = new();
    private ComboState currentState;
    private int stateIdx;

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
        currentState = ComboState.MAX1;
        stateIdx = 0;
        rateCount = 0;
        totalRate = 0;
        score.Value = 0;
        comboCount.Value = 0;
        comboGauge.Value = 0;
        judgeResult = new int[(int)Judge.Miss + 1];
    }

    private void AddScore(int amount)
    {
        comboCount.Value += (int)currentState;      
        float multiplier = 1 + comboCount.Value / 3000;
        score.Value += Mathf.RoundToInt(amount * multiplier);
    }

    private void AddComboGauge(Judge judge)
    {
        switch(judge)
        {
            case Judge.M100:
                comboGauge.Value += 0.017f; break;
            case Judge.M90:
                comboGauge.Value += 0.012f; break;
            case Judge.M80:
                comboGauge.Value += 0.01f; break;
            case Judge.M70:
                comboGauge.Value += 0.008f; break;
            case Judge.M60:
                comboGauge.Value += 0.007f; break;
            case Judge.M50:
                comboGauge.Value += 0.005f; break;
            case Judge.M40:
                comboGauge.Value += 0.004f; break;
            case Judge.M30:
                comboGauge.Value += 0.003f; break;
            case Judge.M20:
                comboGauge.Value += 0.002f; break;
            case Judge.M10:
                comboGauge.Value += 0.001f; break;
            case Judge.M1:
                comboGauge.Value += 0.0007f; break;
            case Judge.Miss:
                break;
        }
    }

    public void AddJudgeResult(Judge judge, int idx)
    {
        judgeResult[(int)judge]++;

        if (judge != Judge.Miss)
        {
            AddScore(judge switch
            {
                Judge.M100 => (300000 / noteCount) * 1,
                Judge.M90 => (int)((300000 / noteCount) * .8f),
                Judge.M80 => (int)((300000 / noteCount) * .63f),
                Judge.M70 => (int)((300000 / noteCount) * .5f),
                Judge.M60 => (int)((300000 / noteCount) * .4f),
                Judge.M50 => (int)((300000 / noteCount) * .33f),
                Judge.M40 => (int)((300000 / noteCount) * .3f),
                Judge.M30 => (int)((300000 / noteCount) * .29f),
                Judge.M20 => (int)((300000 / noteCount) * .28f),
                Judge.M10 => (int)((300000 / noteCount) * .25f),
                Judge.M1 => (int)((300000 / noteCount) * .01f),
                _ => 0
            });

            totalRate += judge switch
            {
                Judge.M100 => 100,
                Judge.M90 => 90,
                Judge.M80 => 80,
                Judge.M70 => 70,
                Judge.M60 => 60,
                Judge.M50 => 50,
                Judge.M40 => 40,
                Judge.M30 => 30,
                Judge.M20 => 20,
                Judge.M10 => 10,
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

        AddComboGauge(judge);
        rateCount++;
        onJudged?.Invoke(judge);
    }

    public void UpgradeState()
    {
        if (stateIdx == 3)
        {
            comboCount.Value += 20;
            return;
        }
        stateIdx++;

        currentState = comboState[stateIdx];
        onComboStateChanged?.Invoke(currentState);

        return;
    }

    public void ResetComboCount()
    {
        comboList.Add(comboCount.Value);
        comboCount.Value = 0;
        stateIdx = 0;
        comboGauge.Value = 0;
        currentState = ComboState.MAX1;
        onComboStateChanged?.Invoke(currentState);
    }

    public void SavePlayData(BGM bgm)
    {        
        PlayData playData = new PlayData
        {
            bgm = bgm,
            maxCombo = GetBestComboCount(),
            score = score.Value,
            rank = GetRank(),
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

        if (oldMostPlayData == null || oldMostPlayData.score < score.Value)
        {
            Parser.SavePlayData(playData);          
        }

        GameManager.Instance.currnetPlayData = playData;
    }

    private Rank GetRank()
    {
        if (rate > 90) return Rank.S;
        else if (rate > 70) return Rank.A;
        else if (rate > 70) return Rank.B;
        else if (rate > 50) return Rank.C;
        else return Rank.D;
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
