using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;



public class ToggleText : MonoBehaviour
{
    private GameObject text;
    private InputDevice inputDevice;
    public HandType handType;
    private bool handsAvailable = true;
    private bool isHeld = false;


    void Start()
    {
        inputDevice = GetInputDevice();
        text = this.gameObject.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        // Check if the left thumbstick is clicked on the XR controller
        if (handsAvailable && (handType == HandType.Left))
        {
            inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryPressed);
            if (primaryPressed)
            {
                if (!isHeld) // This prevents flickering when button is held.
                {
                    if (text.activeInHierarchy)
                    {
                        text.SetActive(false);
                    }
                    else
                    {
                        text.SetActive(true);
                    }
                    isHeld = true;
                }
            }
            else
            {
                isHeld = false;
            }
        }
    }

    InputDevice GetInputDevice()
    {
        InputDeviceCharacteristics controllerCharacteristic = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;

        if (handType == HandType.Left)
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Left;
        }
        else
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Right;
        }

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristic, inputDevices);

        if (inputDevices.Count > 0)
        {
            handsAvailable = true;
            return inputDevices[0];
        }
        else
        {
            handsAvailable = false;
            return new InputDevice();
        }

    }

}
