using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Tutorial_Task_1 : TaskStep
{
    
    int buttonPress = 0;

    void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onXButtonPressed += TextToggleTutorial;
    }

    void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onXButtonPressed -= TextToggleTutorial;
    }

    void TextToggleTutorial(InputAction.CallbackContext context)
    {
        buttonPress += 1;
    }

    private void Update()
    {

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
