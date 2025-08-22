using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Inspect_Glassware : TaskStep
{
    #region Variables
    List<GlasswareItem> glasswareItems = new List<GlasswareItem>(); // List to hold all glassware items in the scene.

    // Array of glassware names to be inspected. These names should match the names of the GameObjects in the scene. If you add more glassware, add them to this array.
    private readonly string[] glasswareNames =
    {
        "BeakerUp250mL_LargerText"    };



    bool isWebGL = false;
    #endregion

    protected override void SetTaskStepState(string state)
    {
        // Not needed for this task step
    }

    #region Unity Methods
    void OnEnable()
    {
        //This cursed line of code needs fixed it makes the whole script fail if not in WebGl mode.
        //isWebGL = GameObject.Find("Glassware Use").GetComponent<Glassware_Use_Overview>().isWebGL;

        GameEventsManager.instance.inputEvents.onWebGLSkipTask += SkipTask;


        if (isWebGL)
            GameEventsManager.instance.webGLEvents.OnObjectGrabbed += WebGLInspectObject; // Subscribe to the WebGL event for object grabbing.

        foreach (string glasswareName in glasswareNames) // Loop through each glassware name in the array and find the corresponding GameObject in the scene.
        {
            GameObject glasswareObject = GameObject.Find(glasswareName);

            if (glasswareObject.activeSelf == false)
            {
                Debug.LogWarning($"Glassware item '{glasswareName}' is not active in the scene.");
                continue;
            }

            if (glasswareObject != null && glasswareObject.TryGetComponent(out XRGrabInteractable grabInteractable))
            {
                var item = new GlasswareItem(glasswareObject, false, grabInteractable);
                glasswareItems.Add(item);

                // Debug.Log($"Found glassware item: {glasswareName}");

                if (!isWebGL)
                    grabInteractable.selectEntered.AddListener(OnInspect);
            }
            else
                Debug.LogWarning($"Glassware item '{glasswareName}' not found or missing XRGrabInteractable component.");
        }
    }

    void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onWebGLSkipTask -= SkipTask;

        if (!isWebGL)
        {
            foreach (var item in glasswareItems) // Loop through each glassware item in the list and remove the listener from the XRGrabInteractable component.
            {
                if (item.grabInteractable != null)
                    item.grabInteractable.selectEntered.RemoveListener(OnInspect);
            }
        }
        else
            GameEventsManager.instance.webGLEvents.OnObjectGrabbed -= WebGLInspectObject; // Unsubscribe from the WebGL event for object grabbing.
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Handles inspection in VR when the glassware is grabbed.
    /// </summary>
    private void OnInspect(SelectEnterEventArgs arg0) => MarkObjectAsInspected(arg0.interactableObject.transform.gameObject);

    /// <summary>
    /// Manually called for WebGL interactions when the player "grabs" an object.
    /// </summary>
    public void WebGLInspectObject(GameObject grabbedObject) => MarkObjectAsInspected(grabbedObject);

    /// <summary>
    /// Marks the object as inspected and checks if the task is complete.
    /// </summary>
    private void MarkObjectAsInspected(GameObject grabbedObject)
    {
        if (grabbedObject == null)
        {
            Debug.LogError("Attempted to inspect a NULL object!");
            return;
        }

        for (int i = 0; i < glasswareItems.Count; i++)
        {
            if (glasswareItems[i].glassware == grabbedObject && !glasswareItems[i].inspected)
            {
                glasswareItems[i] = new GlasswareItem(glasswareItems[i].glassware, true, glasswareItems[i].grabInteractable);

                if (AllGlasswareInspected())
                    FinishTaskStep();

                break;
            }
        }
    }

    /// <summary>
    /// Checks if all glassware items have been inspected.
    /// </summary>
    /// <returns>True if all glassware items are inspected, otherwise false.</returns>
    private bool AllGlasswareInspected()
    {
        foreach (var item in glasswareItems)
        {
            if (!item.inspected)
                return false;
        }

        return true;
    }

    private void SkipTask(InputAction.CallbackContext obj)
    {
        FinishTaskStep();
    }
    #endregion
}

#region Structs
/// <summary>
/// Struct to hold information about glassware items in the scene.
/// </summary>
public struct GlasswareItem
{
    public GameObject glassware;
    public bool inspected;
    public XRGrabInteractable grabInteractable;

    public GlasswareItem(GameObject glassware, bool inspected, XRGrabInteractable grabInteractable)
    {
        this.glassware = glassware;
        this.inspected = inspected;
        this.grabInteractable = grabInteractable;
    }
}
#endregion
