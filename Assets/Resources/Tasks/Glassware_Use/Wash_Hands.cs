using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wash_Hands : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }

    private bool leftGloveOff = false;
    private bool rightGloveOff = false;
    private bool handsinwater = false;
    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            FinishTaskStep();
        }

        if (handsinwater)
        {
            FinishTaskStep();
        }
        /*if (leftGloveOff && rightGloveOff && handsinwater)*/ //need to create the trigger for the water part
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove += () => leftGloveOff = true;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove += () => rightGloveOff = true;
        GameEventsManager.instance.miscEvents.onHandsinWater += () =>
        {
            if (leftGloveOff && rightGloveOff)
            {
                handsinwater = true;
            }
        };
        // GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }
    void OnDisable()
    {
        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }
    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
