using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SyncFaucet : MonoBehaviour
{
    public GameObject collisionDetector; //stopcock collider
    public GameObject turner;
    public bool reverseRotation = false;
    private XRGrabInteractable grabInteractable;

    float OGturnerYRotation;
    float OGturnerZRotation;
    private void OnEnable()
    {
        grabInteractable = collisionDetector.GetComponent<XRGrabInteractable>();

        //Initialize grab interactable for listening
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
            return;
        }
        OGturnerYRotation = turner.transform.localEulerAngles.y;
        OGturnerZRotation = turner.transform.localEulerAngles.z;
    }


    void Update()
    {
        // Get the current rotation
        float colliderRotation = collisionDetector.transform.localEulerAngles.x;

        //Apply
        if (!reverseRotation)
        {
            turner.transform.localRotation = Quaternion.Euler(colliderRotation, OGturnerYRotation, OGturnerZRotation);
        }
        else
        {
            turner.transform.localRotation = Quaternion.Euler(-colliderRotation, OGturnerYRotation, OGturnerZRotation);
        }
    }
}
