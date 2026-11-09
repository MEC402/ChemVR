using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Placeholder step for the Physical & Chemical Changes module (Chemical_Change_Task). Completes on a "click":
///   - VR:     A button (inputEvents.onAButtonPressed)
///   - WebGL:  skip key (inputEvents.onWebGLSkipTask)
///   - UI:     a Next button calling Chemical_Change_Overview.ClickNext() (miscEvents.OnClickThroughNext)
///   - Dev:    B button, via the TaskStep base class when dev-ops is on.
/// To make a step real, build the working piece as its own TaskStep subclass and swap its
/// prefab into taskStepPrefabs at the same index (see docs/module3_task_sequence.md).
/// </summary>
public class Module3_ClickThroughStep : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        // Placeholder steps carry no resume state.
    }

    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed += OnAButton;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask += OnSkip;
        GameEventsManager.instance.miscEvents.OnClickThroughNext += OnNext;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= OnAButton;
        GameEventsManager.instance.inputEvents.onWebGLSkipTask -= OnSkip;
        GameEventsManager.instance.miscEvents.OnClickThroughNext -= OnNext;
    }

    private void OnAButton(InputAction.CallbackContext ctx)
    {
        FinishTaskStep();
    }

    private void OnSkip(InputAction.CallbackContext ctx)
    {
        FinishTaskStep();
    }

    private void OnNext()
    {
        FinishTaskStep();
    }
}
