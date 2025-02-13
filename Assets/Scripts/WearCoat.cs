using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WearCoat : MonoBehaviour
{
    public GameObject itemToWear;
    public GameObject leftHand;
    public GameObject rightHand;
    private Renderer itemRen;
    private Renderer lHandRen;
    private Renderer rHandRen;
    static private bool wearing;

    void Start()
    {
        itemRen = itemToWear.GetComponent<Renderer>();
        lHandRen = leftHand.GetComponent<Renderer>();
        rHandRen = rightHand.GetComponent<Renderer>();
        wearing = false;
        GameEventsManager.instance.inputEvents.onAButtonPressed += OnAPress;
        GameEventsManager.instance.inputEvents.onXButtonPressed += OnXPress;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= OnAPress;
        GameEventsManager.instance.inputEvents.onXButtonPressed -= OnXPress;
    }

    private void OnXPress(InputAction.CallbackContext obj)
    {
        if (!wearing && IsTouching(lHandRen))
        {
            PutOn();
        }
    }

    private void OnAPress(InputAction.CallbackContext obj)
    {
        if (!wearing && IsTouching(rHandRen))
        {
            PutOn();
        }
    }

    private bool IsTouching(Renderer handRen)
    {
        // Check if the bounds of the renderers intersect
        return handRen.bounds.Intersects(itemRen.bounds);
    }

    private void PutOn()
    {
        wearing = true;
        itemToWear.SetActive(false);
    }

    static public bool IsWearing()
    {
        return wearing;
    }




}
