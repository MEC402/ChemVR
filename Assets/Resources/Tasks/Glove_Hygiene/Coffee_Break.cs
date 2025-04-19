using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class Coffee_Break : TaskStep
{
    bool holdingCoffee = false;
    public GameObject coffee;
    private XRGrabInteractable grabInteractable;
    protected override void SetTaskStepState(string state)
    {
        //Not needed
    }
    void OnEnable()
    {
        coffee = GameObject.Find("coffee");

        GameEventsManager.instance.miscEvents.SetHint(coffee);

        grabInteractable = coffee.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
            return;
        }
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        GameEventsManager.instance.inputEvents.onAButtonPressed += drink;
        GameEventsManager.instance.inputEvents.onXButtonPressed += drink;
    }
    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
        GameEventsManager.instance.inputEvents.onAButtonPressed -= drink;
        GameEventsManager.instance.inputEvents.onXButtonPressed -= drink;
    }
    private void OnGrab(SelectEnterEventArgs arg0)
    {
        //Debug.LogWarning(arg0.interactableObject);
        // if (arg0.interactableObject.Equals(coffee.GetComponent<IXRSelectInteractable>()))
        // {
        //Debug.LogWarning("Picked up coffee");
        holdingCoffee = true;
        // }
    }
    private void OnRelease(SelectExitEventArgs arg0)
    {
        //Debug.LogWarning(arg0.interactableObject);
        // if (arg0.interactableObject.Equals(coffee.GetComponent<IXRSelectInteractable>()))
        // {
        //Debug.LogWarning("Put down coffee");
        holdingCoffee = false;
        // }
    }
    private void drink(InputAction.CallbackContext obj)
    {
        if (holdingCoffee)
        {
            AudioEventManager.DrinkSound();

            FinishTaskStep();
        }
    }
}
