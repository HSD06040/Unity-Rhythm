using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject inputName;
    [SerializeField] private GameObject mask;
    [SerializeField] private float speed;
    [SerializeField] private TMP_InputField nameField;
    private Coroutine lobbyRoutine;    

    private void Start()
    {
        AudioManager.Instance.PlayBGM(BGM.Title, 1);
    }

    private void Update()
    {
        if (UI_Manager.Instance.isMenu) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (GameManager.Instance.isFirstPlaying)
            {
                if (!inputName.activeSelf)
                    InputName();
                else
                    SelectName();
            }
            else
            {
                if (lobbyRoutine == null)
                    lobbyRoutine = StartCoroutine(LoadLobby());
            }
        }        
    }

    private void InputName()
    {
        inputName.SetActive(true);
        StartCoroutine(MaskWidthExpandRoutine());
    }

    private IEnumerator MaskWidthExpandRoutine()
    {
        float elapsed = 0f;
        float duration = 1f / speed;

        RectTransform rect = mask.GetComponent<RectTransform>();
        Vector2 startSize = new Vector2(0f, rect.sizeDelta.y);
        Vector2 endSize = new Vector2(1920f, rect.sizeDelta.y);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            rect.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            yield return null;
        }

        rect.sizeDelta = endSize;
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

        yield return new WaitForSeconds(1f);

        asyncLoad.allowSceneActivation = true;
        UI_Manager.Instance.fadeScreen.ExitFade(FadeType.Defualt);

        yield return new WaitForSeconds(1f);

        lobbyRoutine = null;
    }

    private void SelectName()
    {
        if (string.IsNullOrEmpty(nameField.text)) return;

        DataManager.Instance.playerName = nameField.text;
        GameManager.Instance.isFirstPlaying = false;

        if (lobbyRoutine == null)
            lobbyRoutine = StartCoroutine(LoadLobby());
    }
}
