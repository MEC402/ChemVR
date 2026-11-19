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
        // WebGL: the N key skips whatever step is active. Subscribed here (not in Start/OnEnable) so
        // subclasses that define their own Start/OnEnable cannot hide it.
        GameEventsManager.instance.inputEvents.onWebGLSkipTask += WebGLSkip;
        if (taskStepState != null && taskStepState != "")
        {
            SetTaskStepState(taskStepState);
        }
    }

    private void WebGLSkip(InputAction.CallbackContext obj)
    {
        if (this == null || isFinished)
        {
            GameEventsManager.instance.inputEvents.onWebGLSkipTask -= WebGLSkip;
            return;
        }
        FinishTaskStep();
    }

    protected void FinishTaskStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            GameEventsManager.instance.inputEvents.onWebGLSkipTask -= WebGLSkip;
            GameEventsManager.instance.taskEvents.AdvanceTask(taskId);
            Destroy(this.gameObject);
        }
    }

    protected void FinishTaskStepWithDelay(float minDelay, float maxDelay)
    {
        if (!isFinished)
        {
            isFinished = true;
            GameEventsManager.instance.inputEvents.onWebGLSkipTask -= WebGLSkip;
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
