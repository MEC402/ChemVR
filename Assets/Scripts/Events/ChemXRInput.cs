using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChemXRInput : MonoBehaviour
{
    public enum Button
    {
        A,
        B,
        X,
        Y,
        LGrip,
        RGrip,
        LTrigger,
        RTrigger,
        LThumbstickClick,
        RThumbstickClick,
        Pause
    }

    private static ChemXRInput instance;
    private InputActionMap ChemXRActionMap;

    // On startup, we initialize a static instance of this class, for internal use only.
    // Then we define a new InputActionMap that contains all the standard buttons on a
    // Generic XR Controller. This also produces derived bindings that will associate the
    // actions with the appropriate buttons on Oculus Touch controllers.
    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one ChemXRInput in the scene");
        }
        instance = this;

        ChemXRActionMap = new InputActionMap();

        // RIGHT HAND BUTTONS
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.A),
            InputActionType.Button,   
            binding: "<XRController>{RightHand}/primaryButton"
        );
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.B),
            InputActionType.Button,
            binding: "<XRController>{RightHand}/secondaryButton"
        );
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.RGrip),
            InputActionType.Button,
            binding: "<XRController>{RightHand}/gripPressed"
        );
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.RTrigger),
            InputActionType.Button,
            binding: "<XRController>{RightHand}/triggerPressed"
        );
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.RThumbstickClick),
            InputActionType.Button,
            binding: "<XRController>{RightHand}/thumbstickClicked"
        );

        // LEFT HAND BUTTONS
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.X),
            InputActionType.Button,
            binding: "<XRController>{LeftHand}/primaryButton"
        );
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.Y),
            InputActionType.Button,
            binding: "<XRController>{LeftHand}/secondaryButton"
        );
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.LGrip),
            InputActionType.Button,
            binding: "<XRController>{LeftHand}/gripPressed"
        );
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.LTrigger),
            InputActionType.Button,
            binding: "<XRController>{LeftHand}/triggerPressed"
        );
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.LThumbstickClick),
            InputActionType.Button,
            binding: "<XRController>{LeftHand}/thumbstickClicked"
        );
        // For this action to work in the headset, it is "<XRController>{LeftHand}/start"
        // For this action to work with in the Unity Editor, it is "<XRController>{LeftHand}/menuButton"
#if UNITY_EDITOR
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.Pause),
            InputActionType.Button,
            binding: "<XRController>{LeftHand}/menuButton"
        );
#else
        ChemXRActionMap.AddAction(
            ButtonToActionName(ChemXRInput.Button.Pause),
            InputActionType.Button,
            binding: "<XRController>{LeftHand}/start"
        );
