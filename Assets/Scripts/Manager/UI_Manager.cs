using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Manager : Manager<UI_Manager>
{
    private InputActionAsset inputActions;
    private InputAction menuAction;

    private Canvas worldCanvas;
    private Canvas rectCanvas;

    public SettingPanel settingPanel;
    public UI_GameMenu menu;

    public MVPlayer mvPlayer;
    public UI_FadeScreen fadeScreen;
    public UI_Pause pause;

    public bool isMenu;
    public bool isPause;
    protected override void Awake()
    {
        base.Awake();

        worldCanvas = Instantiate(Resources.Load<Canvas>("UI/WorldCanvas"));
        DontDestroyOnLoad(worldCanvas);

        mvPlayer    = Instantiate(Resources.Load<MVPlayer>("UI/MVPlayer"), worldCanvas.transform);

        rectCanvas  = Instantiate(Resources.Load<Canvas>("UI/RectCanvas"));
        DontDestroyOnLoad(rectCanvas);

        menu = Instantiate(Resources.Load<UI_GameMenu>("UI/Menu"), rectCanvas.transform);
        settingPanel = Instantiate(Resources.Load<SettingPanel>("UI/SettingPanel"), rectCanvas.transform);
        pause = Instantiate(Resources.Load<UI_Pause>("UI/Pause"), rectCanvas.transform);
        fadeScreen  = Instantiate(Resources.Load<UI_FadeScreen>("UI/FadeScreen"), rectCanvas.transform);
    }

    private void Start()
    {
        inputActions = JudgeManager.Instance.inputActions;

        var uiMap = inputActions.FindActionMap("UI");

        menuAction = uiMap.FindAction("Menu");

        menuAction.Enable();

        menuAction.started += (ctx) =>
        {
            if (GameManager.Instance.isBusy) return;

            if (settingPanel.gameObject.activeSelf)
            {
                settingPanel.ClosePanel();
                return;
            }

            if (!menu.gameObject.activeSelf)
            {
                menu.OpenPanel();
                isMenu = true;
            }            
        };
    }    
}