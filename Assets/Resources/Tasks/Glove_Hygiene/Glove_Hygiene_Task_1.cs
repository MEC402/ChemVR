using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glove_Hygiene_Task_1 : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    bool wearingRightGlove;
    bool wearingLeftGlove;
    private void OnEnable()
    {

        GameEventsManager.instance.taskEvents.TaskStartApproved("Glove_Hygiene");
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("glovebox"));

        wearingLeftGlove = false;
        wearingRightGlove = false;
        GameEventsManager.instance.miscEvents.onPutOnRightGlove += RightGloveOn;
        GameEventsManager.instance.miscEvents.onPutOnLeftGlove += LeftGloveOn;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove += RightGloveOff;
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove += LeftGloveOff;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onPutOnRightGlove -= RightGloveOn;
        GameEventsManager.instance.miscEvents.onPutOnLeftGlove -= LeftGloveOn;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove -= RightGloveOff;
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove -= LeftGloveOff;
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
        } else if (!(wearingLeftGlove && wearingRightGlove))
        {
            GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("glovebox"));
        } else if (!WearCoat.IsWearing())
        {
            GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("labcoat"));
        } else if (!WearGoggles.IsWearing())
        {
            GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("goggles"));
        }


    }

}
