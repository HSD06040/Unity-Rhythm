using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Manager : Manager<UI_Manager>
{
    private InputActionAsset inputActions;
    private InputAction settingAction;
    private InputAction menuAction;

    private SettingPanel settingPanel;
    public SettingPanel SettingPanel
    {
        get
        {
            if(settingPanel != null)
                return settingPanel;

            settingPanel = FindObjectOfType<SettingPanel>(true);
            if (settingPanel != null)
                return settingPanel;

            settingPanel = Resources.Load<SettingPanel>("UI/SettingPanel");

            return settingPanel;
        }
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
            if (!SettingPanel.gameObject.activeSelf)
                SettingPanel.OpenPanel();
            else
                SettingPanel.ClosePanel();
        };
    }    
}
