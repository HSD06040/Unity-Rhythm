using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicBar : MonoBehaviour
{
    [SerializeField] private TMP_Text musicName;
    [SerializeField] private Image musicIcon;
    [SerializeField] private Image background;
    [SerializeField] private Color selectColor;
    public void SetMusicBar(Sprite icon, string _name)
    {
        musicName.text = _name;
        musicIcon.sprite = icon;
    }

    public void SetSelected(bool isSelected)
    {
        background.color = isSelected ? selectColor : Color.clear;
    }
}
