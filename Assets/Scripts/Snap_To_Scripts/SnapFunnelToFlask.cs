using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapFunnel2Flask : MonoBehaviour
{
    private XRGrabInteractable grabInteractable; //XRGrabInteractable of attached gameObject
    private Rigidbody myRb; //Rigidbody of attached gameObject
    private bool touching; //is this collider touching a buret collieder?
    private bool snap; //is this funnel gameObject attached to a buret?
    private bool isGrabbed; //is the funnel grabbed? (so funnel doesn't detach unless intentional)
    private GameObject buret; //the buret gameobject
    private Transform parent;
    Vector3 OGfunnelTranslation = new Vector3(0, 0, 0.2f);//new Vector3(0, 0, 0.17f);

    void OnEnable()
    {
        //Initialize
        touching = false;
        snap = false;
        isGrabbed = false;
        parent = transform.parent;

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
    private void SetPositionToBuret()
    {
        //Move the funnel to the burret
        transform.SetParent(buret.transform);
        Quaternion additionalRotation = Quaternion.Euler(-90, 0, 0);
        Quaternion newRotation = buret.transform.rotation * additionalRotation;
        this.transform.SetPositionAndRotation(buret.transform.position, newRotation);
        this.transform.Translate(OGfunnelTranslation);
        myRb.isKinematic = true; // Disable physics
    }

    private void LetGo()
    {
        if (snap)
        {
            GameEventsManager.instance.miscEvents.FunnelUnSnaptoBuret(this.gameObject, buret);
        }
        snap = false;
        transform.SetParent(parent);
        EnableCollisionWithBuret();
        buret = null;
        myRb.useGravity = true;
        myRb.isKinematic = false; // Ensable physics
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
            buret = other.gameObject;
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
                SetPositionToBuret();
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
            DisableCollisionWithBuret();
            this.gameObject.GetComponent<MeshCollider>().enabled = false;
            this.gameObject.GetComponent<SphereCollider>().enabled = true;
            SetPositionToBuret();
            GameEventsManager.instance.miscEvents.FunnelSnaptoBuret(this.gameObject, buret);
        }
        else
        {
            LetGo();
        }
    }

    private void DisableCollisionWithBuret()
    {
        if (buret != null)
        {
            Collider funnelCollider = GetComponent<Collider>();
            Collider buretCollider = buret.GetComponent<Collider>();
            Physics.IgnoreCollision(funnelCollider, buretCollider, true);
        }
    }

    private void EnableCollisionWithBuret()
    {
        if (buret != null)
        {
            Collider funnelCollider = GetComponent<Collider>();
            Collider buretCollider = buret.GetComponent<Collider>();
            Physics.IgnoreCollision(funnelCollider, buretCollider, false);
        }
    }
}
