using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class GameData
{
    public string keyBindingJson;
    public bool isFirstPlaying;

    public int level;
    public int exp;
    public string playerName;

    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    [Header("Setting Data")]
    public int fpsIdx;
    public int resIdx;

    public GameData()
    {
        level = 1;
        exp = 0;
        playerName = string.Empty;
        masterVolume = 1;
        bgmVolume = 1;
        sfxVolume = 1;
        isFirstPlaying = true;
    }
}

[Serializable]
public class PlayData
{
    public BGM bgm;
    public Rank rank;
    public float rate;  
    public float resSpeed;
    public int score;
    public int maxCombo;

    public int perfect;
    public int greate;
    public int good;
    public int miss;
}
