using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hover_Select : MonoBehaviour
{
    public enum direction { up, left, right, down };
    private XRSimpleInteractable interactable;
    public Wheel_UI_Manager wheelManager;
    public direction selection;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        Debug.Log("Hover Entered");
        if(selection == direction.up)
        {
            Debug.Log("Up");
            wheelManager.HandleRightStickUp();
        } else if (selection == direction.down)
        {
            Debug.Log("down");
            wheelManager.HandleRightStickDown();
        } else if (selection == direction.left)
        {
            Debug.Log("Left");
            wheelManager.HandleRightStickLeft();
        }
        else if (selection == direction.right)
        {
            Debug.Log("Right");
            wheelManager.HandleRightStickRight();
        }
    }

    private void OnHoverExit(HoverExitEventArgs arg0)
    {
        wheelManager.unSelectAll();
    }
}
