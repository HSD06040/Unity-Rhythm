using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Manager<DataManager>, ISavable
{
    public Property<int> Level = new();
    public Property<int> Exp = new();
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
        Exp.Value = data.exp;
        Level.Value = data.level;
        playerName = data.playerName;
    }

    public void Save(ref GameData data)
    {
        data.exp = Exp.Value;
        data.level = Level.Value;
        data.playerName = playerName;
    }
}
