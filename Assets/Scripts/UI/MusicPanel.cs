using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPanel : MonoBehaviour
{
    [SerializeField] private List<MusicData> musicDatas;
    [SerializeField] private List<MusicBar> musicBars;
    [SerializeField] private GameObject musicBarPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform content;
    private int currentIdx;

    private void Awake()
    {
        for (int i = 0; i < musicDatas.Count; i++)
        {
            musicBars.Add(Instantiate(musicBarPrefab, content).GetComponent<MusicBar>());
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
    }
    private void ChangeSelection(int direction)
    {
    // 이전 선택 해제
        musicBars[currentIdx].SetSelected(false);

    // 인덱스 변경 (Clamp or Wrap 방식 중 택)
        currentIdx += direction;
        currentIdx = Mathf.Clamp(currentIdx, 0, musicBars.Count - 1);

    // 새로운 선택 표시
        musicBars[currentIdx].SetSelected(true);

    // TODO: ScrollRect로 자동 스크롤 맞추기 (필요 시)
    }

    private void ScrollToCurrent()
    {
        float itemCount = musicBars.Count;
        float normalizePosition = 1f - (currentIdx / (itemCount - 1))
    }
}