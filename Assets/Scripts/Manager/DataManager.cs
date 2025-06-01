using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : Manager<DataManager>, ISavable
{
    public Property<int> level = new();
    public Property<int> exp = new();
    public string playerName;

    public Dictionary<BGM, MusicData> musicDataDic = new();
    private readonly Dictionary<int, int> levelDic = new Dictionary<int, int>
    {
        { 0, 100}, { 1, 100}, { 2, 100}, { 3, 100}, { 4, 100}, { 5, 100}, { 6, 100}, { 7, 100}, { 8, 100},
    };

    private SaveManager saveManager;

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
        return 60 / GetLevelExp();
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
                Debug.Log("추가");
                musicDataDic.Add(data.bgm, data);
            }
            else
            {
                Debug.Log("이미 추가된 노래 데이터 입니다.");
            }
        }
    }
}
