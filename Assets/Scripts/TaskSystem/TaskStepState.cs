using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskStepState
{
    public string state;
    public string status;

    public TaskStepState(string state, string status)
    {
        this.state = state;
        this.status = status;
    }

    public TaskStepState()
    {
        this.state = "";
        this.status = "";
    }
}
