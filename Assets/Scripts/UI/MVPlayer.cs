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

    public void PlayMusicVideo(string url)
    {
        if(videoPlayer.isPlaying)
            videoPlayer.Stop();

        videoPlayer.url = url;
        videoPlayer.Play();
    }

    public void PlayMusicVideo(string url, float delay)
    {
        StartCoroutine(DelayMusicVideoPlay(url, delay));
    }

    public void StopVideo() => videoPlayer.Pause();
    public void ReplayVideo() => videoPlayer.Play();

    IEnumerator DelayMusicVideoPlay(string url, float delay)
    {
        videoPlayer.Stop();
        videoPlayer.url = url;
        yield return new WaitForSeconds(delay);
        videoPlayer.Play();
    }
}
