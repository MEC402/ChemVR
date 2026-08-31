using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ResetTaskManager : MonoBehaviour
{

    public static ResetTaskManager instance { get; private set; }

    public event EventHandler onResetCalled;
    private TaskManager taskManager;
    private ResetTaskMenuComponent resetTaskMenuUI;
    private bool resetUiOpen;
    private List<GameObject> trackedObjects = new();

    [HeaderAttribute("Write the EXACT ID that is written on the TaskInfoSO object for the given module.")]
    [Space]
    [SerializeField] private string taskInfoObjectID; //Like the header says, we need the exact ID in order to get the task index from the task manager.
    [Space]
    [SerializeField] private ModuleCheckpointsSO CheckpointContainer; //The Scriptable Object with all the Checkpoints inside of it.

    private void Awake()
    {
        //Generic instance setup stuff
        if (instance != null)
        {
            Debug.LogError("Found more than one Task Reset Manager in the scene.");
        }
        instance = this;
    }


    private void Start()
    {
        //Get a reference to the task manager so we can reference the task step index.
        taskManager = FindAnyObjectByType<TaskManager>();
        resetTaskMenuUI = FindAnyObjectByType<ResetTaskMenuComponent>();
        resetTaskMenuUI.gameObject.SetActive(false);

        //Find list of all tracked objects in the module to reference later.
        TrackedObject[] allTrackedObjects = FindObjectsByType<TrackedObject>(FindObjectsSortMode.None);
        foreach (TrackedObject objectToTrack in allTrackedObjects)
        {
            trackedObjects.Add(objectToTrack.gameObject);
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onLThumbstickClicked += BeginResetTask;
        GameEventsManager.instance.inputEvents.onRThumbstickClicked += ConfirmResetTask;

    }
    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onLThumbstickClicked -= BeginResetTask;
        GameEventsManager.instance.inputEvents.onRThumbstickClicked -= ConfirmResetTask;
    }


    //Placeholder Update, works for webGL if cannot bind keys to XR thumbstick.
    /*     private void Update()
       {
           if (Input.GetKeyDown(KeyCode.K))
           {
               ResetCurrentTask();
           }
       }  */

    public void ResetCurrentTask()
    {
        onResetCalled?.Invoke(this, EventArgs.Empty);
        //First, get the current task step of the module.
        int currentTaskNumber = taskManager.GetCurrentStepIndex(taskInfoObjectID);
        //Then, get the corresponding checkpoint to that step.
        TaskCheckpointSO currentCheckpoint = CheckpointContainer.GetCheckpoint(currentTaskNumber);
        //Then, get all the saved object states within that checkpoint
        List<ObjectState> statesToRestore = new List<ObjectState>();
        foreach (ObjectState storedState in currentCheckpoint.objectStates)
        {
            statesToRestore.Add(storedState);
            Debug.Log("Added state for " + storedState.objectName);
        }
        //Finally, sort through all states and all tracked objects and match up their data.
        foreach (GameObject trackedObject in trackedObjects)
        {
            //Debug.Log("Object to reset: " + trackedObject.gameObject.name);
            foreach (ObjectState resetState in statesToRestore)
            {
                if (resetState.objectName == trackedObject.gameObject.name) //For each object and each state, check if object matches state's saved name.
                {
                    //Check if object is ChemContainer FIRST, because we want to reset chems, but not always positions. This would get skipped if done last.
                    if (resetState.isChemContainer)
                    {
                        if (trackedObject.TryGetComponent<ChemContainer>(out ChemContainer chemContainer))
                        {
                            chemContainer.SetChem(resetState.currentFluid);
                            chemContainer.UpdateChem();
                        }
                    }

                    if (trackedObject.gameObject.TryGetComponent<XRGrabInteractable>(out XRGrabInteractable grab)) //Check if Object is currently being held, if so, don't reset the position.
                    {
                        if (grab != null && grab.isSelected)
                        {
                            continue;
                        } //Do nothing. If the object is held, we don't want to try and move it out of the user's hands.
                        else
                        {
                            if (trackedObject.gameObject.TryGetComponent<SnapBulbToPipet>(out SnapBulbToPipet bulbScript)) //Make sure object even has the component
                            {
                                if (bulbScript.GetSnap())//Is the bulb currently attached to a pipette?
                                {
                                    //Do nothing. The bulb is snapped to a pipette and will move with it. 
                                    //(This might be a problem, if the bulb starts clipping into stuff again. May need to force bulb to detatch on reset.)   
                                }
                                else
                                {   //If not attached, do all the normal stuff.
                                    //Stored position data
                                    trackedObject.transform.position = resetState.position;
                                    trackedObject.transform.rotation = resetState.rotation;
                                    //Stored velocity data (by all means this should ALWAYS be 0, just forcing all the objects into resting positions upon reset)
                                    Rigidbody rb = trackedObject.GetComponent<Rigidbody>();
                                    rb.velocity = resetState.velocity;
                                    rb.angularVelocity = resetState.angularVelocity;
                                }
                            }
                            else if (trackedObject.gameObject.TryGetComponent<Put_Paper_on_Boat>(out Put_Paper_on_Boat paperScript))
                            {
                                if (paperScript.GetHasSnapped())
                                {
                                    //Again, like the bulbs, do nothing. The object is nested in something else 
                                }
                                else
                                {
                                    //Stored position data
                                    trackedObject.transform.position = resetState.position;
                                    trackedObject.transform.rotation = resetState.rotation;
                                    //Stored velocity data (by all means this should ALWAYS be 0, just forcing all the objects into resting positions upon reset)
                                    Rigidbody rb = trackedObject.GetComponent<Rigidbody>();
                                    rb.velocity = resetState.velocity;
                                    rb.angularVelocity = resetState.angularVelocity;
                                }
                            }
                            else if (trackedObject.TryGetComponent<SnapToTray>(out SnapToTray trayScript))
                            {
                                if (trayScript.GetIsSnapped())
                                {
                                    //Once more, do nothing. Objects that can be attached to the carrying tray don't need to be reset if they're currently attached to the tray.
                                    //In the very rare case that the tray is lost inside the lab with objects on top of it...resetting the tray would bring both back anyway.
                                }
                                else
                                {//Once more, normal reset stuff if not.
                                 //Stored position data
                                    trackedObject.transform.position = resetState.position;
                                    trackedObject.transform.rotation = resetState.rotation;
                                    //Stored velocity data (by all means this should ALWAYS be 0, just forcing all the objects into resting positions upon reset)
                                    Rigidbody rb = trackedObject.GetComponent<Rigidbody>();
                                    rb.velocity = resetState.velocity;
                                    rb.angularVelocity = resetState.angularVelocity;
                                }
                            }
                            else //If none of the above things apply to the given object, just reset.
                            {
                                //Stored position data
                                trackedObject.transform.position = resetState.position;
                                trackedObject.transform.rotation = resetState.rotation;
                                //Stored velocity data (by all means this should ALWAYS be 0, just forcing all the objects into resting positions upon reset)
                                Rigidbody rb = trackedObject.GetComponent<Rigidbody>();
                                rb.velocity = resetState.velocity;
                                rb.angularVelocity = resetState.angularVelocity;
                            }
                        }
                    }
                    //Debug.Log(trackedObject.gameObject.name + " has been reset to last step.");
                }
                else
                    continue;
            }
        }
        //Debug.Log("All objects should be restored!");

    }

    //Might need to add more to this for some sort of UI popup that says "confirm?" or something like that so it can't be done on accident.
    private void BeginResetTask(InputAction.CallbackContext context)
    {
        if (!resetUiOpen)
        {
            resetUiOpen = true;
            resetTaskMenuUI.gameObject.SetActive(true);
        }
        else if (resetUiOpen)
        {
            resetUiOpen = false;
            resetTaskMenuUI.gameObject.SetActive(false);
        }
    }

    private void ConfirmResetTask(InputAction.CallbackContext context)
    {
        if (resetUiOpen)
        {
            ResetCurrentTask();
            resetUiOpen = false;
            resetTaskMenuUI.gameObject.SetActive(false);
        }
    }






}
