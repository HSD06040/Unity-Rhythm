using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[System.Serializable]
public class BGMData
{
    public string name;
    public EventReference eventRef;
}

public class AudioManager : Manager<AudioManager>, ISavable
{
    public FMOD.Channel musicChannel;

    [Header("BGM")]
    public FMOD.ChannelGroup bgmChannelGroup;
    public FMOD.Sound[] bgms;
    public FMOD.Channel[] bgmChannels;

    [Header("SFX")]
    public FMOD.ChannelGroup sfxChannelGroup;
    public FMOD.Sound[] sfxs;
    public FMOD.Channel[] sfxChannels;

    [Header("Volume")]
    public float masterVolume = 1;
    public float sfxVolume = 1;
    public float bgmVolume = 1;

    protected override void Awake()
    {
        base.Awake();

        LoadBGM();
        LoadSFX();
    }

    private void LoadSFX()
    {
        int count = Enum.GetValues(typeof(SFX)).Length;

        sfxChannelGroup = new FMOD.ChannelGroup();
        sfxChannels = new FMOD.Channel[count];
        sfxs = new FMOD.Sound[count];

        for (int i = 0; i < count; i++)
        {
            string sfxFileName = System.Enum.GetName(typeof(SFX), i);
            string audioType = "WAV";

            FMODUnity.RuntimeManager.CoreSystem.createSound
                (Path.Combine(Application.streamingAssetsPath, "SFXS", sfxFileName + "." + audioType),FMOD.MODE.CREATESAMPLE, out sfxs[i]);
        }

        for (int i = 0; i < count; i++)
        {
            sfxChannels[i].setChannelGroup(sfxChannelGroup);
        }
    }

    private void LoadBGM()
    {
        int count = Enum.GetValues(typeof(BGM)).Length;

        bgmChannelGroup = new FMOD.ChannelGroup();
        bgmChannels = new FMOD.Channel[count];
        bgms = new FMOD.Sound[count];

        for (int i = 0; i < count; i++)
        {
            string bgmFileName = Enum.GetName(typeof(BGM), i);
            string audioType = "mp3";

            FMODUnity.RuntimeManager.CoreSystem.createSound
                (Path.Combine(Application.streamingAssetsPath, "BGMS", bgmFileName + "." + audioType), FMOD.MODE.CREATESAMPLE, out bgms[i]);
        }

        for (int i = 0; i < count; i++)
        {
            bgmChannels[i].setChannelGroup(bgmChannelGroup);
        }
    }

    public void PlaySFX(SFX _sfx, float _volume = 1)
    {
        sfxChannels[(int)_sfx].stop();

        FMODUnity.RuntimeManager.CoreSystem.playSound(sfxs[(int)_sfx], sfxChannelGroup, false, out sfxChannels[(int)_sfx]);

        sfxChannels[(int)_sfx].setPaused(true);
        sfxChannels[(int)_sfx].setVolume((_volume * sfxVolume) * masterVolume);
        sfxChannels[(int)_sfx].setPaused(false);
    }

    public void PlayBGM(BGM _bgm, float _volume = 1)
    {
        musicChannel.stop();
        
        FMODUnity.RuntimeManager.CoreSystem.playSound(bgms[(int)_bgm], bgmChannelGroup, false, out bgmChannels[(int)_bgm]);

        bgmChannels[(int)_bgm].setPaused(true);
        bgmChannels[(int)_bgm].setVolume((_volume * bgmVolume) * masterVolume);
        bgmChannels[(int)_bgm].setPaused(false);
        musicChannel = bgmChannels[(int)_bgm];
    }

    public void StopBGM()
    {
        musicChannel.setPaused(true);
    }

    public void RestartBGM()
    {
        musicChannel.setPaused(false);
    }

    public void RewindBGM(float seconds)
    {
        if(musicChannel.hasHandle())
        {
            uint currentPosition;
            musicChannel.getPosition(out currentPosition, FMOD.TIMEUNIT.MS);

            uint resindPosition = (uint)Mathf.Max(0, currentPosition - (uint)(seconds * 1000));

            musicChannel.setPosition(resindPosition, FMOD.TIMEUNIT.MS);
        }
    }

    public void UpdateVolume() => musicChannel.setVolume(bgmVolume * masterVolume);


    public void Save(ref GameData data)
    {
        data.masterVolume = masterVolume;
        data.sfxVolume = sfxVolume;
        data.bgmVolume = bgmVolume;
    }

    public void Load(GameData data)
    {
        masterVolume = data.masterVolume;
        sfxVolume = data.sfxVolume;
        bgmVolume = data.bgmVolume;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            StopBGM();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus && !GameManager.Instance.onMusicPlaying)
           RestartBGM();
    }
}
