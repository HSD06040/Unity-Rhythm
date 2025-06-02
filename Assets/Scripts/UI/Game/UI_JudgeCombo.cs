using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_JudgeCombo : BaseUI
{
    [SerializeField] private VertexGradient[] gradients;
    private TMP_Text judgeText;
    private TMP_Text scoreText;
    private TMP_Text comboText;
    private TMP_Text rateText;
    private TMP_Text speedText;

    private Animator judgeAnim;
    private Animator comboAnim;

    private static readonly int inHash = Animator.StringToHash("In");

    private readonly string[] judgeTexts = new string[]
    {
        "MAX100%", "MAX90%", "MAX80%", "MAX70%", "MAX60%",
        "MAX50%", "MAX40%", "MAX30%", "MAX20%", "MAX10%",
        "MAX1%", "Miss"
    };
    protected override void Awake()
    {
        base.Awake();

        judgeText = GetUI<TextMeshProUGUI>("Judge");
        scoreText = GetUI<TextMeshProUGUI>("Score");
        comboText = GetUI<TextMeshProUGUI>("Combo");
        rateText = GetUI<TextMeshProUGUI>("Rate");
        speedText = GetUI<TextMeshProUGUI>("SpeedText");

        judgeAnim = GetUI<Animator>("Judge");
        comboAnim = GetUI<Animator>("Combo");

        speedText.text = GameManager.Instance.scrollSpeed.ToString();
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
        int idx = (int)judge;

        judgeAnim.SetTrigger(inHash);
        judgeText.colorGradient = gradients[idx];
        judgeText.text = judgeTexts[idx];

        RateUpdate();
    }

    private void RateUpdate()
    {
        rateText.text = ScoreManager.Instance.rate.ToString("F2");
    }

    private void ComboUpdate(int count)
    {
        comboAnim.SetTrigger(inHash);
        comboText.text = count.ToString();
    }

    private void ScoreUpdate(int amount)
    {
        scoreText.text = amount.ToString();
    }
}
