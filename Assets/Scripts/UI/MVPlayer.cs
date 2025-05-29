using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MVPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();    
    }

    private void SetURL(MusicData musicData)
    {
        videoPlayer.url = musicData.videoURL;
    }
}
