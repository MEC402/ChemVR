using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUTTON_RENDERER_TESTING_SCRIPT : MonoBehaviour
{
    // On Button
    public GameObject onButton;
    private Renderer onRen;
    // Off Button
    public GameObject offButton;
    private Renderer offRen;
    // Mode Button
    public GameObject modeButton;
    private Renderer modeRen;
    // Tare Button
    public GameObject tareButton;
    private Renderer tareRen;

    // Hands
    public GameObject leftHand;
    public GameObject rightHand;
    // Hand renderers
    private Renderer lHandRen;
    private Renderer rHandRen;

    // Scale Manager
    public Scale_Manager manager;
    private bool hovering;

    //ADDED TO MAYBE IMPROVE
    private bool hoveringOn;
    private bool hoveringOff;
    private bool hoveringMode;
    private bool hoveringTare;
    //ADDED TO MAYBE IMPROVE


    private void OnEnable()
    {
        onRen = onButton.GetComponent<Renderer>();
        offRen = offButton.GetComponent<Renderer>();
        modeRen = modeButton.GetComponent<Renderer>();
        tareRen = tareButton.GetComponent<Renderer>();

        lHandRen = leftHand.GetComponent<Renderer>();
        rHandRen = rightHand.GetComponent<Renderer>();
        hoveringOn = false;
        hoveringOff = false;
        hoveringMode = false;
        hoveringTare = false;
}

    private void Update()
    {
        //If hover entered, wait for a button press or exit
        /*if (hovering)
        {
            if (lHandRen.bounds.Intersects(onRen.bounds) || rHandRen.bounds.Intersects(onRen.bounds)) 
            {
                manager.pressOn();
                hovering = false;
            }
            else if (lHandRen.bounds.Intersects(offRen.bounds) || rHandRen.bounds.Intersects(offRen.bounds))
            {
                manager.pressOff();
                hovering = false;
            }
            else if (lHandRen.bounds.Intersects(modeRen.bounds) || rHandRen.bounds.Intersects(modeRen.bounds))
            {
                manager.pressMode();
                hovering = false;
            }
            else if (lHandRen.bounds.Intersects(tareRen.bounds) || rHandRen.bounds.Intersects(tareRen.bounds))
            {
                manager.pressTare();
                hovering = false;
            }
        } */

        //ADDED TO MAYBE IMPROVE
        if (hoveringOn)
        {
            if (lHandRen.bounds.Intersects(onRen.bounds) || rHandRen.bounds.Intersects(onRen.bounds))
            {
                manager.pressOn();
                hoveringOn = false;
            }
        }
        if (hoveringOff)
        {
             if (lHandRen.bounds.Intersects(offRen.bounds) || rHandRen.bounds.Intersects(offRen.bounds))
            {
                manager.pressOff();
                hoveringOff = false;
            }
        }
        if (hoveringMode)
        {
            if (lHandRen.bounds.Intersects(modeRen.bounds) || rHandRen.bounds.Intersects(modeRen.bounds))
            {
                manager.pressMode();
                hoveringMode = false;
            }
        }
        if (hoveringTare)
        {
            if (lHandRen.bounds.Intersects(tareRen.bounds) || rHandRen.bounds.Intersects(tareRen.bounds))
            {
                manager.pressTare();
                hoveringTare = false;
            }
        }
        //ADDED TO MAYBE IMPROVE
    }

    public void HoverEnter()
    {
        hovering = true;
    }
    public void HoverExit()
    {
        hovering = false;
    }

    public void OnHoverEnter()
    {
        hoveringOn = true;
    }
    public void OnHoverExit()
    {
        hoveringOn = false;
    }
    public void OffHoverEnter()
    {
        hoveringOff = true;
    }
    public void OffHoverExit()
    {
        hoveringOff = false;
    }
    public void ModeHoverEnter()
    {
        hoveringMode = true;
    }
    public void ModeHoverExit()
    {
        hoveringMode = false;
    }
    public void TareHoverEnter()
    {
        hoveringTare = true;
    }
    public void TareHoverExit()
    {
        hoveringTare = false;
    }
}
