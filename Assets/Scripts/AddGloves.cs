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

    void Start()
    {
        // Get and store the original material
        leftIsTouching = false;
        rightIsTouching = false;
        original = rightHand.GetComponent<SkinnedMeshRenderer>().material;
    }
    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed += OnAPress;
        GameEventsManager.instance.inputEvents.onXButtonPressed += OnXPress;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= OnAPress;
        GameEventsManager.instance.inputEvents.onXButtonPressed -= OnXPress;
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
    public void WebPutOnLeftGloves()
    {
        GameEventsManager.instance.miscEvents.PutOnLeftGlove();
        GameEventsManager.instance.miscEvents.PutOnRightGlove();
    }
}
