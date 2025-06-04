using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MaxBar : BaseUI
{
    [SerializeField] private VertexGradient[] gradients;
    private Slider maxSlider;
    private TMP_Text stateText;
    private Animator anim;

    protected override void Awake()
    {
        base.Awake();

        maxSlider = GetUI<Slider>("Mask");
        stateText = GetUI<TextMeshProUGUI>("StateText");
        anim = GetUI<Animator>("StateText");

        ScoreManager.Instance.comboGauge.AddEvent(UpdateMaxSlider);
        ScoreManager.Instance.onComboStateChanged += ChangeState;
    }

    private void UpdateMaxSlider(float value)
    {
        maxSlider.value = value;

        if (maxSlider.value == 1)
            MaxbarFull();
    }

    private void MaxbarFull()
    {        
        ScoreManager.Instance.comboGauge.Value = 0;
        ScoreManager.Instance.UpgradeState();      
    }

    private void ChangeState(ComboState comboState)
    {
        int i = 0;

        switch(comboState)
        {
            case ComboState.MAX1:
                i = 0; break;
            case ComboState.MAX2:
                i = 1; break;
            case ComboState.MAX3:
                i = 2; break;
            case ComboState.MAX5:
                i = 3; break;
        }

        anim.SetTrigger("In");

        stateText.colorGradient = gradients[i];
        stateText.text = comboState.ToString();
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.comboGauge.RemoveEvent(UpdateMaxSlider);
        ScoreManager.Instance.onComboStateChanged -= ChangeState;
    }
}
