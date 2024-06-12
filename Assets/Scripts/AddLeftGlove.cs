using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class AddLeftGlove : MonoBehaviour
{
    public Material gloves; 
    public GameObject glovebox; 
    private GameObject leftHand; 
    private Material original;
    static private bool wearingLeftGlove;
    Renderer boxRen;
    Renderer handRen;

    void Start()
    {
        // Get and store the original material
        leftHand = GameObject.Find("left hand model");
        original = leftHand.GetComponent<SkinnedMeshRenderer>().material;
        wearingLeftGlove = false;
        boxRen = glovebox.GetComponent<Renderer>();
        handRen = leftHand.GetComponent<Renderer>();

        GameEventsManager.instance.inputEvents.onXButtonPressed += OnXPress;
    }

    void OnXPress(InputAction.CallbackContext context)
    {
        if (IsTouching())
        {
            PutOn();
        }
    }

    private bool IsTouching()
    {
        // Check if the bounds of the renderers intersect
        return handRen.bounds.Intersects(boxRen.bounds);
    }

    void PutOn()
    {
        if (!wearingLeftGlove)
        {
            wearingLeftGlove = true;
            leftHand.GetComponent<SkinnedMeshRenderer>().material = gloves;
            Debug.Log("Left glove put on!");
        }
    }

    static public bool HasGloves()
    {
        return wearingLeftGlove;
    }
}
