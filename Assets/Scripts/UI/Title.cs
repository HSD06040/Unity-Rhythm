using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject inputName;
    private Coroutine lobbyRoutine;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.Title, 1);
    }

    private void Update()
    {
        if (GameManager.Instance.isFirstPlaying)
            InputName();
        else
        {
            if (lobbyRoutine == null)
                lobbyRoutine = StartCoroutine(LoadLobby());
        }
    }

    private void InputName()
    {
        inputName.SetActive(true);
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
}
