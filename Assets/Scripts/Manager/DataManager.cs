using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class DataManager : Manager<DataManager>, ISavable
{
    public Property<int> level = new();
    public Property<int> exp = new();
    public string playerName;

    public Dictionary<BGM, MusicData> musicDataDic = new();
    private readonly Dictionary<int, int> levelDic = new()
{
    { 1, 1500 }, { 2, 1500 }, { 3, 2000 }, { 4, 2000 }, { 5, 2500 }, { 6, 2500 }, { 7, 3000 }, { 8, 3000 },
    { 9, 3500 }, { 10, 3000 }, { 11, 3500 }, { 12, 3500 }, { 13, 3500 }, { 14, 3500 },
    { 15, 3500 }, { 16, 3500 }, { 17, 3500 }, { 18, 3500 }, { 19, 4000 }, { 20, 3500 },
    { 21, 4000 }, { 22, 4000 }, { 23, 4000 }, { 24, 4000 }, { 25, 4000 }, { 26, 4000 },
    { 27, 4000 }, { 28, 4000 }, { 29, 5000 }, { 30, 4000 }, { 31, 4500 }, { 32, 4500 },
    { 33, 4500 }, { 34, 4500 }, { 35, 4500 }, { 36, 4500 }, { 37, 4500 }, { 38, 4500 },
    { 39, 5000 }, { 40, 4500 }, { 41, 5000 }, { 42, 5000 }, { 43, 5000 }, { 44, 5000 },
    { 45, 5000 }, { 46, 5000 }, { 47, 5000 }, { 48, 5000 }, { 49, 5500 }, { 50, 5000 },
    { 51, 5500 }, { 52, 5500 }, { 53, 5500 }, { 54, 5500 }, { 55, 5500 }, { 56, 5500 },
    { 57, 5500 }, { 58, 5500 }, { 59, 6000 }, { 60, 5500 }, { 61, 6000 }, { 62, 6000 },
    { 63, 6000 }, { 64, 6000 }, { 65, 6000 }, { 66, 6000 }, { 67, 6000 }, { 68, 6000 },
    { 69, 7000 }, { 70, 6000 }, { 71, 7000 }, { 72, 7000 }, { 73, 7000 }, { 74, 7000 },
    { 75, 7000 }, { 76, 7000 }, { 77, 7000 }, { 78, 7000 }, { 79, 8000 }, { 80, 7000 },
    { 81, 8000 }, { 82, 8000 }, { 83, 8000 }, { 84, 8000 }, { 85, 8000 }, { 86, 8000 },
    { 87, 8000 }, { 88, 8000 }, { 89, 9000 }, { 90, 8500 }, { 91, 9000 }, { 92, 9000 },
    { 93, 9000 }, { 94, 9000 }, { 95, 9500 }, { 96, 10000 }, { 97, 11000 }, { 98, 11500 },
    { 99, 12500 }, { 100, 30000 }
};


    private SaveManager saveManager;
    public bool isLevelUp;

    protected override void Awake()
    {
        base.Awake();

        LoadAllMusicData();

        saveManager = new GameObject("SaveManager").AddComponent<SaveManager>();
        saveManager.transform.SetParent(transform, false);
    }

    public float GetLevelExp() => levelDic[level.Value];
    public float GetLevelExp(int _level) => levelDic[_level];

    public float GetLevelProportion()
    {
        return exp.Value / GetLevelExp();
    }

    public void Load(GameData data)
    {      
        exp.Value = data.exp;
        level.Value = data.level;
        playerName = data.playerName;
    }

    public void Save(ref GameData data)
    {
        data.exp = exp.Value;
        data.level = level.Value;
        data.playerName = playerName;
    }

    [ContextMenu("=== Delete Save Data ===")]
    public void DeleteFile()
    {
        saveManager = new();
        saveManager.DeleteFile();
    }

    private void LoadAllMusicData()
    {
        MusicData[] musicDatas = Resources.LoadAll<MusicData>("MusicDatas");

        foreach (var data in musicDatas)
        {
            if(!musicDataDic.ContainsKey(data.bgm))
            {
                musicDataDic.Add(data.bgm, data);
            }
            else
            {
                Debug.Log("이미 추가된 노래 데이터 입니다.");
            }
        }
    }

    public void AddExp(int amount)
    {
        Debug.Log(amount);
        exp.Value += amount;

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if(exp.Value >= GetLevelExp())
        {
            exp.Value -= (int)GetLevelExp();
            LevelUp();            
        }
    }

    private void LevelUp()
    {
        level.Value++;
        isLevelUp = true;
    }
}
