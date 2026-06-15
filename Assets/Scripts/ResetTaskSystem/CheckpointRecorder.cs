using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CheckpointRecorder : MonoBehaviour
{
    private List<GameObject> trackedObjects = new();
    private string objectSavePath = "Assets/Scripts/ResetTaskSystem/CheckpointObjects/";
    [HeaderAttribute("Write the EXACT name of the end folder to save the objects to, under 'ResetTaskSystem/CheckpointObjects/'")]
    [Space]
    [SerializeField] private string ModuleFolderName;
    private string path;

    [SerializeField] private string sceneName;
    [SerializeField] private int taskStepIndex = 0;

    public TaskCheckpointSO checkpoint;
    // Start is called before the first frame update
    private void Start()
    {
        TrackedObject[] interactables = FindObjectsByType<TrackedObject>(FindObjectsSortMode.None);

        foreach (TrackedObject interactable in interactables)
        {
            trackedObjects.Add(interactable.gameObject);
        }
        Debug.Log(trackedObjects.Count);

        //Assemble path to save objects to (only did this to make it easier to change in the inspector)
        path = objectSavePath + ModuleFolderName + "/";
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            CaptureCheckpoint();
        }
    }

    public void CaptureCheckpoint()
    {
        taskStepIndex++;
        TaskCheckpointSO checkpoint = ScriptableObject.CreateInstance<TaskCheckpointSO>();
        checkpoint.checkpointStep = taskStepIndex;
        checkpoint.checkpointName = sceneName + "Checkpoint" + "-" + taskStepIndex;

        TrackedObject[] objectsToCapture = FindObjectsByType<TrackedObject>(FindObjectsSortMode.None);

        foreach (TrackedObject capturedObject in objectsToCapture)
        {
            Rigidbody rb = capturedObject.gameObject.GetComponent<Rigidbody>();

            if (capturedObject.TryGetComponent<ChemContainer>(out ChemContainer capturedContainer))
            {
                ChemFluid fluidToCapture = capturedContainer.GetChemFluid();

                ObjectState capturedState = new ObjectState
                {
                    objectName = capturedObject.gameObject.name,
                    position = capturedObject.transform.position,
                    rotation = capturedObject.transform.rotation,

                    velocity = rb ? rb.velocity : Vector3.zero,
                    angularVelocity = rb ? rb.angularVelocity : Vector3.zero,

                    isChemContainer = true,
                    currentFluid = fluidToCapture 
                };

                checkpoint.objectStates.Add(capturedState);
            }
            else
            {
                ObjectState capturedState = new ObjectState
                {
                    objectName = capturedObject.gameObject.name,
                    position = capturedObject.transform.position,
                    rotation = capturedObject.transform.rotation,

                    velocity = rb ? rb.velocity : Vector3.zero,
                    angularVelocity = rb ? rb.angularVelocity : Vector3.zero,

                    isChemContainer = false
                };

                checkpoint.objectStates.Add(capturedState);

            }

        }

        foreach (ObjectState state in checkpoint.objectStates)
        {
            Debug.Log("Object name: " + state.objectName);
        }
        Debug.Log(checkpoint.sceneForCheckpoint);
        Debug.Log(checkpoint.checkpointStep);

#if UNITY_EDITOR
        AssetDatabase.CreateAsset(checkpoint, path + checkpoint.checkpointName + ".asset");
        AssetDatabase.SaveAssets();
#endif

    }


}
