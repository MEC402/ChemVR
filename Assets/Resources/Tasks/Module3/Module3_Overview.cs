using TMPro;
using UnityEngine;

/// <summary>
/// Instruction popup driver for Module 3, modeled on Chemical_Change_Overview.
/// Starts Module3_Task on enable and shows Module3Steps.BuildText for the current step.
/// Scene wiring: put this on the module's popup object (same object as ToggleTextSimple),
/// assign m3Text and taskPreper, and optionally hook a UI Button to ClickNext().
/// </summary>
public class Module3_Overview : MonoBehaviour
{
    public TextMeshProUGUI m3Text;
    public Start_Module taskPreper;
    public bool isWebGL;

    private int curStep;
    private ToggleTextSimple toggle;

    void OnEnable()
    {
        toggle = GetComponent<ToggleTextSimple>();
        if (toggle != null) toggle.enabled = false;
        curStep = 0;

        if (GameEventsManager.instance == null)
        {
            if (m3Text != null) m3Text.text = "There is no game events manager";
            return;
        }

        GameEventsManager.instance.taskEvents.onAbandonTask += Abandon;
        GameEventsManager.instance.taskEvents.onAdvanceTask += Advance;

        if (taskPreper != null) taskPreper.Show();
        if (toggle != null) toggle.enabled = true;
        GameEventsManager.instance.taskEvents.StartTask(Module3Steps.TaskId);
        Render();
    }

    void OnDisable()
    {
        if (toggle != null) toggle.enabled = false;
        if (GameEventsManager.instance == null) return;
        GameEventsManager.instance.taskEvents.onAbandonTask -= Abandon;
        GameEventsManager.instance.taskEvents.onAdvanceTask -= Advance;
    }

    /// <summary>Hook a UI Button's OnClick to this to advance a placeholder step.</summary>
    public void ClickNext()
    {
        GameEventsManager.instance.miscEvents.ClickThroughNext();
    }

    private void Advance(string context)
    {
        if (context == Module3Steps.TaskId)
        {
            if (toggle != null) toggle.enabled = true;
            curStep++;
            Render();
        }
    }

    private void Abandon(string context)
    {
        // Exact match: other overviews use substring checks like Contains("Chemical"),
        // so this module's id deliberately avoids those words.
        if (context == Module3Steps.TaskId)
        {
            if (toggle != null) toggle.enabled = false;
            curStep = 0;
        }
    }

    private void Render()
    {
        if (m3Text != null) m3Text.text = Module3Steps.BuildText(curStep, isWebGL);
    }
}
