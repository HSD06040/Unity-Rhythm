using System.Collections;
using TMPro;
using UnityEngine;

public class UI_Pause : MonoBehaviour
{
    private int currentCount;
    private Animator anim;
    [SerializeField] private TMP_Text countText;

    private static readonly int nextHash = Animator.StringToHash("Next");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        Init();
    }

    public void Init()
    {
        currentCount = 4;
        Next();
    }

    public void Next()
    {
        currentCount--;
        anim.SetTrigger(nextHash);

        if (currentCount > 0)
        {
            countText.text = currentCount.ToString();
        }
        else
        {
            countText.text = "GO!";
            UI_Manager.Instance.mvPlayer.ReplayVideo();
            AudioManager.Instance.RestartBGM();
        }
    }

    public void StartPauseAnim()
    {
        gameObject.SetActive(true);
        anim.SetTrigger(nextHash);
    }

    public void SetActiveFalse() => gameObject.SetActive(false);
}
