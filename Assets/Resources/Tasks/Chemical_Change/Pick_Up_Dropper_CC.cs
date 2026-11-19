using UnityEngine;

/// <summary>
/// Reagent "get the bottle and dropper" step of Module 3: finishes when the player picks up a bulb dropper
/// (a grabbed object whose name contains <see cref="nameContains"/>). The bottle itself is not required.
/// </summary>
public class Pick_Up_Dropper_CC : TaskStep
{
    [SerializeField] string nameContains = "Dropper";

    protected override void SetTaskStepState(string state) { }

    void OnEnable() { GameEventsManager.instance.webGLEvents.OnObjectGrabbed += OnGrabbed; }
    void OnDisable() { GameEventsManager.instance.webGLEvents.OnObjectGrabbed -= OnGrabbed; }

    void OnGrabbed(GameObject obj)
    {
        if (obj != null && obj.name.IndexOf(nameContains, System.StringComparison.OrdinalIgnoreCase) >= 0)
            FinishTaskStep();
    }
}
