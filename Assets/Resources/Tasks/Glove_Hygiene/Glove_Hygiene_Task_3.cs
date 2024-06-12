using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Glove_Hygiene_Task_3 : TaskStep
{
    bool holdingPhone = false;
    public GameObject phone;
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        phone = GameObject.Find("phone2k");
        GameEventsManager.instance.interactableEvents.onPlayerGrabInteractable += grabPhone;
        GameEventsManager.instance.interactableEvents.onPlayerDropInteractable += dropPhone;
        GameEventsManager.instance.inputEvents.onAButtonPressed += answer;
        GameEventsManager.instance.inputEvents.onAButtonPressed += SkipTask;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.interactableEvents.onPlayerGrabInteractable -= grabPhone;
        GameEventsManager.instance.interactableEvents.onPlayerDropInteractable -= dropPhone;
        GameEventsManager.instance.inputEvents.onAButtonPressed -= answer;
        GameEventsManager.instance.inputEvents.onAButtonPressed -= SkipTask;
    }
    private void answer(InputAction.CallbackContext obj)
    {
        if(holdingPhone)
        {
            FinishTaskStep();
        }
    }

    private void dropPhone(GameObject obj)
    {
        if (obj == phone)
        {
            Debug.LogWarning("Put down phone");
            holdingPhone = false;
        }
    }

    private void grabPhone(GameObject obj)
    {
        Debug.LogWarning("Picked up " + obj.name);
        if (obj == phone)
        {
            holdingPhone = true;
        }
    }

    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            FinishTaskStep();
        }
    }

    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
}