#endif
    }

    // Enables the InputActionMap defined in the Awake function.
    // Adds callback function to each action, which will run whenever the action is performed.
    private void OnEnable()
    {
        ChemXRActionMap.Enable();
        foreach (InputAction action in ChemXRActionMap)
        {
            action.performed += ActionPerformed;
            action.canceled += ActionCancelled;
        }        
    }

    // Removes the callback function from each action, then disables the InputActionMap.
    private void OnDisable()
    {
        foreach (InputAction action in ChemXRActionMap)
        {
            action.performed -= ActionPerformed;
        }
        ChemXRActionMap.Disable();
    }

    // Used as callback function for our input actions.
    // `context` is an InputAction.CallbackContext struct that contains information about the
    // action that was performed.
    // Based on the name of the action, we broadcast the appropriate InputEvent from GameEventsManager.
    private void ActionPerformed(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "X Button":
                GameEventsManager.instance.inputEvents.XButtonPressed(context);
                break;
            case "Y Button":
                GameEventsManager.instance.inputEvents.YButtonPressed(context);
                break;
            case "A Button":
                GameEventsManager.instance.inputEvents.AButtonPressed(context);
                break;
            case "B Button":
                GameEventsManager.instance.inputEvents.BButtonPressed(context);
                break;
            case "L Trigger":
                GameEventsManager.instance.inputEvents.LTriggerPressed(context);
                break;
            case "R Trigger":
                GameEventsManager.instance.inputEvents.RTriggerPressed(context);
                break;
            case "L Grip":
                GameEventsManager.instance.inputEvents.LGripPressed(context);
                break;
            case "R Grip":
                GameEventsManager.instance.inputEvents.RGripPressed(context);
                break;
            case "L Thumbstick Click":
                GameEventsManager.instance.inputEvents.LThumbstickClicked(context);
                break;
            case "R Thumbstick Click":
                GameEventsManager.instance.inputEvents.RThumbstickClicked(context);
                break;
            case "Pause":
                GameEventsManager.instance.inputEvents.PauseButtonPressed(context);
                break;
        }
    }

    private void ActionCancelled(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "X Button":
                GameEventsManager.instance.inputEvents.XButtonReleased(context);
                break;
            case "Y Button":
                GameEventsManager.instance.inputEvents.YButtonReleased(context);
                break;
            case "A Button":
                GameEventsManager.instance.inputEvents.AButtonReleased(context);
                break;
            case "B Button":
                GameEventsManager.instance.inputEvents.BButtonReleased(context);
                break;
            case "L Trigger":
                GameEventsManager.instance.inputEvents.LTriggerReleased(context);
                break;
            case "R Trigger":
                GameEventsManager.instance.inputEvents.RTriggerReleased(context);
                break;
            case "L Grip":
                GameEventsManager.instance.inputEvents.LGripReleased(context);
                break;
            case "R Grip":
                GameEventsManager.instance.inputEvents.RGripReleased(context);
                break;
            case "L Thumbstick Click":
                GameEventsManager.instance.inputEvents.LThumbstickReleased(context);
                break;
            case "R Thumbstick Click":
                GameEventsManager.instance.inputEvents.RThumbstickReleased(context);
                break;
            case "Pause":
                GameEventsManager.instance.inputEvents.PauseButtonReleased(context);
                break;
        }
    }

    // Converts a ChemXRInput.Button enum value into a string.
    // Used to ensure consistency in naming actions and referencing them when checking input state
    // or when creating a new InputAction to add to the InputActionMap.
    private static string ButtonToActionName(ChemXRInput.Button button)
    {
        switch (button)
        {
            case Button.A:
                return "A Button";
            case Button.B:
                return "B Button";
            case Button.X:
                return "X Button";
            case Button.Y:
                return "Y Button";
            case Button.LGrip:
                return "L Grip";
            case Button.RGrip:
                return "R Grip";
            case Button.LTrigger:
                return "L Trigger";
            case Button.RTrigger:
                return "R Trigger";
            case Button.LThumbstickClick:
                return "L Thumbstick Click";
            case Button.RThumbstickClick:
                return "R Thumbstick Click";
            case Button.Pause:
                return "Pause";
            default:
                return null;
        }
    }

    /// <summary>
    /// Polls whether or not a button was pressed this frame. Valid buttons are defined in ChemXRInput class.
    /// Used as an alternative to event-based input reading. 
    /// </summary>
    /// <remarks>
    /// Events are still the preferred method for responding to input, but sometimes polling is simpler.
    /// Please note that holding a button only counts as a single press, so this would return true on the
    /// frame the button is first pressed but would return false on future frames even if the button is still
    /// held down. So use only for responding to discrete button presses and not continuous holding of an input.
    ///
    /// USAGE EXAMPLE ----------------------------------------------------------------------------------------
    /// The following code would make a GameObject execute some behavior when the left thumbstick is clicked.
    ///
    ///      void Update() {
    ///          if (ChemXRInput.Get(ChemXRInput.Button.LThumbstickClick)) {
    ///              // ...do something...
    ///          }
    ///      }
    /// ------------------------------------------------------------------------------------------------------
    /// </remarks>
    /// <param name="button">The ChemXRInput.Button to read input of</param>
    /// <returns>True if the button was pressed this frame, and false otherwise.</returns>    
    public static bool Get(ChemXRInput.Button button)
    {
        return instance.ChemXRActionMap.FindAction(ButtonToActionName(button)).WasPerformedThisFrame();
    }
}
