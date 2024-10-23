using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PauseMenuController : MonoBehaviour
{
    [Header("Menus")]
    public GameObject pauseMenu;
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
    private bool menuOpen = false;


    //public MeshRenderer background;
    //public MeshRenderer resume;
    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onPauseButtonPressed += Pause;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onPauseButtonPressed -= Pause;
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }


    /** Pause menu pops up,
     *  actions are disabled, 
     *  hands are added to the UI layer so they are visible over the menu */
    private void Pause(InputAction.CallbackContext obj)
    {
        if (!menuOpen)
        {
            pauseMenu.SetActive(true);
            setHandActions(false);
        }
        else
        {
            pauseMenu.SetActive(false);
            controlsMenu.SetActive(false);
            setHandActions(true);
        }
        menuOpen = !menuOpen;
    }

    /** Easy! Just load main menu scene*/
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu_PlaceHolder");
    }

    /** Easy! Just reload current scene. */
    public void ResetCurrent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /** Swap current pause menu with control menu*/
    public void Controls()
    {
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    /** Swap current control menu with pause menu*/
    public void BackFromControls()
    {
        controlsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    /**
     * If true, will turn on teleportation and turn off the ray interactor.
     * If false, will turn off teleportation and turn on the ray interactor.
     */
    private void setHandActions(bool active)
    {
        setRightHandActions(active);
        setLeftHandActions(active);

        if(!active)
        {
            leftHand.layer = UILayer;
            rightHand.layer = UILayer;
        } else
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