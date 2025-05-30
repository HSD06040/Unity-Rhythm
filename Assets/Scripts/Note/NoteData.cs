using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicData", menuName = "MusicData")]
public class MusicData : ScriptableObject
{
    public BGM bgm;
    public Sprite icon;
    public string artistName;
    public string videoURL;
    public int BPM;
}

[Serializable]
public class MapData
{
    public BGM bgm;
    public List<NoteData> notes;
}

[Serializable]
public class NoteData
{
    public void TransmitData(NoteData _to)
    {
        _to.keyPos = this.keyPos;
        _to.startTime = this.startTime;
        _to.endTime = this.endTime;
        _to.noteType = this.noteType;
        _to.keySound = this.keySound;
        _to.keySoundVolume = this.keySoundVolume;
        _to.longNoteData = this.longNoteData;

        _to.isLongNoteBody = this.isLongNoteBody;
        _to.isLongNoteEnd = this.isLongNoteEnd;
        _to.isLongNoteStart = this.isLongNoteStart;
    }

    public int keyPos;
    public int noteType;
    public NoteData longNoteData;
    public float startTime;
    public float endTime;
    public string keySound;
    public float keySoundVolume;
    public bool isJudgeDone;

    public bool isLongNoteStart;
    public bool isLongNoteBody;
    public bool isLongNoteEnd;
}
