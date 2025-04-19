using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Fold_Paper : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public bool folded;
    public MeshCollider flatPaperCollider;
    public MeshRenderer flatPaperRenderer;
    public MeshCollider foldedPaperCollider;
    public MeshRenderer foldedPaperRenderer;
    public MeshCollider halfFoldedPaperCollider;
    public MeshRenderer halfFoldedPaperRenderer;
    private void Start()
    {
        folded = false;
        grabInteractable = this.GetComponent<XRGrabInteractable>();

        //Initialize grab interactable for listening
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
            return;
        }

        if (flatPaperCollider == null || flatPaperRenderer == null)
        {
            Debug.LogError("flat paper components are missing.");
            return;
        }
        if (foldedPaperCollider == null || foldedPaperRenderer == null)
        {
            Debug.LogError("folded paper components are missing.");
            return;
        }
        if (halfFoldedPaperCollider == null || halfFoldedPaperRenderer == null)
        {
            Debug.LogError("half folded paper components are missing.");
            return;
        }

        //Add listeners
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs arg0)
    {
        if (flatPaperCollider.enabled == true)
        {
            folded = true;
            Debug.Log("Folded " + folded);
        }
            if (!folded)
        {
            GameEventsManager.instance.inputEvents.onAButtonPressed += FoldPaper;
            GameEventsManager.instance.inputEvents.onXButtonPressed += FoldPaper;
        }
    }

    private void OnRelease(SelectExitEventArgs arg0)
    {
        if (!folded)
        {
            GameEventsManager.instance.inputEvents.onAButtonPressed -= FoldPaper;
            GameEventsManager.instance.inputEvents.onXButtonPressed -= FoldPaper;
        }
    }

    private void FoldPaper(InputAction.CallbackContext obj)
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= FoldPaper;
        GameEventsManager.instance.inputEvents.onXButtonPressed -= FoldPaper;
        folded = true;

        //Fold the paper
        if(flatPaperRenderer.enabled)
        {
            flatPaperRenderer.enabled = false;
            flatPaperCollider.enabled = false;
        } else if (foldedPaperRenderer.enabled)
        {
            foldedPaperRenderer.enabled = false;
            foldedPaperCollider.enabled = false;
        }

        halfFoldedPaperRenderer.enabled = true;
        halfFoldedPaperCollider.enabled = true;
    }
}
