using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEffectController : MonoBehaviour
{
    [SerializeField] private GameObject[] inputEffects;
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
        }
        else if (lanes[idx].WasReleasedThisFrame())
        {
            inputEffects[idx].SetActive(false);
        }
    }
}
