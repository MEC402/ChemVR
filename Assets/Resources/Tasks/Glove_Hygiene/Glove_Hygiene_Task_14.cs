using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glove_Hygiene_Task_14 : TaskStep
{
    bool wearingLeftGlove;
    bool wearingRightGlove;
    private void Start()
    {
        wearingLeftGlove = GameObject.Find("left hand model").GetComponent<SkinnedMeshRenderer>().material.name.Contains("blue");
        wearingRightGlove = GameObject.Find("right hand model").GetComponent<SkinnedMeshRenderer>().material.name.Contains("blue");
    }
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove += removeLeft;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove += removeRight;
        GameEventsManager.instance.miscEvents.onPutOnLeftGlove += addLeft;
        GameEventsManager.instance.miscEvents.onPutOnRightGlove += addRight;
    }

    private void removeLeft()
    {
        wearingLeftGlove = false;
        if (!wearingRightGlove)
        {
            FinishTaskStep();
        }
    }

    private void removeRight()
    {
        wearingRightGlove = false;
        if (!wearingLeftGlove)
        {
            FinishTaskStep();
        }
    }
    private void addLeft()
    {
        wearingLeftGlove = true;
    }

    private void addRight()
    {
        wearingRightGlove = true;
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onTakeOffLeftGlove -= removeLeft;
        GameEventsManager.instance.miscEvents.onTakeOffRightGlove -= removeRight;
        GameEventsManager.instance.miscEvents.onPutOnLeftGlove -= addLeft;
        GameEventsManager.instance.miscEvents.onPutOnRightGlove -= addRight;
    }
    

}
