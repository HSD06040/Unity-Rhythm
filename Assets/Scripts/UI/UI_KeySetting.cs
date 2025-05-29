using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_KeySetting : BaseUI
{
    private InputActionAsset inputActions;

    private TMP_Text lane1KeyText;
    private TMP_Text lane2KeyText;
    private TMP_Text lane3KeyText;
    private TMP_Text lane4KeyText;

    private Button lane1KeyButton;
    private Button lane2KeyButton;
    private Button lane3KeyButton;
    private Button lane4KeyButton;

    protected override void Awake()
    {
        base.Awake();

        lane1KeyText = GetUI<TextMeshProUGUI>("Lane1KeyText");
        lane2KeyText = GetUI<TextMeshProUGUI>("Lane2KeyText");
        lane3KeyText = GetUI<TextMeshProUGUI>("Lane3KeyText");
        lane4KeyText = GetUI<TextMeshProUGUI>("Lane4KeyText");

        lane1KeyButton = GetUI<Button>("Lane1KeyButton");
        lane2KeyButton = GetUI<Button>("Lane2KeyButton");
        lane3KeyButton = GetUI<Button>("Lane3KeyButton");
        lane4KeyButton = GetUI<Button>("Lane4KeyButton");

        inputActions = JudgeManager.Instance.inputActions;

        lane1KeyButton.onClick.AddListener(() => StartKeyRebind("Lane1"));
        lane2KeyButton.onClick.AddListener(() => StartKeyRebind("Lane2"));
        lane3KeyButton.onClick.AddListener(() => StartKeyRebind("Lane3"));
        lane4KeyButton.onClick.AddListener(() => StartKeyRebind("Lane4"));
    }

    private void Start()
    {
        UpdateAllKeyTexts();
    }

    private void StartKeyRebind(string actionName)
    {
        InputAction action = inputActions.FindAction(actionName);
        if (action == null)
        {
            Debug.LogError($"InputAction '{actionName}' not found.");
            return;
        }

        action.Disable();

        action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                operation.Dispose();
                action.Enable();

                UpdateKeyText(actionName, action.bindings[0].ToDisplayString());
            })
            .Start();
    }
    private void UpdateKeyText(string actionName, string keyDisplay)
    {
        switch (actionName)
        {
            case "Lane1":
                lane1KeyText.text = keyDisplay;
                break;
            case "Lane2":
                lane2KeyText.text = keyDisplay;
                break;
            case "Lane3":
                lane3KeyText.text = keyDisplay;
                break;
            case "Lane4":
                lane4KeyText.text = keyDisplay;
                break;
        }
    }
    private void UpdateAllKeyTexts()
    {
        UpdateKeyText("Lane1", GetBindingDisplay("Lane1"));
        UpdateKeyText("Lane2", GetBindingDisplay("Lane2"));
        UpdateKeyText("Lane3", GetBindingDisplay("Lane3"));
        UpdateKeyText("Lane4", GetBindingDisplay("Lane4"));
    }

    private string GetBindingDisplay(string actionName)
    {
        var action = inputActions.FindAction(actionName);

        return action.bindings[0].ToDisplayString();
    }
}
