using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapFunnel2Burette : MonoBehaviour
{
    DisableHoldable disableHoldable; //DisableHoldable script attached to gameObject
    private XRGrabInteractable grabInteractable; //XRGrabInteractable of attached gameObject
    private Rigidbody myRb; //Rigidbody of attached gameObject
    private bool touching; //is this collider touching a buret collieder?
    private bool snap; //is this funnel gameObject attached to a buret?
    private bool buretsnap; //is the attached buret attached to a holder?
    private bool isGrabbed; //is the funnel grabbed? (so funnel doesn't detach unless intentional)
    private GameObject buret; //the buret gameobject
    private GameObject stand; // the stand gameObject
    Vector3 OGfunnelTranslation = new Vector3(0, 0, 0.17f);

    private void Update()
    {
        if (buretsnap)
        {
            SetPositionToStand();
        }
        else if (snap)
        {
            SetPositionToBuret();
        }
    }

    void OnEnable()
    {
        //Initialize
        touching = false;
        snap = false;
        buretsnap = false;
        isGrabbed = false;

        //get components
        myRb = GetComponent<Rigidbody>();
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        disableHoldable = GetComponent<DisableHoldable>();

        //Initialize grab interactable for listening
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
            return;
        }

        //Add listeners
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        GameEventsManager.instance.miscEvents.onBuretSnaptoHolder += OnHolder;
        GameEventsManager.instance.miscEvents.onBuretUnSnaptoHolder += OffHolder;
    }
    private void SetPositionToBuret()
    {
        //Move the funnel to the burret

        Quaternion additionalRotation = Quaternion.Euler(-90, 0, 0);
        Quaternion newRotation = buret.transform.rotation * additionalRotation;
        this.transform.SetPositionAndRotation(buret.transform.position, newRotation);
        this.transform.Translate(OGfunnelTranslation);

        disableHoldable.Disable();
    }
    private void SetPositionToStand()
    {
        // Move the funnel to the buret on the holder
        //These are the movements applied to the buret
        Vector3 buretTranslation = new Vector3(-0.0504f, 0, 0.531f);

        Quaternion additionalRotation = Quaternion.Euler(-90, 0, 0);
        Quaternion newRotation = stand.transform.rotation * additionalRotation;
        this.transform.SetPositionAndRotation(stand.transform.position, newRotation);
        Vector3 AdjustedFunnelTranslation = OGfunnelTranslation + buretTranslation;
        this.transform.Translate(AdjustedFunnelTranslation);
    }
    private void OnHolder(GameObject buret, GameObject holder)
    {
        if (snap && (buret == this.buret))
        {
            buretsnap = true;
            stand = holder;
            SetPositionToBuret();
        }
    }
    private void OffHolder(GameObject buret, GameObject holder)
    {
        if (snap && (buret == this.buret))
        {
            buretsnap = false;
            stand = null;
        }
    }
    private void LetGo()
    {
        if (snap)
        {
            GameEventsManager.instance.miscEvents.FunnelUnSnaptoBuret(this.gameObject, buret);
        }
        snap = false;
        buretsnap = false;
        buret = null;
        stand = null;
        myRb.useGravity = true;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        this.gameObject.GetComponent<MeshCollider>().enabled = true;
    }
    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);

        GameEventsManager.instance.miscEvents.onBuretSnaptoHolder -= OnHolder;
        GameEventsManager.instance.miscEvents.onBuretUnSnaptoHolder -= OffHolder;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("urette") && !snap)
        {
            buret = other.gameObject;
            touching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("urette"))
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
            this.gameObject.GetComponent<MeshCollider>().enabled = false;
            this.gameObject.GetComponent<SphereCollider>().enabled = true;
            if (buret.GetComponent<SnapBurette2Holder>().isSnapped()) // is the buret already on a holder?
            {
                buretsnap = true;
                stand = buret.GetComponent<SnapBurette2Holder>().getStand();
                SetPositionToStand();
            }
            GameEventsManager.instance.miscEvents.FunnelSnaptoBuret(this.gameObject, buret);
        }
        else
        {
            LetGo();
        }
    }
}
