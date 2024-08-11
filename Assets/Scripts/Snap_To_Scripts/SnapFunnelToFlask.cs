using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class SnapFunnelToFlask : MonoBehaviour
{
    private XRGrabInteractable grabInteractable; //XRGrabInteractable of attached gameObject
    private Rigidbody myRb; //Rigidbody of attached gameObject
    private bool touching; //is this collider touching a flask collieder?
    private bool snap; //is this funnel gameObject attached to a flask?
    private bool isGrabbed; //is the funnel grabbed? (so funnel doesn't detach unless intentional)
    private GameObject flask; //the flask gameobject
    Vector3 OGfunnelTranslation = new Vector3(0, 0, 0.2f);

    private void Update()
    {
        if (snap)
        {
            SetPositionToFlask();
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
    private void SetPositionToFlask()
    {
        //Move the funnel to the burret

        Quaternion additionalRotation = Quaternion.Euler(-90, 0, 0);
        Quaternion newRotation = flask.transform.rotation * additionalRotation;
        this.transform.SetPositionAndRotation(flask.transform.position, newRotation);
        this.transform.Translate(OGfunnelTranslation);
    }
    private void LetGo()
    {
        snap = false;
        flask = null;
        myRb.useGravity = true;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        this.gameObject.GetComponent<MeshCollider>().enabled = true;
    }
    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("lask") && !snap)
        {
            flask = other.gameObject;
            touching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("lask"))
        {
            touching = false;
            if (isGrabbed)
            {
                LetGo();
            }
            else if (snap)
            {
                SetPositionToFlask();
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
            this.gameObject.GetComponent<MeshCollider>().enabled = false;
            this.gameObject.GetComponent<SphereCollider>().enabled = true;
        }
        else
        {
            LetGo();
        }
    }
}
