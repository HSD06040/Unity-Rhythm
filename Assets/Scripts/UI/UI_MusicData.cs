using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MusicData : BaseUI
{
    private Image musicIcon;
    private PlayData musicPlayData;
    [SerializeField] private Sprite rankNoneImage;

    [Header("PlayData_UI")]
    private Image rankImage;
    private TMP_Text comboText;
    private TMP_Text scoreText;
    private TMP_Text rateText;
    private TMP_Text resSpeed;
    private TMP_Text[] judgeText = new TMP_Text[4];

    public void UpdatePlayDataUI(MusicData data)
    {
        musicPlayData = Parser.LoadPlayData(data.bgm);

        if(musicPlayData != null)
        {
            musicIcon.sprite = data.icon;
            rankImage.sprite = data.icon; // 랭크 이미지로 추후 교체
            comboText.text = musicPlayData.combo.ToString();
            scoreText.text = musicPlayData.score.ToString();
            rateText.text = musicPlayData.rate.ToString();
            resSpeed.text = musicPlayData.resSpeed.ToString();

            judgeText[0].text = musicPlayData.perfect.ToString();
            judgeText[1].text = musicPlayData.greate.ToString();
            judgeText[2].text = musicPlayData.good.ToString();
            judgeText[3].text = musicPlayData.miss.ToString();
        }
        else
        {
            musicIcon.sprite = data.icon;
            rankImage.sprite = rankNoneImage;
            comboText.text = "0";
            scoreText.text = "0";
            rateText.text = "0";
            resSpeed.text = "0";

            for (int i = 0; i < judgeText.Length; i++)
            {
                judgeText[0].text = "0";
            }
        }
    }
}
