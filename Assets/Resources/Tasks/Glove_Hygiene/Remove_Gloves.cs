using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Remove_Gloves : TaskStep
{
    bool wearingLeftGlove;
    bool wearingRightGlove;
    private void Start()
    {
        if (GameObject.Find("left hand model") != null)
            wearingLeftGlove = GameObject.Find("left hand model").GetComponent<SkinnedMeshRenderer>().material.name.ToLower().Contains("blue");
        else
            wearingLeftGlove = ActiveItemsCanvas.Instance.isWearingGloves;

        if (GameObject.Find("right hand model") != null)
            wearingRightGlove = GameObject.Find("right hand model").GetComponent<SkinnedMeshRenderer>().material.name.ToLower().Contains("blue");
        else
            wearingRightGlove = ActiveItemsCanvas.Instance.isWearingGloves;

        if (!wearingLeftGlove && !wearingRightGlove)
        {
            FinishTaskStep();
        }
    }
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("trash"));

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
