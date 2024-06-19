using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapBurette2Holder : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody myRb;
    private bool touching;
    private bool snap;
    private GameObject holder;

    private void Update()
    {
        if (snap)
        {
            Quaternion additionalRotation = Quaternion.Euler(0, 180, 0);
            Quaternion newRotation = holder.transform.rotation * additionalRotation;
            this.transform.SetPositionAndRotation(holder.transform.position, newRotation);
            this.transform.Translate(new Vector3(0.0504f, 0.531f, 0));
        }
    }

    void OnEnable()
    {
        //Initialize
        touching = false;
        snap = false;
        myRb = GetComponent<Rigidbody>();
        //Initialize grab interactable for listening
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
            return;
        }

        //Add listeners
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }
    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("holder"))
        {
            holder = other.gameObject;
            touching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the other collider is of a type B object
        if (other.name.Contains("holder"))
        {
            touching = false;
        }
    }
    private void OnGrab(SelectEnterEventArgs arg0)
    {
        snap = false;
    }
    private void OnRelease(SelectExitEventArgs arg0)
    {
        if (touching)
        {
            snap = true;
            myRb.useGravity = false;
            GameEventsManager.instance.miscEvents.BuretSnaptoHolder();
        } else
        {
            snap = false;
            holder = null;
            myRb.useGravity = true;
            GameEventsManager.instance.miscEvents.BuretUnSnaptoHolder();
        }
    }
}
