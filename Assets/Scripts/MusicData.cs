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
