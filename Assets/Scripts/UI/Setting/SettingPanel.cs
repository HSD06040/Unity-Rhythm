using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{ 
    private Animator anim;
    private static readonly int inHash = Animator.StringToHash("In");
    private static readonly int outHash = Animator.StringToHash("Out");

    private bool canChanged = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenPanel()
    {
        if (!canChanged) return;

        if(GameManager.Instance.onMusicPlaying)
        {
            UI_Manager.Instance.mvPlayer.PauseVideo();
            AudioManager.Instance.StopBGM();
        }

        gameObject.SetActive(true);
        anim.ResetTrigger(outHash);
        anim.SetTrigger(inHash);
    }

    public void ClosePanel()
    {
        if (!canChanged) return;

        if(GameManager.Instance.onMusicPlaying)
            UI_Manager.Instance.pause.StartPauseAnim();

        anim.ResetTrigger(inHash);
        anim.SetTrigger(outHash);        
    }

    public void SetActivateFalse() => gameObject.SetActive(false);

    public void CanChangedTrue() => canChanged = true;
    public void CanChangedFalse() => canChanged = false;
}
