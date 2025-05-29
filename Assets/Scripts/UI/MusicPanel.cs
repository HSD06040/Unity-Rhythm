using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPanel : MonoBehaviour
{
    [SerializeField] private List<MusicData> musicDatas;
    [SerializeField] private List<MusicBar> musicBars;
    [SerializeField] private List<RectTransform> musicBarRects;
    [SerializeField] private GameObject musicBarPrefab;

    [Header("Scroll")]
    [SerializeField] private float scrollSpeed = 5f;
    private float targetScrollPos;
    private bool isScrolling;

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewport;
    [SerializeField] private Transform content;
    private int currentIdx;

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

        currentIdx += direction;
        currentIdx = Mathf.Clamp(currentIdx, 0, musicBars.Count - 1);

        musicBars[currentIdx].SetSelected(true);

        ScrollToSelected();
    }

    //private void ScrollToSelected()
    //{
    //    RectTransform selectedRect = musicBarRects[currentIdx];

    //    // ���� -> ����Ʈ ���� ��ǥ�� ��ȯ
    //    Vector3 worldPos = selectedRect.position;
    //    Vector3 localPos = viewport.InverseTransformPoint(worldPos);

    //    float viewportHeight = viewport.rect.height;

    //    // '���� ����' ���� ���� (ex: ����Ʈ ���ϴ� 20%�� ��ũ�� Ʈ����)
    //    float upperThreshold = viewportHeight * 0.4f;
    //    float lowerThreshold = -viewportHeight * 0.4f;

    //    // localPos.y�� ����Ʈ ���� y�� (��: ���, �Ʒ�: ����)
    //    if (localPos.y > upperThreshold || localPos.y < lowerThreshold)
    //    {
    //        float itemPos = Mathf.Abs(selectedRect.anchoredPosition.y);
    //        float contentHeight = content.GetComponent<RectTransform>().rect.height;

    //        float targetPos = itemPos / (contentHeight - viewportHeight);
    //        targetPos = Mathf.Clamp01(1f - targetPos);

    //        targetScrollPos = targetPos;
    //        isScrolling = true;
    //    }
    //}
    private void ScrollToSelected()
    {
        RectTransform selectedRect = musicBarRects[currentIdx];

        float contentHeight = content.GetComponent<RectTransform>().rect.height;
        float viewportHeight = viewport.rect.height;

        // ������ �߽� ��ġ
        float itemPos = Mathf.Abs(selectedRect.anchoredPosition.y) + (selectedRect.rect.height / 2f);

        // �߾ӿ� ���� ��ũ�� ��ġ ����
        float targetPos = (itemPos - (viewportHeight / 2f)) / (contentHeight - viewportHeight);
        targetPos = Mathf.Clamp01(1f - targetPos);

        targetScrollPos = targetPos;
        isScrolling = true;
    }

}