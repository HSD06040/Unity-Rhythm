using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_JudgeCombo : BaseUI
{
    private TMP_Text judgeText;
    private TMP_Text scoreText;
    private TMP_Text comboText;
    private TMP_Text rateText;

    private float totalRate;
    private int rateCount;
    private float rate => totalRate / rateCount;

    protected override void Awake()
    {
        base.Awake();

        judgeText = GetUI<TextMeshProUGUI>("Judge");
        scoreText = GetUI<TextMeshProUGUI>("Score");
        comboText = GetUI<TextMeshProUGUI>("Combo");
        rateText = GetUI<TextMeshProUGUI>("Rate");
    }    

    private void OnEnable()
    {
        ScoreManager.Instance.onJudged += JudgeUpdate;
        ScoreManager.Instance.comboCount.AddEvent(ComboUpdate);
        ScoreManager.Instance.score.AddEvent(ScoreUpdate);
    }

    private void OnDisable()
    {
        ScoreManager.Instance.onJudged -= JudgeUpdate;
        ScoreManager.Instance.comboCount.RemoveEvent(ComboUpdate);
        ScoreManager.Instance.score.RemoveEvent(ScoreUpdate);
    }

    private void JudgeUpdate(Judge judge)
    {
        switch (judge)
        {
            case Judge.Perfect:
                judgeText.transform.localScale = Vector3.one;
                totalRate += 100;                
                judgeText.text = "Max 100 %";
                break;

            case Judge.Great:
                judgeText.transform.localScale = new Vector3 (.9f, .9f, .9f);
                totalRate += 90;
                judgeText.text = "Max 90 %";
                break;

            case Judge.Good:
                judgeText.transform.localScale = new Vector3(.7f, .7f, .7f);
                totalRate += 70;
                judgeText.text = "Max 70 %";
                break;

            default:
                judgeText.transform.localScale = new Vector3(.7f, .7f, .7f);
                judgeText.text = "Miss";
                break;
        }

        rateCount++;

        RateUpdate();
    }

    private void RateUpdate()
    {
        rateText.text = rate.ToString("F2");
    }

    private void ComboUpdate(int count)
    {
        comboText.text = count.ToString();
    }

    private void ScoreUpdate(int amount)
    {
        scoreText.text = amount.ToString();
    }
}
