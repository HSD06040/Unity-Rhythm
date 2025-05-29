using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Manager<DataManager>, ISavable
{
    public Property<int> level = new();
    public Property<int> exp = new();
    public string playerName;

    private SaveManager saveManager;

    protected override void Awake()
    {
        base.Awake();

        saveManager = new GameObject("SaveManager").AddComponent<SaveManager>();
        saveManager.transform.SetParent(transform, false);
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
        saveManager.DeleteFile();
    }
}
