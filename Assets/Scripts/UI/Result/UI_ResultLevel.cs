using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResultLevel : BaseUI
{
    private TMP_Text level;
    private TMP_Text expText;
    private Slider expSlider1;
    private Slider expSlider2;
    [SerializeField] private GameObject levelUp;

    protected override void Awake()
    {
        base.Awake();

        level = GetUI<TextMeshProUGUI>("Level");
        expText = GetUI<TextMeshProUGUI>("Progress");
        expSlider1 = GetUI<Slider>("Slider1");
        expSlider2 = GetUI<Slider>("Slider");
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

        while (Mathf.Abs(expSlider1.value - levelProprtion) > 0.01f)
        {
            expSlider1.value = Mathf.Lerp(expSlider1.value, levelProprtion, 3 * Time.deltaTime);

            textString = (expSlider1.value * 100).ToString("F0");
            expText.text = $"{textString}%";

            yield return null;
        }

        expSlider1.value = levelProprtion;

        textString = (expSlider1.value * 100).ToString("F0");
        expText.text = $"{textString}%";

        DataManager.Instance.AddExp(GameManager.Instance.currnetPlayData.score / 300);

        StartCoroutine(ExpSliderNextRoutine());
    }

    private IEnumerator ExpSliderNextRoutine()
    {
        while (true)
        {
            float oldLevelProportion = DataManager.Instance.GetLevelProportion();
            int oldLevel = DataManager.Instance.level.Value;

            DataManager.Instance.AddExp(GameManager.Instance.currnetPlayData.score / 300);

            float newLevelProportion = DataManager.Instance.GetLevelProportion();
            int newLevel = DataManager.Instance.level.Value;

            expSlider2.value = oldLevelProportion;

            float t = 0f;
            string textString;

            while (true)
            {
                t += Time.deltaTime * 2.5f;
                expSlider2.value = Mathf.Lerp(expSlider2.value, newLevelProportion, t);

                textString = (expSlider2.value * 100).ToString("F0");
                expText.text = $"{textString}%";

                if (DataManager.Instance.isLevelUp)
                {
                    levelUp.SetActive(true);
                    DataManager.Instance.isLevelUp = false;

                    expSlider2.value = 1f;

                    yield return new WaitForSeconds(0.2f);

                    level.text = DataManager.Instance.level.Value.ToString();

                    expSlider1.value = 0f;
                    expSlider2.value = 0f;

                    yield return new WaitForSeconds(0.2f);

                    break;
                }

                if (Mathf.Abs(expSlider2.value - newLevelProportion) < 0.01f)
                {
                    expSlider2.value = newLevelProportion;
                    break;
                }

                yield return null;
            }

            if (!DataManager.Instance.isLevelUp)
                break;

            yield return null;
        }

        expText.text = $"{(DataManager.Instance.GetLevelProportion() * 100).ToString("F0")}%";
    }
}
