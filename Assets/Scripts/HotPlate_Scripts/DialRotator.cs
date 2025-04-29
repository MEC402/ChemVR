using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

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
    public  TextMeshProUGUI HotPlateLabel;
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
            CheckRotationValue(rawZ); //call a method of switch states to change the TPM text to a number between 1 - 10
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
            CheckRotationValue(rawZ); //call a method of switch states to change the TPM text to a number between 1 - 10
        }
        else
        {
            Debug.LogWarning("HotPlate ref not set. Assign in inspector?");
        }

    }

    private void CheckRotationValue(float z)
    {
        Debug.Log("Z: " + z);

        switch (z)
        {
            case float n when (n >= 0 && n < 40):
                //value of 0
                UpdateText(0);
                Debug.Log("hotplate at level 0");
                break;

            case float n when (n >= 40 && n < 70):
                //value of 1
                UpdateText(1);
                break;
            case float n when (n >= 70 && n < 110):
                //value of 2
                UpdateText(2);
                break;
            case float n when (n >= 110 && n < 130):
                //value of 3
                UpdateText(3);
                break;
            case float n when (n >= 130 && n < 160):
                //value of 4
                UpdateText(4);
                break;
            case float n when (n >= 160 && n < 190):
                //value of 5
                UpdateText(5);
                break;
            case float n when (n >= 190 && n < 220):
                //value of 6
                UpdateText(6);
                break;
            case float n when (n >= 220 && n < 250):
                //value of 7
                UpdateText(7);
                break;
            case float n when (n >= 250 && n < 280):
                //value of 8
                UpdateText(8);
                break;
            case float n when (n >= 280 && n < 310):
                //value of 9
                UpdateText(9);
                break;
            case float n when (n >= 310 && n < 360):
                //value of 10
                UpdateText(10);
                break;
        }
    }

    private void UpdateText(int n)
    {
        string nString = n.ToString();
        HotPlateLabel.text = nString;
        Debug.Log("set number pannel to value!");
    }

}
