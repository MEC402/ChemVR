using UnityEngine;

// Marks the tutorial's final step. TutorialSceneController now owns the actual
// A/X hold-to-advance timing and scene transition (gated on this task reaching
// CAN_FINISH via TaskInfoSO.canFinishOnFinalStep); this step just needs to stay
// instantiated so its panel remains displayed while that hold is in progress.
public class Hold_AX_To_Continue_Tut : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
}
