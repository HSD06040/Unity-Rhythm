using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : Manager<SettingManager>, ISavable
{
    public Property<int> currentFPS;
    public int currentFpsSettingIdx;

    public Property<int> currentWidth;
    public Property<int> currentHeight;
    public int currentResolutionSettingIdx;

    protected override void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void Load(GameData data)
    {
        currentResolutionSettingIdx = data.resIdx;
        currentFpsSettingIdx = data.fpsIdx;
    }

    public void Save(ref GameData data)
    {
        data.resIdx = currentResolutionSettingIdx;
        data.fpsIdx = currentFpsSettingIdx;
    }
}
