using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MVPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private RawImage rawImage;
    [SerializeField] private GameObject[] lines;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();    
        rawImage = GetComponent<RawImage>();
    }

    public void PlayMusicVideo(string url, bool isLine = true)
    {
        rawImage.color = Color.black;

        foreach (var line in lines)
            line.SetActive(isLine);

        if (videoPlayer.isPlaying)
            videoPlayer.Stop();

        videoPlayer.targetTexture.Release();

        videoPlayer.url = url;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        videoPlayer.Play();
        rawImage.color = Color.white;

        videoPlayer.prepareCompleted -= OnVideoPrepared;
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
    }

    IEnumerator DelayMusicVideoPlay(string url, float delay)
    {        

        if (videoPlayer.isPlaying)
            videoPlayer.Stop();

        videoPlayer.url = url;
        yield return new WaitForSeconds(delay);
        videoPlayer.Play();
        rawImage.color = Color.white;         
    }
}
