using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Show_and_Hide_Help : TaskStep
{
    
    int buttonPress = 0;

    void OnEnable()
    {

        GameEventsManager.instance.miscEvents.SetHint(null);

        GameEventsManager.instance.inputEvents.onPauseButtonPressed += ToggleMenu;
    }

    void OnDisable()
    {

        GameEventsManager.instance.inputEvents.onPauseButtonPressed -= ToggleMenu;
    }

    void ToggleMenu(InputAction.CallbackContext context)
    {
        buttonPress += 1;
    }

    private void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            buttonPress++;
        }
        if (buttonPress >= 2)
        {
            FinishTaskStep();
        }
    }
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }

    

}
