using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class SnapBulbToPipet : MonoBehaviour
{
    private XRGrabInteractable grabInteractable; //XRGrabInteractable of attached gameObject
    private Rigidbody myRb; //Rigidbody of attached gameObject
    private bool touching; //is this collider touching a pipet collieder?
    private bool snap; //is this bulb gameObject attached to a pipet?
    private bool isGrabbed; //is the bulb grabbed? (so pipet doesn't detach unless intentional)
    private GameObject pipet; //the pipet gameobject

    // ADDED FOR TESTING
    Vector3 OGbulbTranslation = new Vector3(0, 0, -0.22f);

    private void Update()
    {
        if (snap)
        {
            SetPositionToPipet();
        }
    }

    void OnEnable()
    {
        //Initialize
        touching = false;
        snap = false;
        isGrabbed = false;

        //get components
        myRb = GetComponent<Rigidbody>();
        grabInteractable = this.GetComponent<XRGrabInteractable>();

        //Initialize grab interactable for listening
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
            return;
        }

        //Add listeners
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }
    private void SetPositionToPipet()
    {
        // ADDED FOR TESTING
        Quaternion additionalRotation = Quaternion.Euler(0, 0, 0);

        //Move the bulb to the burret
        Quaternion newRotation = pipet.transform.rotation * additionalRotation;
        this.transform.SetPositionAndRotation(pipet.transform.position, newRotation);
        this.transform.Translate(OGbulbTranslation);
    }
    private void LetGo()
    {
        snap = false;
        pipet = null;
        myRb.useGravity = true;
    }
    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("ipette") && !snap)
        {
            pipet = other.gameObject;
            touching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("ipette"))
        {
            touching = false;
            if (isGrabbed)
            {
                LetGo();
            }
            else if (snap)
            {
                SetPositionToPipet();
            }
        }
    }
    private void OnGrab(SelectEnterEventArgs arg0)
    {
        isGrabbed = true;
        LetGo();
    }
    private void OnRelease(SelectExitEventArgs arg0)
    {
        isGrabbed = false;
        if (touching)
        {
            snap = true;
            myRb.useGravity = false;
        }
        else
        {
            LetGo();
        }
    }
}
