using UnityEngine;

/// <summary>
/// Reagent "add drops" step of Module 3: a bulb dropper holds 1 mL, filled and emptied in four use presses each.
/// Finishes once a dropper that was filled has been emptied into a container.
/// </summary>
public class Empty_Dropper_CC : TaskStep
{
    protected override void SetTaskStepState(string state) { }

    void OnEnable() { PipetteFunctions.OnEmptied += OnEmptied; }
    void OnDisable() { PipetteFunctions.OnEmptied -= OnEmptied; }

    void OnEmptied(PipetteFunctions p) { FinishTaskStep(); }
}
