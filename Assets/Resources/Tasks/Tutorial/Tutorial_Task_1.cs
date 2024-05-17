using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Tutorial_Task_1 : TaskStep
{
    private int buttonPress = 0;
    [SerializeField]
    public GameObject currText; // Set this to the popup text for this step.
    private InputDevice inputDevice;
    public HandType handType;
    private bool handsAvailable = true;
    private bool isHeld = false;

    private void Awake()
    {
        currText.SetActive(true);
        inputDevice = GetInputDevice();

    }

    private void Update()
    {
        // Check if the left thumbstick is clicked on the XR controller
        if (handsAvailable && (handType == HandType.Left))
        {
            inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryPressed);
            if (primaryPressed)
            {
                if (!isHeld) // This prevents spamming the press when button is held.
                {
                    buttonPress += 1;
                    isHeld = true;
                }
            }
            else
            {
                isHeld = false;
            }
        }

        if (buttonPress >= 2)
        {
            // Hide associated text
            currText.SetActive(false);
            FinishTaskStep();
        }
    }
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
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
