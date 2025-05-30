using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private GameObject soundSetting;
    [SerializeField] private GameObject keySetting;

    [SerializeField] private Button soundSettingButton;
    [SerializeField] private Button keySettingButton;
    
    private Animator anim;
    private static readonly int inHash = Animator.StringToHash("In");
    private static readonly int outHash = Animator.StringToHash("Out");

    private bool canChanged = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        soundSettingButton.onClick.AddListener(() => SwitchSetting(soundSetting));
        keySettingButton.onClick.AddListener(() => SwitchSetting(keySetting));
    }

    private void OnDisable()
    {
        soundSettingButton.onClick.RemoveListener(() => SwitchSetting(soundSetting));
        keySettingButton.onClick.RemoveListener(() => SwitchSetting(keySetting));
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
    public void SwitchSetting(GameObject ui)
    {
        keySetting.SetActive(ui == keySetting);
        soundSetting.SetActive(ui == soundSetting);
    }
}
