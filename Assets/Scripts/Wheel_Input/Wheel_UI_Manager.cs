using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
/** On Right toggle hold: Pop up 
* On right toggle release: If one of the options is highlighted: start that task 0 + prompt the use if they are sure, Else: Disapear
* On toggle up: Top moves up + gets bigger + color changes
* On toggle right: Right moves right + gets bigger + color changes
* On toggle down: Top moves down + gets bigger + color changes
* On toggle left: Top moves left + gets bigger + color changes
* Start is called before the first frame update
*/
public class Wheel_UI_Manager : MonoBehaviour
{
    private Wheel_Inputs inputActions;

    //The input of the right toggle
    Vector2 rightStickInput;

    // Control other right hand uses
    [Header("Right Hand Motion Controllers")]
    public ActionBasedSnapTurnProvider turnProvider;
    public XRRayInteractor teleportationProvider;
    private bool rightHandMovementsActive;

    // Controllers for starting tasks
    [Header("Start Task Controllers")]
    public Toggle_Module_All taskPreper;
    public StartTasks taskStarter;
    private Transform taskManager;

    [Header("Right Hand Ray Interactor")]
    [SerializeField]
    XRRayInteractor xrRayInteractor_R;
    [SerializeField]
    LineRenderer lineRenderer_R;
    [SerializeField]
    XRInteractorLineVisual xrInteractorLineVisual_R;

    //Overview popups
    [Header("Overview Pop Ups")]
    [SerializeField]
    GameObject chemicalOverview;
    [SerializeField]
    GameObject glasswareOverview;
    [SerializeField]
    GameObject gloveOverview;
    [SerializeField]
    GameObject tutorialOverview;

    // All show/select booleans
    private bool shown;
    private bool anySelected;
    private bool topSelected;
    private bool bottomSelected;
    private bool leftSelected;
    private bool rightSelected;

    // Text for each option
    [Header("Text Boxes")]
    public TextMeshPro areYouSureTxt;
    public TextMeshPro topTxt;
    public TextMeshPro bottomTxt;
    public TextMeshPro leftTxt;
    public TextMeshPro rightTxt;

    // All Gameobjects
    private GameObject ui;
    private GameObject topUIButton;
    private GameObject bottomUIButton;
    private GameObject leftUIButton;
    private GameObject rightUIButton;
    private GameObject center;

    // Original positions
    private Vector3 topPos;
    private Vector3 bottomPos;
    private Vector3 leftPos;
    private Vector3 rightPos;

    //Displacements
    private Vector3 vertDisp = new Vector3(0.1f, 0, 0.1f);
    private Vector3 horDisp = new Vector3(0.1f, 0, -0.1f);

    //All renderers
    private Renderer topRenderer;
    private Renderer bottomRenderer;
    private Renderer leftRenderer;
    private Renderer rightRenderer;
    private Renderer centerRenderer;

    // Materials
    [Header("Wheel Materials")]
    public Material white;
    public Material highlighted_blue;

    //bools to handle restarting 
    bool restartTut;
    bool restartChe;
    bool restartGla;
    bool restartGlo;
    private void Awake()
    {
        inputActions = new Wheel_Inputs();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Right_Hand.Enable();

        taskManager = GameObject.Find("The Managers").transform;

        ui = GameObject.Find("newUI");
        topUIButton = GameObject.Find("Wheel Top");
        bottomUIButton = GameObject.Find("Wheel Bottom");
        leftUIButton = GameObject.Find("Wheel Left");
        rightUIButton = GameObject.Find("Wheel Right");
        center = GameObject.Find("Wheel Center");

        topPos = topUIButton.transform.localPosition;
        bottomPos = bottomUIButton.transform.localPosition;
        leftPos = leftUIButton.transform.localPosition;
        rightPos = rightUIButton.transform.localPosition;

        topRenderer = topUIButton.GetComponent<Renderer>();
        bottomRenderer = bottomUIButton.GetComponent<Renderer>();
        leftRenderer = leftUIButton.GetComponent<Renderer>();
        rightRenderer = rightUIButton.GetComponent<Renderer>();
        centerRenderer = center.GetComponent<Renderer> ();

        ui.SetActive(false);
        shown = false;

        unSelectAll();
        rightHandMovementsActive = true;

        restartTut = false;
        restartChe = false;
        restartGla = false;
        restartGlo = false;

        GameEventsManager.instance.inputEvents.onRTriggerPressed += popup;
        GameEventsManager.instance.inputEvents.onRTriggerReleased += handle_and_hide;
    }

    private void OnDisable()
    {
        inputActions.Right_Hand.Disable();
        inputActions.Disable();

        GameEventsManager.instance.inputEvents.onRTriggerPressed -= popup;
        GameEventsManager.instance.inputEvents.onRTriggerReleased -= handle_and_hide;
    }

