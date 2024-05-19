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
        /** Debug.LogWarning("Task 0 complete in itself");
        Button tutorialButton = GameObject.Find("Tutorial Button").GetComponent<Button>(); 
        tutorialButton.onClick.RemoveListener(Complete);*/
        FinishTaskStep();
    }

    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }

}
