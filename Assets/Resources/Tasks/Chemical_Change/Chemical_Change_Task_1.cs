using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Chemical_Change_Task_1 : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    bool wearingRightGlove;
    bool wearingLeftGlove;
    private void OnEnable()
    {
        wearingLeftGlove = GameObject.Find("left hand model").GetComponent<SkinnedMeshRenderer>().material.name.ToLower().Contains("blue");
        wearingRightGlove = GameObject.Find("right hand model").GetComponent<SkinnedMeshRenderer>().material.name.ToLower().Contains("blue");
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
        }
    }

}
