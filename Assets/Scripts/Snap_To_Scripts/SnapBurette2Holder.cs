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
    private Transform parent;
    private Rigidbody stopcockRb;

    void OnEnable()
    {
        //Initialize
        touching = false;
        snap = false;
        parent = transform.parent;

        //get components
        myRb = GetComponent<Rigidbody>();
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        Rigidbody[] rbList = this.gameObject.GetComponentsInChildren<Rigidbody>();
        //Find the rigidbody of the stopcock
        foreach (Rigidbody rb in rbList)
        {
            if (rb != myRb)
            {
                stopcockRb = rb;
            }
        }

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
    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
    private void SetPosition()
    {
        transform.SetParent(holder.transform);
        myRb.isKinematic = true;
        Quaternion additionalRotation = Quaternion.Euler(0, 180, 0);
        Quaternion newRotation = holder.transform.rotation * additionalRotation;
        this.transform.SetPositionAndRotation(holder.transform.position, newRotation);
        this.transform.Translate(new Vector3(0.0504f, 0.531f, 0));
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
        if(snap)
        {
            snap = false;
            //stopcockRb.isKinematic = false;
            transform.SetParent(parent);
            myRb.isKinematic = false;
            GameEventsManager.instance.miscEvents.BuretUnSnaptoHolder(this.gameObject, holder);
        }
        holder = null;
        myRb.useGravity = true;
    }
    private void OnRelease(SelectExitEventArgs arg0)
    {
        if (touching)
        {
            //stopcockRb.isKinematic = true;
            snap = true;
            myRb.useGravity = false;
            SetPosition();
            GameEventsManager.instance.miscEvents.BuretSnaptoHolder(this.gameObject, holder);
        }
    }

    public bool isSnapped()
    {
        return snap;
    }
    public GameObject getStand()
    {
        return holder;
    }
}
