using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class AddRightGlove : MonoBehaviour
{
    public Material gloves; 
    public GameObject glovebox;
    private GameObject rightHand;
    private Material original;
    static private bool wearingRightGlove;
    Renderer boxRen;
    Renderer handRen;

    void Start()
    {
        // Get and store the original material
        rightHand = GameObject.Find("right hand model");
        original = rightHand.GetComponent<SkinnedMeshRenderer>().material;
        wearingRightGlove = false;
        boxRen = glovebox.GetComponent<Renderer>();
        handRen = rightHand.GetComponent<Renderer>();
        GameEventsManager.instance.inputEvents.onAButtonPressed += OnAPress;
    }

    void OnAPress(InputAction.CallbackContext context)
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
        if (!wearingRightGlove)
        {
            wearingRightGlove = true;
            rightHand.GetComponent<SkinnedMeshRenderer>().material = gloves;
            Debug.Log("Right glove put on!");
        }
    }

    static public bool HasGloves()
    {
        return wearingRightGlove;
    }
}