    //This has to be removed for the laser beam to work properly. All good! Then we don't use the mouse :)
    /*
    private void Update()
    {
        if(shown)
        {
            rightStickInput = inputActions.Right_Hand.MoveToggle.ReadValue<Vector2>();
            if (rightStickInput.magnitude > 0.1f)
            {
                if (rightStickInput.y > 0.5f)
                {
                    HandleRightStickUp();
                }
                else if (rightStickInput.y < -0.5f)
                {
                    HandleRightStickDown();
                }
                else if (rightStickInput.x > 0.5f)
                {
                    HandleRightStickRight();
                }
                else if (rightStickInput.x < -0.5f)
                {
                    HandleRightStickLeft();
                }
                else
                {
                    unSelectAll();
                }
            }
            else
            {
                unSelectAll();
            }
        }
        // re-enable right joystick actions if none selected, not shown, and not already enabled
        if (!shown && !rightHandMovementsActive)
        {
            rightStickInput = inputActions.Right_Hand.MoveToggle.ReadValue<Vector2>();
            //make hand reset first
            if (rightStickInput.magnitude < 0.1f || ((rightStickInput.y < 0.5f) && (rightStickInput.y > -0.5f)) && ((rightStickInput.x < 0.5f) && (rightStickInput.x > -0.5f)))
            {
                setRightHandActions(true);
            }
        }
    }
    */
    private void popup(InputAction.CallbackContext obj)
    {
        //Disable right joystick actions on show
        shown = true;
        setRightHandActions(false);

        //If you reopen the popup instead of continue / abandon
        if (taskPreper.current != null)
        {
            GameEventsManager.instance.inputEvents.onAButtonPressed -= Continue;
            GameEventsManager.instance.inputEvents.onBButtonPressed -= Abandon;
            taskPreper.UnPrep();
        }

        //Make the ui visible
        setAllVisable(true);
        ui.SetActive(true);
        unSelectAll();
    }

    private void handle_and_hide(InputAction.CallbackContext obj)
    {
        if (anySelected)
        {
            setAllVisable(false);
            GameEventsManager.instance.inputEvents.onAButtonPressed += Continue;
            GameEventsManager.instance.inputEvents.onBButtonPressed += Abandon;
            restartTut = false;
            restartChe = false;
            restartGla = false;
            restartGlo = false;
            bool startBlank = false;
            string existing = "";
            if (taskManager.childCount == 0)
            {
                startBlank = true;
            } else
            {
                existing = taskManager.GetChild(0).name;
            }
            if (topSelected)
            {
                if (!startBlank && existing.Contains("tut"))
                {
                    restartTut = true;
                } else
                {
                    startTut();
                }
            }
            else if (bottomSelected)
            {
                if (!startBlank && existing.Contains("gla"))
                {
                    restartGla = true;
                }
                else
                {
                    startGla();
                }
            }
            else if (leftSelected)
            {
                if (!startBlank && existing.Contains("che"))
                {
                    restartChe = true;
                }
                else
                {
                    StartChem();
                }
            }
            else if (rightSelected)
            {
                if (!startBlank && existing.Contains("glo"))
                {
                    restartGlo = true;
                }
                else
                {
                    StartGlo();
                }
            }


            if (startBlank) //If no other modules going, don't bother with popup
            {
                SkipPopUp();
            }
        }
        else
        {
            Hide();
        }
    }


    private void startTut()
    {
        taskPreper.TutorialPrep();
        taskStarter.StartTutorial();
    }
    private void StartChem()
    {
        taskPreper.ChemicalChangePrep();
        taskStarter.StartChemicalChange();
    }
    private void startGla()
    {
        taskPreper.GlasswareUsePrep();
        taskStarter.StartGlasswareUse();
    }
    private void StartGlo()
    {
        taskPreper.GloveHygienePrep();
        taskStarter.StartGloveHygiene();
    }
    private void Hide()
    {
        shown = false;
        ui.SetActive(false);

        //Only re-enable right hand actions if joystick reset
        if (rightStickInput.magnitude < 0.1f || ((rightStickInput.y < 0.5f) && (rightStickInput.y > -0.5f)) && ((rightStickInput.x < 0.5f) && (rightStickInput.x > -0.5f)))
        {
            setRightHandActions(true);
        }
    }

    private void SkipPopUp()
    {
        Continue();
        string taskName = (taskPreper.current.name).Replace(" ", "_");
        GameEventsManager.instance.taskEvents.AdvanceTask(taskName + "_Task");
        Destroy(taskManager.GetChild(0).gameObject);
    }

    private void Continue(InputAction.CallbackContext obj)
    {
        if (restartTut)
        {
            startTut();
            SkipPopUp();
        } else if (restartChe)
        {
            StartChem();
            SkipPopUp();
        } else if (restartGla)
        {
            startGla();
            SkipPopUp();
        }
        else if (restartGlo)
        {
            StartGlo();
            SkipPopUp();
        } else
        {
            Continue();
        }
    }
    private void Continue()
    {
        StartAny();
        GameEventsManager.instance.inputEvents.onAButtonPressed -= Continue;
        GameEventsManager.instance.inputEvents.onBButtonPressed -= Abandon;
        Hide();
    }

