using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UserPanel : BaseUI
{
    private TMP_Text userLevel;
    private TMP_Text userName;
    private TMP_Text expText;
    private Slider expSlider;    

    protected override void Awake()
    {
        base.Awake();

        userLevel = GetUI<TextMeshProUGUI>("UserLevel");
        userName = GetUI<TextMeshProUGUI>("UserName");
        expText = GetUI<TextMeshProUGUI>("ExpText");
        expSlider = GetUI<Slider>("ExpSlider");
    }

    private void Start()
    {
        userLevel.text = DataManager.Instance.level.Value.ToString();
        userName.text = DataManager.Instance.playerName;

        StartCoroutine(ExpSliderRoutine());
    }

    private IEnumerator ExpSliderRoutine()
    {
        yield return new WaitForSeconds(.5f);                

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
