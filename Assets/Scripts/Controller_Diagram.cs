using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Diagram : MonoBehaviour
{
    [SerializeField]
    private GameObject leftController;
    [SerializeField]
    private GameObject rightController;
    [SerializeField]
    private GameObject aButton;
    [SerializeField]
    private GameObject bButton;
    [SerializeField]
    private GameObject xButton;
    [SerializeField]
    private GameObject yButton;
    [SerializeField]
    private GameObject hamburgerButton;
    [SerializeField]
    private GameObject leftToggle;
    [SerializeField]
    private GameObject rightToggle;
    [SerializeField]
    private GameObject leftTrigger;
    [SerializeField]
    private GameObject rightTrigger;
    [SerializeField]
    private GameObject leftGrip;
    [SerializeField]
    private GameObject rightGrip;
    void Start()
    {
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
        hideHambugerButton();
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
    public void hideHambugerButton()
    {
        hamburgerButton.GetComponent<MeshRenderer>().enabled = false;
        hamburgerButton.GetComponentInChildren<Canvas>().enabled = false;
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
    public void showHamburgerButton()
    {
        hamburgerButton.GetComponent<MeshRenderer>().enabled = true;
        hamburgerButton.GetComponentInChildren<Canvas>().enabled = true;
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
