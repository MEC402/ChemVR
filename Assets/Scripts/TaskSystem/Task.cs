using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    // static info
    public TaskInfoSO info;

    // state info
    public TaskState state;
    private int currentTaskStepIndex;
    private TaskStepState[] taskStepStates;

    public Task(TaskInfoSO taskInfo)
    {
        this.info = taskInfo;
        this.state = TaskState.REQUIREMENTS_NOT_MET;
        this.currentTaskStepIndex = 0;
        this.taskStepStates = new TaskStepState[info.taskStepPrefabs.Length];
        for (int i = 0; i < taskStepStates.Length; i++)
        {
            taskStepStates[i] = new TaskStepState();
        }
    }

    public Task(TaskInfoSO taskInfo, TaskState taskState, int currentTaskStepIndex, TaskStepState[] taskStepStates)
    {
        this.info = taskInfo;
        this.state = taskState;
        this.currentTaskStepIndex = currentTaskStepIndex;
        this.taskStepStates = taskStepStates;

        // if the task step states and prefabs are different lengths,
        // something has changed during development and the saved data is out of sync.
        if (this.taskStepStates.Length != this.info.taskStepPrefabs.Length)
        {
            Debug.LogWarning("Task Step Prefabs and Task Step States are of different lengths. " +
                "This indicates something changed with the TaskInfo and the saved data is now " +
                "out of sync. Reset your data or this might cause issues. TaskId: " + this.info.id);
        }
    }

    public void StartOver()
    {
        if (this.currentTaskStepIndex > 0)
        {
            if(CurrentStepExists())
            {
                string destroy = GetCurrentTaskStepPrefab().GetComponent<TaskStep>().ToString();
                if (destroy.Contains("("))
                {
                    //Replace duplicate task name with clone
                    int start = destroy.IndexOf("(") + 1;
                    int end = destroy.IndexOf(")");
                    destroy = destroy.Remove(start, (end - start));
                    destroy = destroy.Insert(start, "Clone");

                    // Remove space before (Clone)
                    destroy = destroy.Remove(start - 2, 1);
                }
                Object.Destroy(GameObject.Find(destroy));
            }                      
        }
        this.state = TaskState.CAN_START;

        currentTaskStepIndex = 0;
    }

    public int getStep()
    {
        return currentTaskStepIndex;
    }
    public void MoveToNextStep()
    {
        currentTaskStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentTaskStepIndex < info.taskStepPrefabs.Length);
    }

    public void InstantiateCurrentTaskStep(Transform parentTransform)
    {
        GameObject taskStepPrefab = GetCurrentTaskStepPrefab();
        if (taskStepPrefab != null)
        {
            TaskStep taskStep = Object.Instantiate<GameObject>(taskStepPrefab, parentTransform).GetComponent<TaskStep>();
            taskStep.InitializeTaskStep(info.id, currentTaskStepIndex, taskStepStates[currentTaskStepIndex].state);
        }
    }

    private GameObject GetCurrentTaskStepPrefab()
    {
        GameObject taskStepPrefab = null;
        if (CurrentStepExists())
        {
            taskStepPrefab = info.taskStepPrefabs[currentTaskStepIndex];
        }
        else
        {
            Debug.LogError("Tried to get task step prefab, but stepIndex was out of range indicating that " +
                "there's no current step: TaskId=" + info.id + ", stepIndex=" + currentTaskStepIndex);
        }
        return taskStepPrefab;
    }

    public void StoreTaskStepState(TaskStepState taskStepState, int stepIndex)
    {
        if (stepIndex < taskStepStates.Length)
        {
            taskStepStates[stepIndex].state = taskStepState.state;
            taskStepStates[stepIndex].status = taskStepState.status;
        }
        else
        {
            Debug.LogWarning("Tried to access task step data, but stepIndex was out of range: " +
                "Quest Id = " + info.id + ", Step Index = " + stepIndex);
        }
    }

    public TaskData GetTaskData()
    {
        return new TaskData(state, currentTaskStepIndex, taskStepStates);
    }

    public string GetFullStatusText()
    {
        string fullStatus = "";

        if (state == TaskState.REQUIREMENTS_NOT_MET)
        {
            fullStatus = "Requirements are not yet met to start this task.";
        }
        else if (state == TaskState.CAN_START)
        {
            fullStatus = "This task can be started.";
        } 
        else
        {
            // display all previous steps with strikethroughs
            for (int i = 0; i < currentTaskStepIndex; i++)
            {
                fullStatus += "<s>" + taskStepStates[i].status + "</s>\n";
            }
            
            //display the current step if it exists
            if (CurrentStepExists())
            {
                fullStatus += taskStepStates[currentTaskStepIndex].status;
            }

            // when the task is completed or turned in
            if (state == TaskState.CAN_FINISH)
            {
                fullStatus += "The task is ready to be turned in.";
            }
            else if (state == TaskState.FINISHED)
            {
                fullStatus += "The task has been completed.";
            }
        }

        return fullStatus;
    }
}
