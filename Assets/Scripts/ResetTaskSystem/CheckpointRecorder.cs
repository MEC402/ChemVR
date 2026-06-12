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

    [SerializeField] private string sceneName;
    [SerializeField] private int taskStepIndex = 0;

    public TaskCheckpoint checkpoint;
    // Start is called before the first frame update
    private void Start()
    {
        TrackedObject[] interactables = FindObjectsByType<TrackedObject>(FindObjectsSortMode.None);

        foreach (TrackedObject interactable in interactables)
        {
            trackedObjects.Add(interactable.gameObject);
        }
        Debug.Log(trackedObjects.Count);
 
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
        if (checkpoint != null)
            checkpoint.objectStates.Clear();
        else
        {
            checkpoint = ScriptableObject.CreateInstance<TaskCheckpoint>();
            checkpoint.sceneForCheckpoint = sceneName;
        }
        taskStepIndex++;
        checkpoint.checkpointStep = taskStepIndex;
        checkpoint.checkpointName = sceneName + "Checkpoint" + "-" + taskStepIndex;

        TrackedObject[] objectsToCapture = FindObjectsByType<TrackedObject>(FindObjectsSortMode.None);

        foreach (TrackedObject capturedObject in objectsToCapture)
        {
            Rigidbody rb = capturedObject.gameObject.GetComponent<Rigidbody>();

            ObjectState capturedState = new ObjectState
            {
                objectName = capturedObject.gameObject.name,
                position = capturedObject.transform.position,
                rotation = capturedObject.transform.rotation,

                velocity = rb ? rb.velocity : Vector3.zero,
                angularVelocity = rb ? rb.angularVelocity : Vector3.zero
            };

            checkpoint.objectStates.Add(capturedState);
        }

        foreach(ObjectState state in checkpoint.objectStates)
        {
            Debug.Log("Object name: " + state.objectName);
        }
        Debug.Log(checkpoint.sceneForCheckpoint);
        Debug.Log(checkpoint.checkpointStep);

        #if UNITY_EDITOR
        AssetDatabase.CreateAsset(checkpoint, "Assets/Scripts/ResetTaskSystem/CheckpointObjects/GlasswareUseVR/" + checkpoint.checkpointName + ".asset");
        AssetDatabase.SaveAssets();
        #endif

    }


}
