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
    [SerializeField] private Transform starParent;

    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject emptyStarPrefab;
    private Animator anim;
    private int max = 15;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();   
    }    

    public void SetMusicBar(MusicData data)
    {
        musicName.text = data.musicName;
        artistName.text = data.artistName;
        musicIcon.sprite = data.icon;

        for (int i = 0; i < data.difficulty; i++)
        {
            Instantiate(starPrefab, starParent);
        }

        for (int i = 0; i < max - data.difficulty; i++)
        {
            Instantiate(emptyStarPrefab, starParent);
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            anim.SetTrigger("In");
        }
        else
        {
            anim.SetTrigger("Out");
        }
    }
}
