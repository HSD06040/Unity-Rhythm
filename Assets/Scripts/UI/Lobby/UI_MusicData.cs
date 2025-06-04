using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MusicData : BaseUI
{
    [SerializeField] private VertexGradient[] gradients;
    [SerializeField] private Image musicIcon;
    [SerializeField] private float musicIconSpeed;
    private PlayData musicPlayData;

    [Header("PlayData_UI")]
    private TMP_Text rank;
    private TMP_Text comboText;
    private TMP_Text scoreText;
    private TMP_Text rateText;
    private TMP_Text resSpeed;
    private TMP_Text[] judgeText = new TMP_Text[4];

    [Header("Music Difficulty")]
    private TMP_Text musicDifficultySpeedText;

    protected override void Awake()
    {
        base.Awake();

        rank = GetUI<TextMeshProUGUI>("Rank");

        musicDifficultySpeedText = GetUI<TextMeshProUGUI>("MusicSpeed");
        comboText = GetUI<TextMeshProUGUI>("Combo");
        scoreText = GetUI<TextMeshProUGUI>("Score");
        rateText = GetUI<TextMeshProUGUI>("Rate");
        resSpeed = GetUI<TextMeshProUGUI>("Speed");

        judgeText[0] = GetUI<TextMeshProUGUI>("Perfect");
        judgeText[1] = GetUI<TextMeshProUGUI>("Great");
        judgeText[2] = GetUI<TextMeshProUGUI>("Good");
        judgeText[3] = GetUI<TextMeshProUGUI>("Miss");
    }

    private void Update()
    {
        musicIcon.transform.Rotate(0, 0, musicIconSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        GameManager.Instance.onScrollSpeedChanged += DifficultUpdate; 
    }

    private void OnDisable()
    {
        GameManager.Instance.onScrollSpeedChanged -= DifficultUpdate;
    }

    public void UpdatePlayDataUI(MusicData data)
    {
        musicPlayData = Parser.LoadPlayData(data.bgm);

        if(musicPlayData != null)
        {
            musicIcon.sprite = data.icon;
            rank.colorGradient = gradients[(int)musicPlayData.rank];
            rank.text = musicPlayData.rank.ToString();
            comboText.text = musicPlayData.maxCombo.ToString();
            scoreText.text = musicPlayData.score.ToString();
            rateText.text = musicPlayData.rate.ToString();
            resSpeed.text = musicPlayData.resSpeed.ToString();

            judgeText[0].text = musicPlayData.m100.ToString();
            judgeText[1].text = musicPlayData.m90.ToString();
            judgeText[2].text = musicPlayData.m80.ToString();
            judgeText[3].text = musicPlayData.m70.ToString();
        }
        else
        {
            musicIcon.sprite = data.icon;
            rank.color = Color.clear;
            comboText.text = "0";
            scoreText.text = "0";
            rateText.text = "0";
            resSpeed.text = "0";

            for (int i = 0; i < judgeText.Length; i++)
            {
                judgeText[i].text = "0";
            }
        }

        GameManager.Instance.scrollSpeed = 1;
        StartCoroutine(RotateRoutine());
    }

    private IEnumerator RotateRoutine()
    {
        Quaternion startRot = musicIcon.transform.rotation;
       
        Quaternion targetRot = Quaternion.Euler(
                musicIcon.transform.eulerAngles.x,
                musicIcon.transform.eulerAngles.y,
                musicIcon.transform.eulerAngles.z + 90
            );

        float duration = 0.4f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            musicIcon.transform.rotation = Quaternion.Lerp(startRot, targetRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        musicIcon.transform.rotation = targetRot;
    }

    private void DifficultUpdate(float amount) => musicDifficultySpeedText.text = amount.ToString("F1");
}
