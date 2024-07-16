using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Glove_Hygiene_Task_3 : TaskStep
{
    bool holdingPhone = false;
    public GameObject phone;
    private XRGrabInteractable grabInteractable;
    protected override void SetTaskStepState(string state)
    {
        //Not needed
    }
    void OnEnable()
    {
        phone = GameObject.Find("phone2k");
        grabInteractable = phone.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
            return;
        }
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        GameEventsManager.instance.inputEvents.onAButtonPressed += answer;
        GameEventsManager.instance.inputEvents.onXButtonPressed += answer;
    }
    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
        GameEventsManager.instance.inputEvents.onAButtonPressed -= answer;
        GameEventsManager.instance.inputEvents.onXButtonPressed -= answer;
    }
    private void OnGrab(SelectEnterEventArgs arg0)
    {
        //Debug.LogWarning(arg0.interactableObject);
        if (arg0.interactableObject.Equals(phone.GetComponent<IXRSelectInteractable>()))
        {
            //Debug.LogWarning("Picked up phone");
            holdingPhone = true;
        }
    }
    private void OnRelease(SelectExitEventArgs arg0)
    {
        //Debug.LogWarning(arg0.interactableObject);
        if (arg0.interactableObject.Equals(phone.GetComponent<IXRSelectInteractable>()))
        {
            //Debug.LogWarning("Put down phone");
            holdingPhone = false;
        }
    }
    private void answer(InputAction.CallbackContext obj)
    {
        if(holdingPhone)
        {
            FinishTaskStep();
        }
    }
}
