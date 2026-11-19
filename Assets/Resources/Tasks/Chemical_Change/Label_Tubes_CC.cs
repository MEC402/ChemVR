using UnityEngine;

/// <summary>
/// Task 8 of the Physical &amp; Chemical Changes module (Label the test tubes): finishes once the Sharpie has
/// made the labels of at least <see cref="requiredCount"/> tubes visible (tubes 1 and 2).
/// Tubes labeled before this step started also count.
/// </summary>
public class Label_Tubes_CC : TaskStep
{
    [SerializeField] int requiredCount = 2;

    protected override void SetTaskStepState(string state) { }

    void OnEnable()
    {
        TubeLabel.OnLabeled += OnLabeled;
        Check();
    }

    void OnDisable()
    {
        TubeLabel.OnLabeled -= OnLabeled;
    }

    void OnLabeled(TubeLabel t)
    {
        Debug.Log("[Label_Tubes_CC] tube " + t.Number + " labeled");
        Check();
    }

    void Check()
    {
        int n = 0;
        foreach (var t in FindObjectsByType<TubeLabel>(FindObjectsSortMode.None))
            if (t.IsLabeled) n++;
        if (n >= requiredCount) FinishTaskStep();
    }
}
