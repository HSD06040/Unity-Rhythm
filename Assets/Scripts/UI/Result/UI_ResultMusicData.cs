using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResultMusicData : BaseUI
{
    [SerializeField] private GameObject starPerfab;
    [SerializeField] private GameObject emptyStarPrefab;

    private Image musicIcon;
    private TMP_Text speedText;
    private TMP_Text musicNameText;
    private TMP_Text artistNameText;
    private Transform startParent;

    private MusicData md;
    private int max = 15;    

    protected override void Awake()
    {
        base.Awake();

        speedText = GetUI<TextMeshProUGUI>("Speed");
        musicNameText = GetUI<TextMeshProUGUI>("MusicName");
        artistNameText = GetUI<TextMeshProUGUI>("ArtistName");
        musicIcon = GetUI<Image>("MusicIcon");
        startParent = GetUI<RectTransform>("Stars");
    }

    private void Start()
    {
        md = GameManager.Instance.currentMusicData;

        speedText.text = $"{GameManager.Instance.scrollSpeed.ToString()} Speed";
        musicNameText.text = md.musicName;
        artistNameText.text = md.artistName;

        musicIcon.sprite = md.icon;

        StartCoroutine(StarSpawnRoutine());
    }

    private IEnumerator StarSpawnRoutine()
    {
        YieldInstruction delay = new WaitForSeconds(.3f);

        for (int i = 0; i < md.difficulty; i++)
        {
            Instantiate(starPerfab, startParent);
            yield return delay;
        }

        for (int i = 0; i < max - md.difficulty; i++)
        {
            Instantiate(emptyStarPrefab, startParent);
            yield return delay;
        }
    }
}