    private void Abandon(InputAction.CallbackContext obj)
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= Continue;
        GameEventsManager.instance.inputEvents.onBButtonPressed -= Abandon;
        taskPreper.UnPrep();
        Hide();
    }

    private void setAllVisable(bool visible)
    {
        topRenderer.enabled = visible;
        bottomRenderer.enabled = visible;
        leftRenderer.enabled = visible; ;
        rightRenderer.enabled = visible;
        centerRenderer.enabled = visible;

        areYouSureTxt.enabled = !visible;
        topTxt.enabled = visible;
        bottomTxt.enabled = visible;
        leftTxt.enabled = visible;
        rightTxt.enabled = visible;

    }
    private void setRightHandActions(bool active)
    {
        turnProvider.enableTurnAround = active;
        turnProvider.enableTurnLeftRight = active;
        teleportationProvider.enabled = active;
        rightHandMovementsActive = active;

        //Swap right hand enactors for the ray interactor
        //enable right ray interactor on show
        xrRayInteractor_R.enabled = !active;
        lineRenderer_R.enabled = !active;
        xrInteractorLineVisual_R.enabled = !active;
    }
    public void unSelectAll()
    {
        anySelected = false;
        
        topSelected = false;
        topUIButton.transform.localPosition = topPos;
        topRenderer.material = white;

        bottomSelected = false;
        bottomUIButton.transform.localPosition = bottomPos;
        bottomRenderer.material = white;

        leftSelected = false;
        leftUIButton.transform.localPosition = leftPos;
        leftRenderer.material = white;
        
        rightSelected = false;
        rightUIButton.transform.localPosition = rightPos;
        rightRenderer.material = white;
    }
    public void HandleRightStickUp()
    {
        if (shown)
        {
            if(anySelected)
                unSelectAll();
            anySelected = true;
            topSelected = true;
            topUIButton.transform.localPosition -= vertDisp;
            topRenderer.material = highlighted_blue;
        }
    }

    public void HandleRightStickDown()
    {
        if (shown)
        {
            if (anySelected)
                unSelectAll();
            anySelected = true;
            bottomSelected = true;
            bottomUIButton.transform.localPosition += vertDisp;
            bottomRenderer.material = highlighted_blue;
        }
    }

    public void HandleRightStickRight()
    {
        if (shown)
        {
            if (anySelected)
                unSelectAll();
            anySelected = true;
            rightSelected = true;
            rightUIButton.transform.localPosition -= horDisp;
            rightRenderer.material = highlighted_blue;
        }
    }

    public void HandleRightStickLeft()
    {
        if (shown)
        {
            if (anySelected)
                unSelectAll();
            anySelected = true;
            leftSelected = true;
            leftUIButton.transform.localPosition += horDisp;
            leftRenderer.material = highlighted_blue;
        }
    }

    private void StartAny()
    {
        GameObject current = taskPreper.current;
        //Try and abandon/restart all tasks - must restart as button press is step 0 on all tasks.
        if (tutorialOverview.GetComponent<ToggleTextSimple>().enabled)
        {
            if (!current.name.Equals("Tutorial"))
            {
                GameEventsManager.instance.taskEvents.AbandonTask("Tutorial_Task");
            }
            else
            {
                tutorialOverview.GetComponent<Tutorial_Overview>().restart();
            }
        }
        if (gloveOverview.GetComponent<ToggleTextSimple>().enabled)
        {
            if (!current.name.Equals("Glove Hygiene"))
            {
                GameEventsManager.instance.taskEvents.AbandonTask("Glove_Hygiene_Task");
            }
            else
            {
                Debug.Log("Restarting Glove Hygiene");
                gloveOverview.GetComponent<Glove_Hygiene_Overview>().restart();
            }
        }
        if (glasswareOverview.GetComponent<ToggleTextSimple>().enabled)
        {
            if (!current.name.Equals("Glassware Use"))
            {
                GameEventsManager.instance.taskEvents.AbandonTask("Glassware_Use_Task");
            }
            else
            {
                glasswareOverview.GetComponent<Glassware_Use_Overview>().restart();
            }
        }
        if (chemicalOverview.GetComponent<ToggleTextSimple>().enabled)
        {
            if (!current.name.Equals("Chemical Change"))
            {
                GameEventsManager.instance.taskEvents.AbandonTask("Chemical_Change_Task");
            }
            else
            {
                chemicalOverview.GetComponent<Chemical_Change_Overview>().restart();
            }
        }
        this.GetComponent<Toggle_Module_All>().Show();

    }
}
