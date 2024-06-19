using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapFunnel2Burette : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Rigidbody myRb;
    private bool touching;
    private bool snap;
    private GameObject burette;

    private void Update()
    {
        if (snap)
        {
            Quaternion additionalRotation = Quaternion.Euler(-90, 0, 0);
            Quaternion newRotation = burette.transform.rotation * additionalRotation;
            this.transform.SetPositionAndRotation(burette.transform.position, newRotation);
            this.transform.Translate(new Vector3(0.0f, 0.0f, 0.172f));
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
        if (other.name.Contains("urette"))
        {
            burette = other.gameObject;
            touching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("urette"))
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
            GameEventsManager.instance.miscEvents.FunnelSnaptoBuret();
        } else
        {
            snap = false;
            burette = null;
            myRb.useGravity = true;
            GameEventsManager.instance.miscEvents.FunnelUnSnaptoBuret();
        }
    }
}
