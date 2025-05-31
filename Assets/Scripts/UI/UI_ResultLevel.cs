using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResultLevel : BaseUI
{
    private TMP_Text level;
    private TMP_Text expText;
    private Slider expSlider;

    protected override void Awake()
    {
        base.Awake();

        level = GetUI<TextMeshProUGUI>("Level");
        expText = GetUI<TextMeshProUGUI>("Progress");
        expSlider = GetUI<Slider>("Slider");
    }

    private void Start()
    {
        level.text = DataManager.Instance.level.Value.ToString();
        StartCoroutine(ExpSliderRoutine());
    }

    private IEnumerator ExpSliderRoutine()
    {
        float levelProprtion = DataManager.Instance.GetLevelProportion();
        string textString;

        while (expSlider.value != levelProprtion - 0.1f)
        {
            expSlider.value = Mathf.Lerp(expSlider.value, levelProprtion, 3 * Time.deltaTime);
            textString = (expSlider.value * 100).ToString("F0");
            expText.text = $"{textString}%";
            yield return null;
        }

        expSlider.value = levelProprtion;
        textString = (expSlider.value * 100).ToString("F0");
        expText.text = $"{textString}%";
    }
}
