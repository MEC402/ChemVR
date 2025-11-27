using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskInfoSO", menuName = "ScriptableObjects/TaskInfoSO", order = 1)]
public class TaskInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;

    [Header("Requirements")]
    public int experimentRequirement;   // optional: use to require player to be within certain experiment to start task
    public int pointRequirement;        // optional: use to require player to be at a certain number of progress points
    public TaskInfoSO[] taskPrerequisites;

    [Header("Steps")]
    public GameObject[] taskStepPrefabs;

    [Header("Rewards")]
    public int pointsReward; // optional: award progress points to track completion or unlock stuff

    // ensure the id is always the name of the Scriptable Object asset
    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
