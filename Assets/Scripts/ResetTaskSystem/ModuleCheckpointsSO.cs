using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Module Checkpoint Container")]
public class ModuleCheckpointsSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [SerializeField] TaskCheckpointSO[] ModuleCheckpoints;

    public TaskCheckpointSO GetCheckpoint(int checkpointIndex)
    {
        return ModuleCheckpoints[checkpointIndex];
    }

    
}
