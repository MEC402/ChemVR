using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GU_Task_0 : TaskStep
{
    public void Start()
    {
        Button tutorialButton = GameObject.Find("Glassware Use Button").GetComponent<Button>();

        tutorialButton.onClick.AddListener(Complete);
    }
    public void Complete()
    {
        FinishTaskStep();
    }

    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }

}
