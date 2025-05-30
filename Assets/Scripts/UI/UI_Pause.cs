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
        gameObject.SetActive(false);
    }

    public void Init()
    {
        currentCount = 3;
        Next();
    }

    public void Next()
    {
        if (currentCount > 0)
        {
            anim.SetTrigger(nextHash);
            countText.text = currentCount.ToString();
            currentCount--;
        }
        else
        {
            UI_Manager.Instance.mvPlayer.ReplayVideo();
            AudioManager.Instance.RestartBGM();            
        }                
    }

    public void StartPauseAnim()
    {
        gameObject.SetActive(true);
        Init();
    }

    public void SetActiveFalse() => gameObject.SetActive(false);
}
