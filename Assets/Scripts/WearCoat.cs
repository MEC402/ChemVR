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

        if (leftHand != null)
            lHandRen = leftHand.GetComponent<Renderer>();
        if (rightHand != null)
            rHandRen = rightHand.GetComponent<Renderer>();

        wearing = false;
        GameEventsManager.instance.inputEvents.onRTriggerPressed += OnAPress;
        GameEventsManager.instance.inputEvents.onLTriggerPressed += OnXPress;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onRTriggerPressed -= OnAPress;
        GameEventsManager.instance.inputEvents.onLTriggerPressed -= OnXPress;
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
        if (handRen == null)
            return false;

        // Check if the bounds of the renderers intersect
        return handRen.bounds.Intersects(itemRen.bounds);
    }

    /// <summary>
    /// This method is called from the WebGL build since the input system is different
    /// </summary>
    public void WebPutOn()
    {
        PutOn();
        ActiveItemsCanvas.Instance.UpdateItemUI();
    }

    private void PutOn()
    {
        AudioEventManager.LabCoatSound();
        wearing = true;
        itemToWear.SetActive(false);
    }

    static public bool IsWearing()
    {
        return wearing;
    }




}
