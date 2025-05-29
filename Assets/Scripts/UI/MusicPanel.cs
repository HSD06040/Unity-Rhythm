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
    // ���� ���� ����
        musicBars[currentIdx].SetSelected(false);

    // �ε��� ���� (Clamp or Wrap ��� �� ��)
        currentIdx += direction;
        currentIdx = Mathf.Clamp(currentIdx, 0, musicBars.Count - 1);

    // ���ο� ���� ǥ��
        musicBars[currentIdx].SetSelected(true);

    // TODO: ScrollRect�� �ڵ� ��ũ�� ���߱� (�ʿ� ��)
    }

    private void ScrollToCurrent()
    {
        float itemCount = musicBars.Count;
        float normalizePosition = 1f - (currentIdx / (itemCount - 1))
    }
}