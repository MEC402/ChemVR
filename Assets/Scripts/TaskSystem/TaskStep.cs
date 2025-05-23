using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class TaskStep : MonoBehaviour
{
    private bool isFinished = false;
    private string taskId;
    private int stepIndex;

    protected void Start()
    {
        GameEventsManager.instance.inputEvents.onBButtonPressed += DevSkipTask;
    }

    protected void OnDestroy()
    {
        GameEventsManager.instance.inputEvents.onBButtonPressed -= DevSkipTask;
    }

    protected void DevSkipTask(InputAction.CallbackContext obj)
    {
        if (DevOpsState.IsDevOpsEnabled())
        {
            FinishTaskStep();
        }
    }

    public void InitializeTaskStep(string taskId, int stepIndex, string taskStepState)
    {
        this.taskId = taskId;
        this.stepIndex = stepIndex;
        if (taskStepState != null && taskStepState != "")
        {
            SetTaskStepState(taskStepState);
        }
    }

    protected void FinishTaskStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            GameEventsManager.instance.taskEvents.AdvanceTask(taskId);
            Destroy(this.gameObject);
        }
    }

    protected void FinishTaskStepWithDelay(float minDelay, float maxDelay)
    {
        if (!isFinished)
        {
            isFinished = true;
            StartCoroutine(DelayedFinish(minDelay, maxDelay));
        }
    }

    IEnumerator DelayedFinish(float minDelay, float maxDelay)
    {
        float delay = UnityEngine.Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);
        GameEventsManager.instance.taskEvents.AdvanceTask(taskId);
        Destroy(this.gameObject);
    }

    protected void ChangeState(string newState, string newStatus)
    {
        GameEventsManager.instance.taskEvents.TaskStepStateChange(
            taskId,
            stepIndex,
            new TaskStepState(newState, newStatus)
        );
    }

    protected abstract void SetTaskStepState(string state);
}
