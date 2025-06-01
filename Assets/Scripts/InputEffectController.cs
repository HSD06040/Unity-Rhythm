using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputEffectController : MonoBehaviour
{
    [SerializeField] private GameObject[] inputEffects;
    [SerializeField] private Image[] buttons;
    private InputAction[] lanes;

    private void Start()
    {
        lanes = JudgeManager.Instance.lanes;
    }

    private void Update()
    {
        for (int i = 0; i < lanes.Length; i++)
        {
            ActivateEffect(i);
        }
    }

    private void ActivateEffect(int idx)
    {
        if (lanes[idx].WasPressedThisFrame())
        {
            inputEffects[idx].SetActive(true);
            buttons[idx].color = Color.gray;
        }
        else if (lanes[idx].WasReleasedThisFrame())
        {
            inputEffects[idx].SetActive(false);
            buttons[idx].color = Color.white;
        }
    }
}
