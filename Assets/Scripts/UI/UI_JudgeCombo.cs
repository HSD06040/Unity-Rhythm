using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_JudgeCombo : MonoBehaviour
{
    [SerializeField] private TMP_Text judgeText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text rateText;
    private float totalRate;
    private int rateCount;
    private float rate => totalRate / rateCount;

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
        rateText.text = rate.ToString();
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
