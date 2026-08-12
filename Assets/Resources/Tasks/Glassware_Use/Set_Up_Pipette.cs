using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Set_Up_Pipette : TaskStep
{
    private bool pipetteOneSetup = false;
    private bool pipetteTwoSetup = false;


    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
        GameEventsManager.instance.miscEvents.OnPippetConnectedFirst += OnAttachBulb;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask += SkipTask;
    }
    void OnDisable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
        GameEventsManager.instance.miscEvents.OnPippetConnectedFirst -= OnAttachBulb;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask -= SkipTask;
    }

    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }

    private void OnAttachBulb()
    {
        if(!pipetteOneSetup && !pipetteTwoSetup)
        {
            pipetteOneSetup = true;
            return;
        }
        else if (pipetteOneSetup && !pipetteTwoSetup)
        {
            pipetteTwoSetup = true;
            FinishTaskStep();
            return;
        }
        else if (pipetteOneSetup && pipetteTwoSetup)
        {
            FinishTaskStep();
        }
    }


}
