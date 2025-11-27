using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupHighlighter : MonoBehaviour
{
    public GameObject highlightPopup;
    public float flashDuration = 0.175f; // Duration each flash state lasts
    private Renderer highlightRenderer;

    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onTextPopUp += StartFlashing;
    }
    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onTextPopUp -= StartFlashing;
    }
    private void Start()
    {
        if (highlightPopup != null)
        {
            highlightRenderer = highlightPopup.GetComponent<MeshRenderer>();
            if (highlightRenderer == null)
            {
                Debug.LogError("No Renderer found on the target object.");
            }
        }
        else
        {
            Debug.LogError("No highlight object assigned.");
        }

    }
    public void StartFlashing()
    {
        if (highlightRenderer != null)
        {
            StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
    {        
        // Make the object visible
        highlightRenderer.enabled = true;
        yield return new WaitForSeconds(flashDuration);

        // Make the object invisible
        highlightRenderer.enabled = false;
        yield return new WaitForSeconds(flashDuration);

        // Make the object visible again
        highlightRenderer.enabled = true;
        yield return new WaitForSeconds(flashDuration);

        // Make the object invisible again
        highlightRenderer.enabled = false;
    }
}
