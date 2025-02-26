using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Highlight_Selection : MonoBehaviour
{
    private XRSimpleInteractable interactable;
    public MeshRenderer highlight;

    // void Awake()
    // {
    //     interactable = GetComponent<XRSimpleInteractable>();
    // }

    // void OnEnable()
    // {
    //     interactable.hoverEntered.AddListener(OnHoverEnter);
    //     interactable.hoverExited.AddListener(OnHoverExit);
    // }

    // void OnDisable()
    // {
    //     interactable.hoverEntered.RemoveListener(OnHoverEnter);
    //     interactable.hoverExited.AddListener(OnHoverExit);
    // }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        Debug.Log("hovering over " + args.ToString());
        highlight.enabled = true;
    }

    public void OnHoverExit(HoverExitEventArgs arg0)
    {
        highlight.enabled = false;
    }
}
