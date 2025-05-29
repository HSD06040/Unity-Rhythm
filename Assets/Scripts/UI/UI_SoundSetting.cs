using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoundSetting : BaseUI
{
    private Slider masterVolumeSlider;
    private Slider bgmVolumeSlider;
    private Slider sfxVolumeSlider;

    private TMP_Text masterVolumeText;
    private TMP_Text bgmVolumeText;
    private TMP_Text sfxVolumeText;

    protected override void Awake()
    {
        base.Awake();

        masterVolumeSlider = GetUI<Slider>("Master");
        bgmVolumeSlider = GetUI<Slider>("BGM");
        sfxVolumeSlider = GetUI<Slider>("SFX");

        masterVolumeText = GetUI<TextMeshProUGUI>("MasterText");
        bgmVolumeText = GetUI<TextMeshProUGUI>("BGMText");
        sfxVolumeText = GetUI<TextMeshProUGUI>("SFXText");
    }

    private void OnEnable()
    {
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        SetMasterVolume(masterVolumeSlider.value);
        SetBGMVolume(bgmVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);
    }

    private void OnDisable()
    {
        masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
        bgmVolumeSlider.onValueChanged.RemoveListener(SetBGMVolume);
        sfxVolumeSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }

    private void SetMasterVolume(float value)
    {
        AudioManager.Instance.masterVolume.Value = value;
        masterVolumeText.text = ((int)(value * 100)).ToString();
    }

    private void SetBGMVolume(float value)
    {
        AudioManager.Instance.bgmVolume.Value = value;
        bgmVolumeText.text = ((int)(value * 100)).ToString();
    }

    private void SetSFXVolume(float value)
    {
        AudioManager.Instance.sfxVolume.Value = value;
        sfxVolumeText.text = ((int)(value * 100)).ToString();
    }
}
