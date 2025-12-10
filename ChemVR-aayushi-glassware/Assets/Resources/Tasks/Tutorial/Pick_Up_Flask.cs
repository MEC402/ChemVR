using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Pick_Up_Flask : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    private void OnEnable()
    {
        GameEventsManager.instance.interactableEvents.onPlayerGrabInteractable += grabbedObject;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.interactableEvents.onPlayerGrabInteractable -= grabbedObject;
    }

    private void grabbedObject(GameObject obj)
    {
        if(obj.name.ToLower().Contains("flask"))
        {
            FinishTaskStep();
        }
    }
}
