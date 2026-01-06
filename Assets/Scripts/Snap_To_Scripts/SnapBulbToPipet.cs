using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


public class SnapBulbToPipet : MonoBehaviour
{
    private XRGrabInteractable grabInteractable; //XRGrabInteractable of attached gameObject
    private Rigidbody myRb; //Rigidbody of attached gameObject
    private bool touching; //is this collider touching a pipette collieder?
    private bool snap; //is this bulb gameObject attached to a pipette?
    private bool isGrabbed; //is the bulb grabbed? (so pipette doesn't detach unless intentional)
    private bool isHeld = false; //is the bulb held via WebGL grab?
    [SerializeField] private GameObject pipetteCollider; //the matching pipete object

    // ADDED FOR TESTING
    Vector3 OGbulbTranslation = new Vector3(0, 0, -0.001f);

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

        // WebGL listeners (if available)
        if (GameEventsManager.instance != null && GameEventsManager.instance.webGLEvents != null)
        {
            GameEventsManager.instance.webGLEvents.OnObjectGrabbed += WebGLGrab;
            GameEventsManager.instance.webGLEvents.OnObjectReleased += WebGLRelease;
        }
    }
    private void SetPositionToPipet()
    {
        // ADDED FOR TESTING
        Quaternion additionalRotation = Quaternion.Euler(0, 0, 0);

        //Move the bulb to the burret
        Quaternion newRotation = pipetteCollider.transform.rotation * additionalRotation;
        this.transform.SetPositionAndRotation(pipetteCollider.transform.position, newRotation);
        this.transform.Translate(OGbulbTranslation);
    }
    private void LetGo()
    {
        snap = false;
        myRb.useGravity = true;
    }
    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);

        // WebGL listeners (if available)
        if (GameEventsManager.instance != null && GameEventsManager.instance.webGLEvents != null)
        {
            GameEventsManager.instance.webGLEvents.OnObjectGrabbed -= WebGLGrab;
            GameEventsManager.instance.webGLEvents.OnObjectReleased -= WebGLRelease;
        }

        // Make sure input events are cleaned up
        if (GameEventsManager.instance != null && GameEventsManager.instance.inputEvents != null)
        {
            GameEventsManager.instance.inputEvents.onRTriggerPressed -= AttachBulb;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PipetteBulb") && !snap)
        {
            touching = true;
            GameEventsManager.instance.miscEvents.PippetConnectedFirst();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PipetteBulb"))
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

    // WebGL Grab Handlers, these allow for pressing the F key to attach the bulbs, instead of holding the two objects like you do in VR.
    private void WebGLGrab(GameObject grabbedObject)
    {
        if (grabbedObject == gameObject)
        {
            isGrabbed = true;
            isHeld = true;
            LetGo();
            if (GameEventsManager.instance != null && GameEventsManager.instance.inputEvents != null)
                GameEventsManager.instance.inputEvents.onRTriggerPressed += AttachBulb;
        }
    }

    private void WebGLRelease(GameObject releasedObject)
    {
        if (releasedObject == gameObject)
        {
            isGrabbed = false;
            isHeld = false;
            if (touching)
            {
                snap = true;
                myRb.useGravity = false;
            }
            else
            {
                LetGo();
            }

            if (GameEventsManager.instance != null && GameEventsManager.instance.inputEvents != null)
            {
                GameEventsManager.instance.inputEvents.onRTriggerPressed -= AttachBulb;
            }
        }
    }

    public void AttachBulb(InputAction.CallbackContext context)
    {
        if (isHeld)
        {
            GameObject player = GameObject.Find("FP Player");
            if (player != null)
            {
                WebGLGrab webGrab = player.GetComponent<WebGLGrab>();
                if (webGrab != null)
                {
                    webGrab.ForceReleaseObject();
                    isHeld = false;
                    isGrabbed = false;
                    snap = true;
                    myRb.useGravity = false;
                    if (GameEventsManager.instance != null && GameEventsManager.instance.inputEvents != null)
                        GameEventsManager.instance.inputEvents.onRTriggerPressed -= AttachBulb;
                }
            }

        }
    }
}
