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
        Button yesButton = GameObject.Find("VR Movement and Interaction/Complete XR Origin Set Up Variant/XR Origin (XR Rig)/Camera Offset/Left Controller/Left Hand/UI/Spatial Panel Manipulator Model (1)/CoachingCardRoot/Yes Button").GetComponent<Button>();
        Button noButton = GameObject.Find("VR Movement and Interaction/Complete XR Origin Set Up Variant/XR Origin (XR Rig)/Camera Offset/Left Controller/Left Hand/UI/Spatial Panel Manipulator Model (1)/CoachingCardRoot/No Button").GetComponent<Button>();

        yesButton.onClick.AddListener(Complete);
        noButton.onClick.AddListener(Abandon);
    }
    public void Complete()
    {
        Restart();
        FinishTaskStep();
    }

    public void Restart()
    {
        GameObject taskManager = GameObject.Find("The Managers");
        int childCount = taskManager.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject child = taskManager.transform.GetChild(i).gameObject;
            if (child.name.Contains("tut") && !child.name.Contains("0"))
            {
                Destroy(child);
            } else if (!child.name.Contains("tut"))
            {
                Destroy(child);
            }
        }
    }
    public void Abandon()
    {
        GameObject destroy = GameObject.Find("Tutorial_Task_0(Clone)");
        Destroy(destroy);
    }
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }

}
