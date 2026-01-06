using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class SnapToTray : MonoBehaviour
{
    private XRGrabInteractable grabInteractable; //XRGrabInteractable of attached gameObject
    private Rigidbody myRb; //Rigidbody of attached gameObject
    private bool touching; //is this collider touching a tray collieder?
    private bool snap; //is this bulb gameObject attached to a tray?
    private bool isGrabbed; //is the bulb grabbed? (so tray doesn't detach unless intentional)
    private GameObject tray; //the tray gameobject
    private int snapPointIndex = -1; //the index of the snap point on the tray
    private GameObject TrayPoint;
    private Vector3 Offset = new Vector3(0f, 0f, 0f);

    // ADDED FOR TESTING
    Vector3 OGbulbTranslation = new Vector3(0, 0, -0.001f);

    private void Update()
    {
        if (snap)
        {
            SetPositionToTray(Offset);
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
    private void SetPositionToTray(Vector3 offsetPOS)
    {
        // ADDED FOR TESTING
        Quaternion additionalRotation = Quaternion.Euler(0, 0, 0);

        //Move the me to the tray
        Quaternion newRotation = tray.transform.rotation * additionalRotation;
        this.transform.SetPositionAndRotation(tray.transform.position + offsetPOS, newRotation);
        this.transform.Translate(OGbulbTranslation);
    }
    private void LetGo()
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
            if (TrayPoint.TryGetComponent<TraySnap>(out TraySnap traySnap))
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
        if (touching)
        {
            snap = true;
            myRb.useGravity = false;
            if (TrayPoint.TryGetComponent<TraySnap>(out TraySnap traySnap))
            { traySnap.AddMe(this); }
        }
        else
        {
            LetGo();
        }
    }

    public void SetObject(GameObject theObject, int Index)
    {
        tray = theObject;
        snapPointIndex = Index;
    }
}