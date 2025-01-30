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

    public event Action<InputAction.CallbackContext> onXButtonReleased;
    public void XButtonReleased(InputAction.CallbackContext context)
    {
        if (onXButtonReleased != null)
        {
            onXButtonReleased(context);
        }
    }

    public event Action<InputAction.CallbackContext> onYButtonReleased;
    public void YButtonReleased(InputAction.CallbackContext context)
    {
        if (onYButtonReleased != null)
        {
            onYButtonReleased(context);
        }
    }

    public event Action<InputAction.CallbackContext> onAButtonReleased;
    public void AButtonReleased(InputAction.CallbackContext context)
    {
        if (onAButtonReleased != null)
        {
            onAButtonReleased(context);
        }
    }

    public event Action<InputAction.CallbackContext> onBButtonReleased;
    public void BButtonReleased(InputAction.CallbackContext context)
    {
        if (onBButtonReleased != null)
        {
            onBButtonReleased(context);
        }
    }

    public event Action<InputAction.CallbackContext> onLGripReleased;
    public void LGripReleased(InputAction.CallbackContext context)
    {
        if (onLGripReleased != null)
        {
            onLGripReleased(context);
        }
    }

    public event Action<InputAction.CallbackContext> onRGripReleased;
    public void RGripReleased(InputAction.CallbackContext context)
    {
        if (onRGripReleased != null)
        {
            onRGripReleased(context);
        }
    }

    public event Action<InputAction.CallbackContext> onLTriggerReleased;
    public void LTriggerReleased(InputAction.CallbackContext context)
    {
        if (onLTriggerReleased != null)
        {
            onLTriggerReleased(context);
        }
    }

    public event Action<InputAction.CallbackContext> onRTriggerReleased;
    public void RTriggerReleased(InputAction.CallbackContext context)
    {
        if (onRTriggerReleased != null)
        {
            onRTriggerReleased(context);
        }
    }

    public event Action<InputAction.CallbackContext> onLThumbstickReleased;
    public void LThumbstickReleased(InputAction.CallbackContext context)
    {
        if (onLThumbstickReleased != null)
        {
            onLThumbstickReleased(context);
        }
    }

    public event Action<InputAction.CallbackContext> onRThumbstickReleased;
    public void RThumbstickReleased(InputAction.CallbackContext context)
    {
        if (onRThumbstickReleased != null)
        {
            onRThumbstickReleased(context);
        }
    }

    //Pause featyres
    public event Action<InputAction.CallbackContext> onPauseButtonPressed;
    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (onPauseButtonPressed != null)
        {
            onPauseButtonPressed(context);
        }
    }

    public event Action<InputAction.CallbackContext> onPauseButtonReleased;
    public void PauseButtonReleased(InputAction.CallbackContext context)
    {
        if (onPauseButtonReleased != null)
        {
            onPauseButtonReleased(context);
        }
    }
}
