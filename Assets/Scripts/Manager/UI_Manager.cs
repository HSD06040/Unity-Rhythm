using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Manager : Manager<UI_Manager>
{
    private InputActionAsset inputActions;
    private InputAction settingAction;
    private InputAction menuAction;

    private Canvas worldCanvas;
    private Canvas rectCanvas;

    public SettingPanel settingPanel;

    private MusicPanel _musicPanel;
    public MusicPanel musicPanel
    {
        get
        {
            if (_musicPanel != null)
                return _musicPanel;

            _musicPanel = FindObjectOfType<MusicPanel>(true);
            if (_musicPanel != null)
                return _musicPanel;

            _musicPanel = Resources.Load<MusicPanel>("UI/MusicPanel");

            return _musicPanel;
        }
    }

    public MVPlayer mvPlayer;
    public UI_FadeScreen fadeScreen;

    protected override void Awake()
    {
        base.Awake();

        worldCanvas = Instantiate(Resources.Load<Canvas>("UI/WorldCanvas"));
        DontDestroyOnLoad(worldCanvas);

        mvPlayer    = Instantiate(Resources.Load<MVPlayer>("UI/MVPlayer"), worldCanvas.transform);

        rectCanvas  = Instantiate(Resources.Load<Canvas>("UI/RectCanvas"));
        DontDestroyOnLoad(rectCanvas);

        settingPanel = Instantiate(Resources.Load<SettingPanel>("UI/SettingPanel"), rectCanvas.transform);
        fadeScreen  = Instantiate(Resources.Load<UI_FadeScreen>("UI/FadeScreen"), rectCanvas.transform);
    }

    private void Start()
    {
        inputActions = JudgeManager.Instance.inputActions;

        var uiMap = inputActions.FindActionMap("UI");

        settingAction = uiMap.FindAction("Setting");
        menuAction = uiMap.FindAction("Menu");

        settingAction.Enable();
        menuAction.Enable();

        settingAction.started += (ctx) =>
        {
            if (!settingPanel.gameObject.activeSelf)
                settingPanel.OpenPanel();
            else
                settingPanel.ClosePanel();
        };
    }    
}