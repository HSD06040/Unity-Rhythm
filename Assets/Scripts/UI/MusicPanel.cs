using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPanel : MonoBehaviour
{
    [Header("MusicDatas")]
    [SerializeField] private List<MusicData> musicDatas;
    [SerializeField] private List<MusicBar> musicBars;
    [SerializeField] private List<RectTransform> musicBarRects;
    [SerializeField] private GameObject musicBarPrefab;
    [Space]
    
    [SerializeField] private UI_MusicData musicDataPanel;

    [Header("Scroll")]
    [SerializeField] private float scrollSpeed = 5f;
    private float targetScrollPos;
    private bool isScrolling;   
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewport;
    [SerializeField] private Transform content;
    private RectTransform contentRect;

    private int currentIdx;
    private float timer;
    private Coroutine changeMusicData;

    private void Awake()
    {
        for (int i = 0; i < musicDatas.Count; i++)
        {
            MusicBar bar = Instantiate(musicBarPrefab, content).GetComponent<MusicBar>();
            musicBars.Add(bar);
            musicBarRects.Add(bar.GetComponent<RectTransform>());
            musicBars[i].SetMusicBar(musicDatas[i].icon, musicDatas[i].bgm.ToString());
        }

        if(musicBars.Count > 0)
        {
            currentIdx = 0;
            musicBars[currentIdx].SetSelected(true);
        }
    }

    private void OnEnable()
    {
        currentIdx = 0;
        musicDataPanel.UpdatePlayDataUI(musicDatas[currentIdx]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelection(-1);
        }

        if (isScrolling)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(
                scrollRect.verticalNormalizedPosition,
                targetScrollPos,
                Time.deltaTime * scrollSpeed
            );

            if (Mathf.Abs(scrollRect.verticalNormalizedPosition - targetScrollPos) < 0.001f)
            {
                scrollRect.verticalNormalizedPosition = targetScrollPos;
                isScrolling = false;
            }
        }
    }
    private void ChangeSelection(int direction)
    {
        musicBars[currentIdx].SetSelected(false);

        int temIdx = currentIdx;
        currentIdx += direction;
        currentIdx = Mathf.Clamp(currentIdx, 0, musicBars.Count - 1);

        musicBars[currentIdx].SetSelected(true);

        ScrollToSelected();

        if (temIdx == currentIdx) return;

        if(changeMusicData != null)
        {
            StopCoroutine(changeMusicData);        
            changeMusicData = null;
        }

        changeMusicData = StartCoroutine(ChangeMusic());
    }

    private void ScrollToSelected()
    {
        RectTransform selectedRect = musicBarRects[currentIdx];

        contentRect ??= content.GetComponent<RectTransform>();
        
        float contentHeight = contentRect.rect.height;
        float viewportHeight = viewport.rect.height;

        float itemPos = Mathf.Abs(selectedRect.anchoredPosition.y) + (selectedRect.rect.height / 2f);

        float targetPos = (itemPos - (viewportHeight / 2f)) / (contentHeight - viewportHeight);
        targetPos = Mathf.Clamp01(1f - targetPos);

        targetScrollPos = targetPos;
        isScrolling = true;
    }

    private IEnumerator ChangeMusic()
    {
        timer = .7f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            yield return null;
        }

        musicDataPanel.UpdatePlayDataUI(musicDatas[currentIdx]);
    }
}