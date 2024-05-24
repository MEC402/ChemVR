using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tutorial_Task_0 : TaskStep
{
    public void Start()
    {
        Button tutorialButton = GameObject.Find("Tutorial Button").GetComponent<Button>();

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
