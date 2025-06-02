using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GameMenu : BaseUI
{
    private Animator[] imageAnimators = new Animator[5];
    private Animator[] textAnimators = new Animator[5];
    private Button[] buttons = new Button[5];

    private static readonly int inHash = Animator.StringToHash("In");
    private static readonly int outHash = Animator.StringToHash("Out");

    private int currentIdx;
    private Coroutine lobbyRoutine;
    private Coroutine restartRoutine;

    protected override void Awake()
    {
        base.Awake();

        imageAnimators[0] = GetUI<Animator>("ContinueImage");        
        imageAnimators[1] = GetUI<Animator>("RestartImage");
        imageAnimators[2] = GetUI<Animator>("MusicSelectImage");
        imageAnimators[3] = GetUI<Animator>("SettingImage");
        imageAnimators[4] = GetUI<Animator>("ExitImage");

        textAnimators[0] = GetUI<Animator>("ContinueText");
        textAnimators[1] = GetUI<Animator>("RestartText");
        textAnimators[2] = GetUI<Animator>("MusicSelectText");
        textAnimators[3] = GetUI<Animator>("SettingText");
        textAnimators[4] = GetUI<Animator>("ExitText");

        buttons[0] = GetUI<Button>("ContinueButton");
        buttons[1] = GetUI<Button>("RestartButton");
        buttons[2] = GetUI<Button>("MusicSelectButton");
        buttons[3] = GetUI<Button>("SettingButton");
        buttons[4] = GetUI<Button>("ExitButton");
    }

    private void OnEnable()
    {
        currentIdx = 0;

        if(GameManager.Instance.onMusicPlaying)
        {
            buttons[1].gameObject.SetActive(true);
            buttons[2].gameObject.SetActive(true);
            buttons[3].gameObject.SetActive(false);
            buttons[4].gameObject.SetActive(false);

            buttons[0].onClick.AddListener(() => Continue());
            buttons[1].onClick.AddListener(() => Restart());
            buttons[2].onClick.AddListener(() => MusicSelect());
        }
        else
        {
            buttons[1].gameObject.SetActive(false);
            buttons[2].gameObject.SetActive(false);
            buttons[3].gameObject.SetActive(true);
            buttons[4].gameObject.SetActive(true);

            buttons[0].onClick.AddListener(() => Continue());
            buttons[3].onClick.AddListener(() => Setting());
            buttons[4].onClick.AddListener(() => Exit());
        }

        UpdateCurrent();
    }

    private void OnDisable()
    {
        if (GameManager.Instance.onMusicPlaying)
        {
            buttons[0].onClick.RemoveListener(() => Continue());
            buttons[1].onClick.RemoveListener(() => Restart());
            buttons[2].onClick.RemoveListener(() => MusicSelect());
        }
        else
        {
            buttons[0].onClick.RemoveListener(() => Continue());
            buttons[3].onClick.RemoveListener(() => Setting());
            buttons[4].onClick.RemoveListener(() => Exit());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            SelectButton(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            SelectButton(1);

        if(Input.GetKeyDown(KeyCode.Return))
            buttons[currentIdx].onClick.Invoke();
    }

    private void SelectButton(int amount)
    {
        if (currentIdx + amount < 0 || currentIdx + amount > buttons.Length - 1) return;

        if (GameManager.Instance.onMusicPlaying && (currentIdx + amount == 3 || currentIdx + amount == 4))                    
            return;        

        imageAnimators[currentIdx].SetTrigger(outHash);
        textAnimators[currentIdx].SetTrigger(outHash);

        currentIdx += amount;

        if(!GameManager.Instance.onMusicPlaying && (currentIdx == 1 || currentIdx == 2))
        {
            if (amount > 0)
                currentIdx = 3;
            else
                currentIdx = 0;
        }
        

        UpdateCurrent();
    }

    private void UpdateCurrent()
    {
        imageAnimators[currentIdx].SetTrigger(inHash);
        textAnimators[currentIdx].SetTrigger(inHash);
    }

    public void Setting()
    {
        UI_Manager.Instance.settingPanel.OpenPanel();
    }

    public void Continue()
    {
        UI_Manager.Instance.isPause = true;
        ClosePanel();
    }

    public void Restart()
    {
        if (restartRoutine == null)
            restartRoutine = StartCoroutine(RestartRoutine());
    }

    public void MusicSelect()
    {
        if (lobbyRoutine == null)
            lobbyRoutine = StartCoroutine(LoadLobby());
    }

    public void Exit()
    {
        Application.Quit();
    }

    private IEnumerator LoadLobby()
    {
        UI_Manager.Instance.fadeScreen.EnterFade(FadeType.Defualt);

        yield return new WaitForSeconds(1);
        UI_Manager.Instance.mvPlayer.StopVideo();
        AudioManager.Instance.StopBGM();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby");
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
            yield return null;

        asyncLoad.allowSceneActivation = true;
        UI_Manager.Instance.fadeScreen.ExitFade(FadeType.Defualt);

        yield return new WaitForSeconds(1f);

        lobbyRoutine = null;
    }

    private IEnumerator RestartRoutine()
    {
        UI_Manager.Instance.fadeScreen.EnterFade(FadeType.Defualt);

        yield return new WaitForSeconds(.5f);

        GameManager.Instance.onMusicPlaying = false;

        UI_Manager.Instance.mvPlayer.StopVideo();
        AudioManager.Instance.StopBGM();

        yield return new WaitForSeconds(2f);

        GameManager.Instance.SetGameStart();

        UI_Manager.Instance.fadeScreen.ExitFade(FadeType.Defualt);

        ClosePanel();
    }

    public void OpenPanel()
    {
        if (GameManager.Instance.onMusicPlaying)
        {
            UI_Manager.Instance.mvPlayer.PauseVideo();
            AudioManager.Instance.StopBGM();
        }

        textAnimators[currentIdx].SetTrigger(outHash);
        imageAnimators[currentIdx].SetTrigger(outHash);

        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        if (GameManager.Instance.onMusicPlaying && restartRoutine == null)
            UI_Manager.Instance.pause.StartPauseAnim();

        restartRoutine ??= null;

        gameObject.SetActive(false);
        UI_Manager.Instance.isMenu = false;
    }
}
