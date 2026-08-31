using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class SnapToTray : MonoBehaviour
{
    private XRGrabInteractable grabInteractable; //XRGrabInteractable of attached gameObject
    private Rigidbody myRb; //Rigidbody of attached gameObject
    private bool touching; //is this collider touching a tray collieder?
    private bool snap; //is this gameObject attached to a tray?
    private bool isGrabbed; //is the object grabbed? (so tray doesn't detach unless intentional)
    private GameObject tray; //the tray gameobject
    private int snapPointIndex = -1; //the index of the snap point on the tray
    private GameObject TrayPoint;
    [SerializeField] private Vector3 Offset = new Vector3(0f, 0f, 0f);

    [SerializeField] private float rotationOffset = 0f;

    [SerializeField] private Vector3 snappedLocalNudge = new Vector3(0, 0, -0.001f);

    [Tooltip("Radius used to catch a tray snap point on release when the grabbed item's collider didn't have time to overlap the tray's trigger before selectExited fired (fast-release timing).")]
    [SerializeField] private float releaseCheckRadius = 0.05f;

    private void Update()
    {
        if (snap)
        {
            SetPositionToTray(Offset);
            this.GameObject().transform.Rotate(rotationOffset, 0, 0);
        }
    }

    void OnEnable()
    {
        //If this was still occupying a tray slot when it was disabled (component/GameObject
        //toggled off and back on while snapped), free that slot first. Otherwise the slot
        //leaks permanently: touching gets wiped below, so OnGrab's own RemoveMe call can
        //never fire for it afterward either.
        if (snap && snapPointIndex != -1 && TrayPoint != null && TrayPoint.TryGetComponent<TraySnap>(out TraySnap traySnap))
        {
            traySnap.RemoveMe(snapPointIndex);
        }

        //Initialize
        touching = false;
        snap = false;
        isGrabbed = false;
        snapPointIndex = -1;

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
    private void SetPositionToTray(Vector3 offsetPOS)
    {
        // ADDED FOR TESTING
        Quaternion additionalRotation = Quaternion.Euler(0, 0, 0);

        //Move the me to the tray
        Quaternion newRotation = tray.transform.rotation * additionalRotation;
        this.transform.SetPositionAndRotation(tray.transform.position + offsetPOS, newRotation);
        this.transform.Translate(snappedLocalNudge);
    }
    public void LetGo()
    {
        snap = false;
        tray = null;
        myRb.useGravity = true;
    }
    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TrayCollider") && !snap)
        {
            touching = true;
            TrayPoint = other.gameObject;

            //A held item only snaps on release (see OnRelease), but an item that was
            //dropped away from the tray and free-fell onto it never gets a release
            //event near the tray, so snap as soon as it physically lands on it.
            if (!isGrabbed)
            {
                TrySnapToTray();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("TrayCollider"))
        {
            touching = false;
            if (isGrabbed)
            {
                LetGo();
            }
            else if (snap)
            {
                SetPositionToTray(Offset);
            }
        }
    }
    private void OnGrab(SelectEnterEventArgs arg0)
    {
        isGrabbed = true;
        if (touching)
        {
            if (snapPointIndex != -1 && TrayPoint.TryGetComponent<TraySnap>(out TraySnap traySnap))
            { traySnap.RemoveMe(snapPointIndex); }
            snapPointIndex = -1;
            LetGo();
        }
        else
        {
            LetGo();
        }

    }
    private void OnRelease(SelectExitEventArgs arg0)
    {
        isGrabbed = false;

        //The grabbed collider may not have had time to physically overlap the tray's
        //trigger yet if it was released while still moving quickly toward the tray.
        //Fall back to an explicit overlap check so a fast release doesn't just drop it.
        if (!touching)
        {
            TryFindNearbyTray();
        }

        if (touching)
        {
            TrySnapToTray();
        }
        else
        {
            LetGo();
        }
    }

    private void TrySnapToTray()
    {
        snap = true;
        myRb.useGravity = false;
        if (TrayPoint.TryGetComponent<TraySnap>(out TraySnap traySnap))
        { traySnap.AddMe(this); }
    }

    private void TryFindNearbyTray()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, releaseCheckRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("TrayCollider"))
            {
                touching = true;
                TrayPoint = hit.gameObject;
                break;
            }
        }
    }

    public void SetObject(GameObject theObject, int Index)
    {
        tray = theObject;
        snapPointIndex = Index;
    }

    public bool GetIsSnapped()
    {
        return snap;
    }
}