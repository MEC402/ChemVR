using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Put_Paper_on_Boat : MonoBehaviour
{
    private XRGrabInteractable grabInteractable; //XRGrabInteractable of attached gameObject
    private Rigidbody myRb; //Rigidbody of attached gameObject
    private bool touching; //is this collider touching a flask collieder?
    private bool snap; //is this paper gameObject attached to a boat?
    private bool isGrabbed; //is the paper grabbed? (so paper doesn't detach unless intentional)
    private GameObject boat; //the boat gameobject
    private float riseAmount = -0.004f;
    Vector3 OGfunnelTranslation;// = new Vector3(0, -0.015f, 0.0f);

    private float r = -36.5f;

    private MeshCollider flatPaperCollider;
    private MeshRenderer flatPaperRenderer;
    private MeshCollider foldedPaperCollider;
    private MeshRenderer foldedPaperRenderer;
    private MeshCollider halfFoldedPaperCollider;
    private MeshRenderer halfFoldedPaperRenderer;

    [HideInInspector] public bool isInBoat = false; //is the paper in the boat?

    private void Update()
    {
        if (snap)
        {
            SetPositionToBoat();
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

        Fold_Paper paperComponents = GetComponent<Fold_Paper>();
        flatPaperCollider = paperComponents.flatPaperCollider;
        flatPaperRenderer = paperComponents.flatPaperRenderer;
        foldedPaperCollider = paperComponents.foldedPaperCollider;
        foldedPaperRenderer = paperComponents.foldedPaperRenderer;
        halfFoldedPaperCollider = paperComponents.halfFoldedPaperCollider;
        halfFoldedPaperRenderer = paperComponents.halfFoldedPaperRenderer;

        if (flatPaperCollider == null || flatPaperRenderer == null)
        {
            Debug.LogError("flat paper components are missing.");
            return;
        }
        if (foldedPaperCollider == null || foldedPaperRenderer == null)
        {
            Debug.LogError("folded paper components are missing.");
            return;
        }
        if (halfFoldedPaperCollider == null || halfFoldedPaperRenderer == null)
        {
            Debug.LogError("half folded paper components are missing.");
            return;
        }

        //Add listeners
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // WebGL Listeners
        GameEventsManager.instance.webGLEvents.OnObjectGrabbed += OnWebGLGrab;
        GameEventsManager.instance.webGLEvents.OnObjectReleased += OnWebGLRelease;
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);

        // WebGL Listeners
        GameEventsManager.instance.webGLEvents.OnObjectGrabbed -= OnWebGLGrab;
        GameEventsManager.instance.webGLEvents.OnObjectReleased -= OnWebGLRelease;
    }

    private void SetPositionToBoat()
    {
        //Move the paper to the boat's position and rotation, but add a little bit of translation to the paper so it is not inside the boat
        Quaternion additionalRotation = Quaternion.Euler(90, 0, 0);
        Quaternion newRotation = boat.transform.rotation * additionalRotation;

        transform.SetPositionAndRotation(boat.transform.position, newRotation);
        transform.Translate(OGfunnelTranslation);
        transform.localRotation *= Quaternion.Euler(0, r, 0);
    }

    private void LetGo()
    {
        snap = false;
        boat = null;
        myRb.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("boat"))
        {
            boat = other.gameObject;
            touching = true;

            GameEventsManager.instance.miscEvents.PaperInBoat(true);

            if (other.name.Contains("mall"))
                OGfunnelTranslation = new Vector3(0, riseAmount, 0);
            else if (other.name.Contains("edium"))
                OGfunnelTranslation = new Vector3(0, 2 * riseAmount, 0);
            else if (other.name.Contains("arge"))
                OGfunnelTranslation = new Vector3(0, 3 * riseAmount, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("boat"))
        {
            touching = false;

            GameEventsManager.instance.miscEvents.PaperInBoat(false);

            if (isGrabbed)
                LetGo();
            else if (snap)
                SetPositionToBoat();
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
            if (foldedPaperCollider.enabled == false)
            {
                if (flatPaperCollider.enabled == true)
                {
                    flatPaperCollider.enabled = false;
                    flatPaperRenderer.enabled = false;
                }
                else if (halfFoldedPaperCollider.enabled == true)
                {
                    halfFoldedPaperCollider.enabled = false;
                    halfFoldedPaperRenderer.enabled = false;
                }
                foldedPaperCollider.enabled = true;
                foldedPaperRenderer.enabled = true;
            }
        }
        else
        {
            LetGo();
        }
    }

    #region WebGL Interactions
    /// <summary>
    /// Manually called for WebGL interactions when the player "grabs" an object.
    /// </summary>
    /// <param name="grabbedObject">The object that was grabbed.</param>
    private void OnWebGLGrab(GameObject grabbedObject)
    {
        if (grabbedObject != gameObject) return; // Ensure the grabbed object is this paper

        isGrabbed = true;
        LetGo();
    }

    /// <summary>
    /// Manually called for WebGL interactions when the player "releases" an object.
    /// </summary>
    /// <param name="releasedObject">The object that was released.</param>
    private void OnWebGLRelease(GameObject releasedObject)
    {
        if (releasedObject != gameObject) return; // Ensure the released object is this paper

        StartCoroutine(HandleWebGLRelease());
    }

    IEnumerator HandleWebGLRelease()
    {
        yield return new WaitForSeconds(0.25f); // Delay to allow for object to fall on boat

        // Reset the state of the paper
        isGrabbed = false;

        if (touching)
        {
            GetComponent<BoxCollider>().enabled = false; // Disable the collider to prevent further interactions

            snap = true;
            myRb.useGravity = false;

            if (foldedPaperCollider.enabled == false)
            {
                if (flatPaperCollider.enabled == true)
                {
                    flatPaperCollider.enabled = false;
                    flatPaperRenderer.enabled = false;
                }
                else if (halfFoldedPaperCollider.enabled == true)
                {
                    halfFoldedPaperCollider.enabled = false;
                    halfFoldedPaperRenderer.enabled = false;
                }

                foldedPaperCollider.enabled = true;
                foldedPaperRenderer.enabled = true;

                isInBoat = true; // Set the paper as being in the boat
            }
        }
        else
            LetGo();
    }
    #endregion
}
