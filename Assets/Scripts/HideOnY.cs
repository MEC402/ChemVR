using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HideOnY : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onRTriggerPressed += Hide;
    }

    // Update is called once per frame
    void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onRTriggerPressed -= Hide;
    }
    public void Update()
    {
        //Added for keyboard support
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            Hide();
        }
    }

    private void Hide(InputAction.CallbackContext obj)
    {
        this.gameObject.SetActive(false);
    }
    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
