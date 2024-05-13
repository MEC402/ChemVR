using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbedObjectReporter : MonoBehaviour
{
    public GameObject leftHandObject = null;
    public GameObject rightHandObject = null;
    //private XRDirectInteractor leftInteractor = null;
    //private XRDirectInteractor rightInteractor = null;

    private void Awake()
    {
        //leftInteractor = leftHandObject.GetComponent<XRDirectInteractor>();
        //rightInteractor = rightHandObject.GetComponent<XRDirectInteractor>();
    }

    private void OnEnable()
    {
        // This way of doing things has been deprecated. Listeners are added to the XRDirectInteractor component instead.

        //leftInteractor.onSelectEntered.AddListener(Grab);
        //leftInteractor.onSelectExited.AddListener(Drop);
        //rightInteractor.onSelectEntered.AddListener(Grab);
        //rightInteractor.onSelectExited.AddListener(Drop);
    }

    private void OnDisable()
    {
        // This way of doing things has been deprecated. Listeners are removed from the XRDirectInteractor component instead.

        //leftInteractor.onSelectEntered.RemoveListener(Grab);
        //leftInteractor.onSelectExited.RemoveListener(Drop);
        //rightInteractor.onSelectEntered.RemoveListener(Grab);
        //rightInteractor.onSelectExited.RemoveListener(Drop);
    }

    public void Grab(XRBaseInteractable interactable)
    {
        Debug.Log("A " + interactable.gameObject.name + " was grabbed.");
        GameEventsManager.instance.interactableEvents.PlayerGrabInteractable(interactable.gameObject);
    }

    public void Drop(XRBaseInteractable interactable)
    {
        Debug.Log("A " + interactable.gameObject.name + " was dropped.");
        GameEventsManager.instance.interactableEvents.PlayerDropInteractable(interactable.gameObject);
    }
}
