using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MVPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private Image image;
    [SerializeField] private GameObject[] lines;

    private readonly YieldInstruction delay = new WaitForSeconds(.5f);

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();    
    }

    public void PlayMusicVideo(string url, bool isLine = true)
    {
        image.gameObject.SetActive(true);

        foreach (var line in lines)
            line.SetActive(isLine);

        if (videoPlayer.isPlaying)
            videoPlayer.Stop();

        videoPlayer.targetTexture.Release();

        videoPlayer.url = url;

        StartCoroutine(MVPlayRoutine());
    }

    private IEnumerator MVPlayRoutine()
    {
        videoPlayer.Play();
        
        yield return delay;

        image.gameObject.SetActive(false);
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
    }

    IEnumerator DelayMusicVideoPlay(string url, float delay)
    {        
        if (videoPlayer.isPlaying)
            videoPlayer.Stop();

        videoPlayer.url = url;
        yield return new WaitForSeconds(delay);
        videoPlayer.Play();     
    }
}
