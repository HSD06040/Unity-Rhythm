using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;

public class NoteMaker : MonoBehaviour
{
    public BGM bgm;
    public float currentTime => timer - songDelay;
    public float songDelay;
    public float timer;

    public bool isMusicPlay;

    public TMP_Text currentSong;
    public TMP_Text currentTimeText;
    public TMP_InputField keySound;
    public TMP_InputField keySoundVolume;
    public TMP_InputField endTime;

    public MapData mapData;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangeMusicStartStop();
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RewindMusic(1);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            RewindMusic(-1);
        }

        if(Input.GetKeyDown(KeyCode.D))
            AddShortNote(0);

        if (Input.GetKeyDown(KeyCode.F))
            AddShortNote(1);

        if (Input.GetKeyDown(KeyCode.H))
            AddShortNote(2);

        if (Input.GetKeyDown(KeyCode.J))
            AddShortNote(3);

        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.Instance.PlayBGM(bgm, 1);
            isMusicPlay = true;
            timer = 0;
            songDelay = 0;
        }

        currentTimeText.text = currentTime.ToString();

        if (!isMusicPlay) return;

        timer += Time.deltaTime;
    }

    public void ChangeMusicStartStop()
    {
        if (isMusicPlay)
        {
            StopMusic();
        }
        else
        {
            RestartMusic();
        }
    }

    public void StopMusic()
    {
        isMusicPlay = false;
        AudioManager.Instance.StopBGM();
    }

    public void RestartMusic()
    {
        isMusicPlay = true;
        AudioManager.Instance.RestartBGM();
    }

    public void RewindMusic(float seconds)
    {
        songDelay += seconds;
        AudioManager.Instance.RewindBGM(seconds);
    }

    public void SaveMusic()
    {
        MapParser.SaveMap(mapData, bgm);
    }

    public void LoadMusic()
    {
        mapData.notes = MapParser.LoadMap(BGM.NOPAIN);
    }

    public void AddShortNote(int keyPos)
    {
        NoteData data = new NoteData
        {
            startTime = currentTime * 1000,
            keyPos = keyPos,
            noteType = 0,
            keySound = keySound.text,
            keySoundVolume = float.TryParse(keySoundVolume.text, out float volume) ? volume : 1f
        };

        mapData.notes.Add(data);
    }
    public void AddLongNote(int keyPos)
    {
        NoteData data = new NoteData
        {
            startTime = currentTime * 1000,
            keyPos = keyPos,
            noteType = 1,
            endTime = float.TryParse(endTime.text, out float _endTime) ? _endTime * 1000 : 0,
            keySound = keySound.text,
            keySoundVolume = float.TryParse(keySoundVolume.text, out float volume) ? volume : 1f,            
        };

        mapData.notes.Add(data);
    }

    private void OnValidate()
    {
        currentSong.text = bgm.ToString();
    }
}
