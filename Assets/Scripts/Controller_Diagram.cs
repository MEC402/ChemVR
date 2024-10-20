using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Diagram : MonoBehaviour
{
    private GameObject leftController;
    private GameObject rightController;
    private GameObject aButton;
    private GameObject bButton;
    private GameObject xButton;
    private GameObject yButton;
    private GameObject leftToggle;
    private GameObject rightToggle;
    private GameObject leftTrigger;
    private GameObject rightTrigger;
    private GameObject leftGrip;
    private GameObject rightGrip;
    void Start()
    {
        leftController = GameObject.Find("ControllerLeftDiagram");
        rightController = GameObject.Find("ControllerRightDiagram");
        aButton = GameObject.Find("A Button");
        bButton = GameObject.Find("B Button");
        xButton = GameObject.Find("X Button");
        yButton = GameObject.Find("Y Button");
        leftToggle = GameObject.Find("Left Toggle");
        rightToggle = GameObject.Find("Right Toggle");
        leftTrigger = GameObject.Find("Left Trigger");
        rightTrigger = GameObject.Find("Right Trigger");
        leftGrip = GameObject.Find("Left Grip");
        rightGrip = GameObject.Find("Right Grip");

        hideAllDiagrams();
    }
    public void hideAllDiagrams()
    {
        hideLeftController();
        hideRightController();
    }
    public void hideLeftController()
    {
        leftController.GetComponent<SpriteRenderer>().enabled = false;
        hideLeftToggle();
        hideLeftTrigger();
        hideLeftGrip();
        hideXButton();
        hideYButton();
    }
    public void hideRightController()
    {
        rightController.GetComponent<SpriteRenderer>().enabled = false;
        hideRightToggle();
        hideRightTrigger();
        hideRightGrip();
        hideAButton();
        hideBButton();
    }
    public void hideXButton()
    {
        xButton.GetComponent<MeshRenderer>().enabled = false;
        xButton.GetComponentInChildren<Canvas>().enabled = false;
    }
    public void hideYButton()
    {
        yButton.GetComponent<MeshRenderer>().enabled = false;
        yButton.GetComponentInChildren<Canvas>().enabled = false;
    }
    public void hideAButton()
    {
        aButton.GetComponent<MeshRenderer>().enabled = false;
        aButton.GetComponentInChildren<Canvas>().enabled = false;
    }
    public void hideBButton()
    {
        bButton.GetComponent<MeshRenderer>().enabled = false;
        bButton.GetComponentInChildren<Canvas>().enabled = false;
    }
    public void hideLeftToggle()
    {
        leftToggle.GetComponent<MeshRenderer>().enabled = false;
    }
    public void hideRightToggle()
    {
        rightToggle.GetComponent<MeshRenderer>().enabled = false;
    }
    public void hideLeftTrigger()
    {
        leftTrigger.GetComponent<MeshRenderer>().enabled = false;
    }
    public void hideRightTrigger()
    {
        rightTrigger.GetComponent<MeshRenderer>().enabled = false;
    }
    public void hideLeftGrip()
    {
        leftGrip.GetComponent<MeshRenderer>().enabled = false;
    }
    public void hideRightGrip()
    {
        rightGrip.GetComponent<MeshRenderer>().enabled = false;
    }
    public void showAllDiagrams()
    {
        showLeftController();
        showRightController();
    }
    public void showLeftController()
    {
        leftController.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void showRightController()
    {
        rightController.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void showXButton()
    {
        xButton.GetComponent<MeshRenderer>().enabled = true;
        xButton.GetComponentInChildren<Canvas>().enabled = true;
    }
    public void showYButton()
    {
        yButton.GetComponent<MeshRenderer>().enabled = true;
        yButton.GetComponentInChildren<Canvas>().enabled = true;
    }
    public void showAButton()
    {
        aButton.GetComponent<MeshRenderer>().enabled = true;
        aButton.GetComponentInChildren<Canvas>().enabled = true;
    }
    public void showBButton()
    {
        bButton.GetComponent<MeshRenderer>().enabled = true;
        bButton.GetComponentInChildren<Canvas>().enabled = true;
    }
    public void showLeftToggle()
    {
        leftToggle.GetComponent<MeshRenderer>().enabled = true;
    }
    public void showRightToggle()
    {
        rightToggle.GetComponent<MeshRenderer>().enabled = true;
    }
    public void showLeftTrigger()
    {
        leftTrigger.GetComponent<MeshRenderer>().enabled = true;
    }
    public void showRightTrigger()
    {
        rightTrigger.GetComponent<MeshRenderer>().enabled = true;
    }
    public void showLeftGrip()
    {
        leftGrip.GetComponent<MeshRenderer>().enabled = true;
    }
    public void showRightGrip()
    {
        rightGrip.GetComponent<MeshRenderer>().enabled = true;
    }
}
