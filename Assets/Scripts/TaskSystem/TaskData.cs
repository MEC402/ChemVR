using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskData
{
    public TaskState state;
    public int taskStepIndex;
    public TaskStepState[] taskStepStates;

    public TaskData(TaskState state, int taskStepIndex, TaskStepState[] taskStepStates)
    {
        this.state = state;
        this.taskStepIndex = taskStepIndex;
        this.taskStepStates = taskStepStates;
    }
}
