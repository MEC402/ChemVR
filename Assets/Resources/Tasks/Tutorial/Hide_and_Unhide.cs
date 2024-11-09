using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Hide_and_Unhide : TaskStep
{
    
    int buttonPress = 0;

    void OnEnable()
    {
        GameEventsManager.instance.taskEvents.TaskStartApproved("Tutorial");

        GameEventsManager.instance.miscEvents.SetHint(null);

        GameEventsManager.instance.inputEvents.onYButtonPressed += TextToggleTutorial;
        //GameEventsManager.instance.inputEvents.onLTriggerPressed += TextToggleTutorial;
    }

    void OnDisable()
    {

        GameEventsManager.instance.inputEvents.onYButtonPressed -= TextToggleTutorial;
        //GameEventsManager.instance.inputEvents.onLTriggerPressed -= TextToggleTutorial;
    }

    void TextToggleTutorial(InputAction.CallbackContext context)
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
