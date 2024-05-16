using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEvents 
{
    public event Action<InputAction.CallbackContext> onXButtonPressed;
    public void XButtonPressed(InputAction.CallbackContext context)
    {
        if (onXButtonPressed != null)
        {
            onXButtonPressed(context);
        }        
    }

    public event Action<InputAction.CallbackContext> onYButtonPressed;
    public void YButtonPressed(InputAction.CallbackContext context)
    {
        if (onYButtonPressed != null)
        {
            onYButtonPressed(context);
        }
    }

    public event Action<InputAction.CallbackContext> onAButtonPressed;
    public void AButtonPressed(InputAction.CallbackContext context)
    {
        if (onAButtonPressed != null)
        {
            onAButtonPressed(context);
        }
    }

    public event Action<InputAction.CallbackContext> onBButtonPressed;
    public void BButtonPressed(InputAction.CallbackContext context)
    {
        if (onBButtonPressed != null)
        {
            onBButtonPressed(context);
        }
    }

    public event Action<InputAction.CallbackContext> onLGripPressed;
    public void LGripPressed(InputAction.CallbackContext context)
    {
        if (onLGripPressed != null)
        {
            onLGripPressed(context);
        }
    }

    public event Action<InputAction.CallbackContext> onRGripPressed;
    public void RGripPressed(InputAction.CallbackContext context)
    {
        if (onRGripPressed != null)
        {
            onRGripPressed(context);
        }
    }

    public event Action<InputAction.CallbackContext> onLTriggerPressed;
    public void LTriggerPressed(InputAction.CallbackContext context)
    {
        if (onLTriggerPressed != null)
        {
            onLTriggerPressed(context);
        }
    }

    public event Action<InputAction.CallbackContext> onRTriggerPressed;
    public void RTriggerPressed(InputAction.CallbackContext context)
    {
        if (onRTriggerPressed != null)
        {
            onRTriggerPressed(context);
        }
    }

    public event Action<InputAction.CallbackContext> onLThumbstickClicked;
    public void LThumbstickClicked(InputAction.CallbackContext context)
    {
        if (onLThumbstickClicked != null)
        {
            onLThumbstickClicked(context);
        }
    }

    public event Action<InputAction.CallbackContext> onRThumbstickClicked;
    public void RThumbstickClicked(InputAction.CallbackContext context)
    {
        if (onRThumbstickClicked != null)
        {
            onRThumbstickClicked(context);
        }
    }
}
