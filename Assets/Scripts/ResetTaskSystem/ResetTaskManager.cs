using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTaskManager : MonoBehaviour
{

    public static ResetTaskManager instance { get; private set; }
    private TaskManager taskManager;
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

        //Find list of all tracked objects in the module to reference later.
        TrackedObject[] allTrackedObjects = FindObjectsByType<TrackedObject>(FindObjectsSortMode.None);
        foreach (TrackedObject objectToTrack in allTrackedObjects)
        {
            trackedObjects.Add(objectToTrack.gameObject);
        }
    }

    //Placeholder Update, until I can properly bind resetting task to more UI involved stuff and XR inputs. Just for testing.
    private void Update()
    {   
        if (Input.GetKeyDown(KeyCode.K))
        {
            ResetCurrentTask();
        }
    }

    public void ResetCurrentTask()
    {
        //First, get the current task step of the module.
        int currentTaskNumber = taskManager.GetCurrentStepIndex(taskInfoObjectID);
        //Then, get the corresponding checkpoint to that step.
        TaskCheckpointSO currentCheckpoint = CheckpointContainer.GetCheckpoint(currentTaskNumber);
        //Then, get all the saved object states within that checkpoint
        List<ObjectState> statesToRestore = new List<ObjectState>();
        foreach(ObjectState storedState in currentCheckpoint.objectStates)
        {
            statesToRestore.Add(storedState);
            Debug.Log("Added state for " + storedState.objectName);
        }
        //Finally, sort through all states and all tracked objects and match up their data.
        foreach(GameObject trackedObject in trackedObjects)
        {
            Debug.Log("Object to reset: " + trackedObject.gameObject.name);
            foreach(ObjectState resetState in statesToRestore)
            {
                if(resetState.objectName == trackedObject.gameObject.name)
                {
                    //Stored position data
                    trackedObject.transform.position = resetState.position;
                    trackedObject.transform.rotation = resetState.rotation;
                    //Stored velocity data (by all means this should ALWAYS be 0, just forcing all the objects into resting positions upon reset)
                    Rigidbody rb = trackedObject.GetComponent<Rigidbody>();
                    rb.velocity = resetState.velocity;
                    rb.angularVelocity = resetState.angularVelocity;

                    //If object being reset happens to be chem container, continue, also still do TryGetComponent anyway cause we don't want errors, just in case.
                    if(resetState.isChemContainer)
                    {
                        if (trackedObject.TryGetComponent<ChemContainer>(out ChemContainer chemContainer))
                        {
                            chemContainer.SetChem(resetState.currentFluid);
                            chemContainer.UpdateChem();
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






}
