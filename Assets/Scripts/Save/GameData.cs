using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Data
{

}

[Serializable]
public class GameData : Data
{
    public string keyBindingJson;

    public int level;
    public int exp;
    public string playerName;

    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    public GameData()
    {
        level = 1;
        exp = 0;
        playerName = string.Empty;
        masterVolume = 1;
        bgmVolume = 1;
        sfxVolume = 1;
    }
}

[Serializable]
public class PlayData : Data
{
    public BGM bgm;
    public Rank rank;
    public float rate;  
    public float resSpeed;
    public int score;
    public int combo;
}
