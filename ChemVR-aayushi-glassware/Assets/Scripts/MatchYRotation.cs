using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MatchYRotation : MonoBehaviour
{
    public bool reverseDirection = false; //right now I can't seem to work this out

    private Transform handTransform;
    private bool isGrabbed = false;
    private float initialHandZRotation;

    void Update()
    {
        if (isGrabbed && handTransform != null)
        {
            // Calculate the relative Y rotation difference
            float currentHandZRotation = handTransform.rotation.eulerAngles.z;
            float rotationDifference = currentHandZRotation - initialHandZRotation;


            //Apply limits
            float currentXRotation = transform.rotation.eulerAngles.x;
            float endRotation = currentXRotation + rotationDifference;
            if (endRotation < 270 && endRotation > 90)
            {
                rotationDifference = 0;
            }            
            transform.Rotate(new Vector3(rotationDifference, 0, 0));

            //Update initial hand rotation
            initialHandZRotation = handTransform.rotation.eulerAngles.z;
        }
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        // Set the hand's transform when the object is grabbed
        handTransform = args.interactorObject.transform;
        isGrabbed = true;

        // Store the initial rotations
        initialHandZRotation = handTransform.rotation.eulerAngles.z;
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        // Clear the hand's transform when the object is released
        handTransform = null;
        isGrabbed = false;
    }
}