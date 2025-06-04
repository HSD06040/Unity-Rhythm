using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Particular : BaseUI
{
    private TMP_Text[] particularTexts = new TMP_Text[(int)Judge.Miss+1];
    private Animator anim;
    private bool isOpen;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();

        particularTexts[(int)Judge.M100] = GetUI<TextMeshProUGUI>("M100");
        particularTexts[(int)Judge.M90] = GetUI<TextMeshProUGUI>("M90");
        particularTexts[(int)Judge.M80] = GetUI<TextMeshProUGUI>("M80");
        particularTexts[(int)Judge.M70] = GetUI<TextMeshProUGUI>("M70");
        particularTexts[(int)Judge.M60] = GetUI<TextMeshProUGUI>("M60");
        particularTexts[(int)Judge.M50] = GetUI<TextMeshProUGUI>("M50");
        particularTexts[(int)Judge.M40] = GetUI<TextMeshProUGUI>("M40");
        particularTexts[(int)Judge.M30] = GetUI<TextMeshProUGUI>("M30");
        particularTexts[(int)Judge.M20] = GetUI<TextMeshProUGUI>("M20");
        particularTexts[(int)Judge.M10] = GetUI<TextMeshProUGUI>("M10");
        particularTexts[(int)Judge.M1] = GetUI<TextMeshProUGUI>("M1");
        particularTexts[(int)Judge.Miss] = GetUI<TextMeshProUGUI>("Miss");
    }

    private void Start()
    {
        PlayData playData = GameManager.Instance.currnetPlayData;

        particularTexts[(int)Judge.M100].text = playData.m100.ToString();
        particularTexts[(int)Judge.M90].text = playData.m90.ToString();
        particularTexts[(int)Judge.M80].text = playData.m80.ToString();
        particularTexts[(int)Judge.M70].text = playData.m70.ToString();
        particularTexts[(int)Judge.M60].text = playData.m60.ToString();
        particularTexts[(int)Judge.M50].text = playData.m50.ToString();
        particularTexts[(int)Judge.M40].text = playData.m40.ToString();
        particularTexts[(int)Judge.M30].text = playData.m30.ToString();
        particularTexts[(int)Judge.M20].text = playData.m20.ToString();
        particularTexts[(int)Judge.M10].text = playData.m10.ToString();
        particularTexts[(int)Judge.M1].text = playData.m1.ToString();
        particularTexts[(int)Judge.Miss].text = playData.miss.ToString();
    }

    public void Change()
    {
        if (isOpen)
            ClosePanel();
        else
            OpenPanel();
    }

    private void OpenPanel()
    {
        anim.SetTrigger("In");
        isOpen = true;
    }
    private void ClosePanel()
    {
        anim.SetTrigger("Out");
        isOpen = false;
    }
}
