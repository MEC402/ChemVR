using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Task Checkpoint")]
public class TaskCheckpointSO : ScriptableObject
{
    public string sceneForCheckpoint;
    public int checkpointStep;
    public string checkpointName;

    public List<ObjectState> objectStates = new List<ObjectState>();
}
