using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class MainMenuController : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject expirementMenu;
    public GameObject controlsMenu;

    [Header("Hands")]
    public GameObject leftHand;
    public GameObject rightHand;

    // Control other right hand uses
    [Header("Right Hand Motion Controllers")]
    public XRRayInteractor teleportationProvider_R;

    [Header("Right Hand Ray Interactor")]
    [SerializeField]
    XRRayInteractor xrRayInteractor_R;
    [SerializeField]
    LineRenderer lineRenderer_R;
    [SerializeField]
    XRInteractorLineVisual xrInteractorLineVisual_R;

    // Control other right hand uses
    [Header("Left Hand Motion Controllers")]
    public XRRayInteractor teleportationProvider_L;

    [Header("Left Hand Ray Interactor")]
    [SerializeField]
    XRRayInteractor xrRayInteractor_L;
    [SerializeField]
    LineRenderer lineRenderer_L;
    [SerializeField]
    XRInteractorLineVisual xrInteractorLineVisual_L;

    private int UILayer = 5;
    private int defaultLayer = 0;

    void Start()
    {
        //Start with main menu
        mainMenu.SetActive(true);
        setHandActions(false);
    }

    public void Begin()
    {
        SceneManager.LoadScene("LabSceneTutorial");
    }

    /** Swap current pause menu with control menu*/
    public void Expirements()
    {
        mainMenu.SetActive(false);
        expirementMenu.SetActive(true);
    }

    /** Start Tutorial */
    public void StartTutorial()
    {
        SceneManager.LoadScene("LabSceneTutorial");
    }

    /** Start Tutorial */
    public void StartExp1()
    {
        SceneManager.LoadScene("LabSceneGloveHygiene");
    }

    /** Start Tutorial */
    public void StartExp2()
    {
        SceneManager.LoadScene("LabSceneGlasswareUse");
    }

    /** Start Tutorial */
    public void StartExp3()
    {
        SceneManager.LoadScene("LabSceneChemicalChange");
    }

    /** Swap current control menu with pause menu*/
    public void BackFromExpirements()
    {
        expirementMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    /** Swap current pause menu with control menu*/
    public void Controls()
    {
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    /** Swap current control menu with pause menu*/
    public void BackFromControls()
    {
        controlsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    /**
     * If true, will turn on teleportation and turn off the ray interactor.
     * If false, will turn off teleportation and turn on the ray interactor.
     */
    private void setHandActions(bool active)
    {
        setRightHandActions(active);
        setLeftHandActions(active);

        if (!active)
        {
            leftHand.layer = UILayer;
            rightHand.layer = UILayer;
        }
        else
        {
            leftHand.layer = defaultLayer;
            rightHand.layer = defaultLayer;
        }
    }

    /**
     * If true, will turn on teleportation and turn off the ray interactor.
     * If false, will turn off teleportation and turn on the ray interactor.
     */
    private void setRightHandActions(bool active)
    {
        // Turn off teleporter
        teleportationProvider_R.enabled = active;

        //Swap right hand enactors for the ray interactor
        xrRayInteractor_R.enabled = !active;
        lineRenderer_R.enabled = !active;
        xrInteractorLineVisual_R.enabled = !active;
    }

    /**
     * If true, will turn on teleportation and turn off the ray interactor.
     * If false, will turn off teleportation and turn on the ray interactor.
     */
    private void setLeftHandActions(bool active)
    {
        // Turn off teleporter
        teleportationProvider_L.enabled = active;

        //Swap right hand enactors for the ray interactor
        xrRayInteractor_L.enabled = !active;
        lineRenderer_L.enabled = !active;
        xrInteractorLineVisual_L.enabled = !active;
    }
}