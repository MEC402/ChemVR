using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class AddGloves : MonoBehaviour
{
    public Material gloves;
    public GameObject leftHand;
    public GameObject rightHand;
    private Material original;
    private bool leftIsTouching;
    private bool rightIsTouching;

    private enum HandType { None, Left, Right }
    private HandType currentHand = HandType.None;


    void Start()
    {
        // Get and store the original material
        leftIsTouching = false;
        rightIsTouching = false;
        if (rightHand != null)
            original = rightHand.GetComponent<SkinnedMeshRenderer>().material;
    }
    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onRTriggerPressed += OnAPress;
        GameEventsManager.instance.inputEvents.onLTriggerPressed += OnXPress;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onRTriggerPressed -= OnAPress;
        GameEventsManager.instance.inputEvents.onLTriggerPressed -= OnXPress;
    }

    void OnAPress(InputAction.CallbackContext context)
    {
        if (rightIsTouching && (rightHand.GetComponent<SkinnedMeshRenderer>().material.name.Contains(original.name)))
        {
            PutOnRightGlove();
        }
    }
    void OnXPress(InputAction.CallbackContext context)
    {
        if (leftIsTouching && (leftHand.GetComponent<SkinnedMeshRenderer>().material.name.Contains(original.name)))
        {
            PutOnLeftGlove();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("right") && other.name.Contains("hand"))
        {
            rightIsTouching = true;
        }
        else if (other.name.Contains("left") && other.name.Contains("hand"))
        {
            leftIsTouching = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("right") && other.name.Contains("hand"))
        {
            rightIsTouching = false;
        }
        else if (other.name.Contains("left") && other.name.Contains("hand"))
        {
            leftIsTouching = false;
        }
    }
    void PutOnLeftGlove()
    {
        leftHand.GetComponent<SkinnedMeshRenderer>().material = gloves;
        GameEventsManager.instance.miscEvents.PutOnLeftGlove();
        //Debug.Log("Left glove put on!");
    }
    void PutOnRightGlove()
    {
        rightHand.GetComponent<SkinnedMeshRenderer>().material = gloves;
        GameEventsManager.instance.miscEvents.PutOnRightGlove();
        //Debug.Log("Right glove put on!");
    }

    /// <summary>
    /// This method is called from the WebGL build since the input system is different
    /// </summary>
    public void WebPutOnGloves()
    {
        GameEventsManager.instance.miscEvents.PutOnLeftGlove();
        GameEventsManager.instance.miscEvents.PutOnRightGlove();
    }




        public void GetRayOverlap(HoverEnterEventArgs args)
    {
        if (args.interactorObject != null)
        {
            currentHand = DetermineGrabbingHand(args.interactorObject);
        }
        switch (currentHand)
        {
            case HandType.Left:
                leftIsTouching = true;
                break;
            case HandType.Right:
                rightIsTouching = true;
                break; 
        }
    }
    public void ResetRayOverlap(HoverExitEventArgs args)
    {
        if (args.interactorObject != null)
        {
            currentHand = DetermineGrabbingHand(args.interactorObject);
        }
        switch (currentHand)
        {
            case HandType.Left:
                leftIsTouching = false;
                break;
            case HandType.Right:
                rightIsTouching = false;
                break; 
        }
        currentHand = HandType.None;
    }


        private HandType DetermineGrabbingHand(IXRInteractor interactor)
    {
        var controllerTransform = interactor.transform;

        // Check transform hierarchy for left/right indicators
        Transform current = controllerTransform;
        while (current != null)
        {
            string name = current.name.ToLower();
            if (name.Contains("left")) return HandType.Left;
            if (name.Contains("right")) return HandType.Right;
            current = current.parent;
        }

        // Check XRController component for handedness
        var xrController = controllerTransform.GetComponentInParent<XRController>();
        if (xrController != null)
        {
            var controllerNode = xrController.controllerNode;
            if (controllerNode == UnityEngine.XR.XRNode.LeftHand) return HandType.Left;
            if (controllerNode == UnityEngine.XR.XRNode.RightHand) return HandType.Right;
        }

        // Default to right hand
        return HandType.Right;
    }
}
