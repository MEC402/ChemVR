using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DialRotator : MonoBehaviour
{
    [SerializeField] Transform linkedDial;
    [SerializeField] private int snapRotationAmount = 25;
    [SerializeField] private float angleTolerance;
    [SerializeField] private GameObject Righthandmodel;
    [SerializeField] private GameObject LefthandModel;
    [SerializeField] bool shouldUseDummyHands;
    [SerializeField] private Heat_Controller hotplate;

    private XRBaseInteractor interactor;
    private float startAngle;
    private bool requiresStartAngle = true;
    private bool shouldGetHandRotation = false;
    //    private float 

    private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

    private void OnEnable()
    {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabEnd);
    }
    private void OnDisable()
    {
        grabInteractor.selectEntered.RemoveListener(GrabbedBy);
        grabInteractor.selectExited.RemoveListener(GrabEnd);
    }

    private void GrabEnd(SelectExitEventArgs arg0)
    {
        shouldGetHandRotation = false;
        requiresStartAngle = true;
        HandModelVisibility(false);
    }

    private void GrabbedBy(SelectEnterEventArgs arg0)
    {
        //interactor = GetComponent<XRGrabInteractable>().interactorsSelecting;
        interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
        interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;

        shouldGetHandRotation = true;
        startAngle = 0f;
        HandModelVisibility(true);
    }

    private void HandModelVisibility(bool visibilityState)
    {
        if (!shouldUseDummyHands) return;
        if (interactor.CompareTag("RightHand"))
        {
            Righthandmodel.SetActive(visibilityState);
        }
        else
        {
            LefthandModel.SetActive(visibilityState);
        }
    }

    private void Update()
    {
        if (shouldGetHandRotation)
        {
            var rotationAngle = GetInteractorRotation();
            GetRotationDistance(rotationAngle);
        }
    }
    public float GetInteractorRotation() => interactor.GetComponent<Transform>().eulerAngles.z;

    private void GetRotationDistance(float currentAngle)
    {
        if (!requiresStartAngle)
        {
            var angleDifference = Mathf.Abs(startAngle - currentAngle);

            if (angleDifference > angleTolerance)
            {
                if (angleDifference > 270f)
                {
                    float angleCheck;

                    if (startAngle < currentAngle)
                    {
                        angleCheck = CheckAngle(currentAngle, startAngle);

                        if (angleCheck < angleTolerance)
                            return;
                        else
                        {
                            RotateDialClockwise();
                            startAngle = currentAngle;
                        }
                    }

                    else if (startAngle > currentAngle)
                    {
                        angleCheck = CheckAngle(currentAngle, startAngle);

                        if (angleCheck < angleTolerance)
                            return;

                        else
                        {
                            RotateDialAntiClockwise();
                            startAngle = currentAngle;
                        }
                    }
                }

                else
                {
                    if (startAngle < currentAngle)
                    {
                        RotateDialAntiClockwise();
                        startAngle = currentAngle;
                    }
                    else if (startAngle > currentAngle)
                    {
                        RotateDialClockwise();
                        startAngle = currentAngle;
                    }
                }
            }
        }
        else
        {
            requiresStartAngle = false;
            startAngle = currentAngle;
        }
    }
    private float CheckAngle(float currentAngle, float startAngle) => (360f - currentAngle) + startAngle;

    private void RotateDialClockwise()
    {
        float rawZ = linkedDial.localEulerAngles.z + snapRotationAmount;
        Debug.Log("Clockwise rawZ: " + rawZ);
        if (rawZ > 360f)
        {
            rawZ -= 360f;
        }

        if (rawZ > 320f)
        {
            return;
        }

        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y, rawZ);

        if (hotplate != null)
        {
            hotplate.DialChanged(rawZ);
            
        }
        else
        {
            Debug.LogWarning("HotPlate ref not set. Assign in inspector?");
        }

    }

    private void RotateDialAntiClockwise()
    {
        float rawZ = linkedDial.localEulerAngles.z - snapRotationAmount;
        Debug.Log("Anticlockwise rawZ " + rawZ);

        if (rawZ < 0f)
        {
            return;
        }

        linkedDial.localEulerAngles = new Vector3(linkedDial.localEulerAngles.x, linkedDial.localEulerAngles.y, rawZ);


        if (hotplate != null)
        {
            hotplate.DialChanged(rawZ);
        }
        else
        {
            Debug.LogWarning("HotPlate ref not set. Assign in inspector?");
        }

    }

}
