using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameSetting : BaseUI
{
    private int[] widths;
    private int[] heights;
    private string[] resolutions;
    private RefreshRate[] refreshRates;

    private TMP_Text resolutionText;
    private TMP_Text fpsText;

    private Button resLeftButton;
    private Button resRigthButton;
    private Button fpsLeftButton;
    private Button fpsRigthButton;

    private int currentResolutionIdx;
    private int currentFpsIdx;

    protected override void Awake()
    {
        base.Awake();

        currentResolutionIdx = SettingManager.Instance.currentResolutionSettingIdx;
        currentFpsIdx = SettingManager.Instance.currentFpsSettingIdx;

        SetupResolution();

        resolutionText = GetUI<TextMeshProUGUI>("ResText");
        fpsText = GetUI<TextMeshProUGUI>("FpsText");

        resLeftButton = GetUI<Button>("ResLeftButton");
        resRigthButton = GetUI<Button>("ResRigthButton");

        fpsLeftButton = GetUI<Button>("FpsLeftButton");
        fpsRigthButton = GetUI<Button>("FpsLeftButton");

        UpdateFps();
        //UpdateResolution();
    }

    public void ResolutionChange(int amount)
    {
        if (currentResolutionIdx + amount < 0 || currentResolutionIdx + amount > resolutions.Length - 1) return;

        currentResolutionIdx += amount;
        SettingManager.Instance.currentFpsSettingIdx = currentResolutionIdx;

        //UpdateResolution();
    }

    private void UpdateResolution()
    {
        Screen.SetResolution(widths[currentResolutionIdx], heights[currentResolutionIdx], FullScreenMode.Windowed);
        resolutionText.text = resolutions[currentResolutionIdx];
    }

    public void FpsChange(int amount)
    {
        if (currentFpsIdx + amount < 0 || currentFpsIdx + amount > refreshRates.Length - 1) return;

        currentFpsIdx += amount;
        SettingManager.Instance.currentFpsSettingIdx = currentFpsIdx;
 
        UpdateFps();
    }

    private void UpdateFps()
    {
        Application.targetFrameRate = (int)refreshRates[currentFpsIdx].value;
        fpsText.text = $"{refreshRates[currentFpsIdx].value.ToString("F0")} fps";
    }

    private void SetupResolution()
    {
        Resolution[] _resolutions = Screen.resolutions;

        refreshRates = new RefreshRate[_resolutions.Length];
        widths = new int[_resolutions.Length];
        heights = new int[_resolutions.Length];
        resolutions = new string[_resolutions.Length];

        for (int i = 0; i < _resolutions.Length; i++)
        {
            widths[i] = _resolutions[i].width;
            heights[i] = _resolutions[i].height;
            resolutions[i] = $"{widths[i]} x {heights[i]}";

            Debug.Log(resolutions[i]);
            Debug.Log(refreshRates[i].value);

            refreshRates[i] = _resolutions[i].refreshRateRatio;
        }
    }
}
