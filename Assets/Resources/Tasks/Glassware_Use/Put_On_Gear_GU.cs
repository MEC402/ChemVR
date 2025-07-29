using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Put_On_Gear_GU : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    bool wearingRightGlove;
    bool wearingLeftGlove;
    private void OnEnable()
    {
        GameEventsManager.instance.taskEvents.TaskStartApproved("Glassware_Use");

        wearingLeftGlove = false;
        wearingRightGlove = false;
        GameEventsManager.instance.miscEvents.onPutOnRightGlove += RightGloveOn;
        GameEventsManager.instance.miscEvents.onPutOnLeftGlove += LeftGloveOn;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove += RightGloveOff;
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove += LeftGloveOff;

        //GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onPutOnRightGlove -= RightGloveOn;
        GameEventsManager.instance.miscEvents.onPutOnLeftGlove -= LeftGloveOn;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove -= RightGloveOff;
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove -= LeftGloveOff;

        //GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }

    private void RightGloveOn()
    {
        wearingRightGlove = true;
    }

    private void LeftGloveOn()
    {
        wearingLeftGlove = true;
    }

    private void RightGloveOff()
    {
        wearingRightGlove = false;
    }

    private void LeftGloveOff()
    {
        wearingLeftGlove = false;
    }

    private void Update()
    {
        // If wearing both gloves, the coat, and goggles.
        if ((wearingLeftGlove && wearingRightGlove) && (WearCoat.IsWearing()) && (WearGoggles.IsWearing()))
        {
            FinishTaskStep();
        }
    }

    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
