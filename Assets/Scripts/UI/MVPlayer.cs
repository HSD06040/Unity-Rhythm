using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MVPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private RawImage rawImage;
    [SerializeField] private Texture video;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();    
        rawImage = GetComponent<RawImage>();
    }

    public void PlayMusicVideo(string url)
    {
        if (rawImage.texture == null)
        {
            rawImage.color = Color.white;
            rawImage.texture = video;
        }

        if(videoPlayer.isPlaying)
            videoPlayer.Stop();

        videoPlayer.url = url;
        videoPlayer.Play();
    }

    public void PlayMusicVideo(string url, float delay)
    {
        StartCoroutine(DelayMusicVideoPlay(url, delay));
    }

    public void PauseVideo() => videoPlayer.Pause();
    public void ReplayVideo() => videoPlayer.Play();
    public void StopVideo()
    {
        videoPlayer.Stop();
        rawImage.color = Color.black;
        rawImage.texture = null;
    }

    IEnumerator DelayMusicVideoPlay(string url, float delay)
    {
        if (rawImage.texture == null)
        {
            rawImage.color = Color.white;
            rawImage.texture = video;
        }

        if (videoPlayer.isPlaying)
            videoPlayer.Stop();

        videoPlayer.url = url;
        yield return new WaitForSeconds(delay);
        videoPlayer.Play();
    }
}
