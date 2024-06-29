using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbedObjectReporter : MonoBehaviour
{
    public void OnEnable()
    {
        GameEventsManager.instance.interactableEvents.onPlayerGrabInteractable += Grab;
        GameEventsManager.instance.interactableEvents.onPlayerDropInteractable += Drop;
    }

    public void OnDisable()
    {
        GameEventsManager.instance.interactableEvents.onPlayerGrabInteractable -= Grab;
        GameEventsManager.instance.interactableEvents.onPlayerDropInteractable -= Drop;
    }
    public void Grab(GameObject gameObject)
    {
        Debug.Log(gameObject.name + " was grabbed");        
    }

    public void Drop(GameObject gameObject)
    {
        Debug.Log(gameObject.name + " was dropped");
    }
}
