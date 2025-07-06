using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPressCheck : MonoBehaviour
{
    private PourActivator pA;
    //private bool canFluidPour = false;
    private void Start()
    {
        pA = this.gameObject.GetComponent<PourActivator>();
    }

    void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed += OnAButton;
        GameEventsManager.instance.inputEvents.onAButtonReleased += OffAButton;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= OnAButton;
        GameEventsManager.instance.inputEvents.onAButtonReleased -= OffAButton;
    }
    private void OnAButton(InputAction.CallbackContext context)
    {
        SetPourActivatorStatus(true);
    }

    private void OffAButton(InputAction.CallbackContext context)
    {
        SetPourActivatorStatus(false);
    }

    void SetPourActivatorStatus(bool canFluidPour)
    {
        if (canFluidPour)
        {
            //Debug.Log("flow should be active");
            pA.Activate();
        }
        else
        {
            //Debug.Log("flow should be off");
            pA.Deactivate();
        }
    }
}
