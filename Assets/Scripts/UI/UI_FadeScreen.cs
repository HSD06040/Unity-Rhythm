using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeScreen : MonoBehaviour
{
    private Image fadeImage;
    private Animator anim;
    private RectTransform rectTransform;

    #region AnimHash

    private static readonly int fadeInHash = Animator.StringToHash("FadeIn");
    private static readonly int fadeOutHash = Animator.StringToHash("FadeOut");

    private static readonly int horizontalFadeInHash = Animator.StringToHash("HorizontalFadeIn");
    private static readonly int horizontalFadeOutHash = Animator.StringToHash("HorizontalFadeOut");

    private static readonly int verticalFadeInHash = Animator.StringToHash("VerticalFadeIn");
    private static readonly int verticalFadeOutHash = Animator.StringToHash("VerticalFadeOut");

    #endregion

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
    }

    public void ChangeFade(FadeType fadeType)
    {
        if (!fadeImage.gameObject.activeSelf)
            EnterFade(fadeType);
        else
            ExitFade(fadeType);
    }

    public void EnterFade(FadeType fadeType)
    {
        fadeImage.gameObject.SetActive(true);

        switch (fadeType)
        {
            case FadeType.Defualt:
                anim.SetTrigger(fadeInHash);
                break;
            case FadeType.Vertical:
                FadeInVertical();
                anim.SetTrigger(verticalFadeInHash);
                break;
            case FadeType.Horizontal:
                FadeInHorizontal();
                anim.SetTrigger(horizontalFadeInHash);
                break;
        }
    }

    public void ExitFade(FadeType fadeType)
    {
        switch (fadeType)
        {
            case FadeType.Defualt:
                anim.SetTrigger(fadeOutHash);
                break;
            case FadeType.Vertical:
                anim.SetTrigger(verticalFadeOutHash);
                break;
            case FadeType.Horizontal:
                anim.SetTrigger(horizontalFadeOutHash);
                break;
        }
    }

    public void FadeInVertical()
    {
        rectTransform.anchoredPosition = new Vector2(0, 540f);
        rectTransform.sizeDelta = new Vector2(1920, 5f);
    }

    public void FadeInHorizontal()
    {
        rectTransform.anchoredPosition = new Vector2(-960, 0);
        rectTransform.sizeDelta = new Vector2(0, 1080f);
    }
}
