using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicBar : MonoBehaviour
{
    [SerializeField] private TMP_Text musicName;
    [SerializeField] private TMP_Text artistName;
    [SerializeField] private Image barImage;
    [SerializeField] private Image musicIcon;    

    public void SetMusicBar(MusicData data)
    {
        musicName.text = data.bgm.ToString();
        artistName.text = data.artistName;
        musicIcon.sprite = data.icon;
    }

    public void SetSelected(bool isSelected)
    {
        barImage.color = isSelected ? Color.white : Color.clear;
    }
}
