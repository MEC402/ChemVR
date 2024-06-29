using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskEvents
{
    public event Action<string> onStartTask;
    public void StartTask(string id)
    {
        if (onStartTask != null)
        {
            onStartTask(id);
        }
    }

    public event Action<string> onAdvanceTask;
    public void AdvanceTask(string id)
    {
        if (onAdvanceTask != null) 
        {
            onAdvanceTask(id);
        }
    }

    public event Action<string> onAbandonTask;
    public void AbandonTask(string id)
    {
        if (onAbandonTask != null)
        {
            onAbandonTask(id);
        }
    }

    public event Action<string> onFinishTask;
    public void FinishTask(string id)
    {
        if (onFinishTask != null)
        {
            onFinishTask(id);
        }
    }

    public event Action<Task> onTaskStateChange;
    public void TaskStateChange(Task task)
    {
        if (onTaskStateChange != null)
        {
            onTaskStateChange(task);
        }
    }

    public event Action<string, int, TaskStepState> onTaskStepStateChange;
    public void TaskStepStateChange(string id, int stepIndex, TaskStepState taskStepState)
    {
        if (onTaskStepStateChange != null)
        {
            onTaskStepStateChange(id, stepIndex, taskStepState);
        }
    }
}
