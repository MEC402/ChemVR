using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject leftHand;
    public GameObject rightHand;

    private int UILayer = 5;
    private int defaultLayer = 0;
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
        if (!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            leftHand.layer = UILayer;
            rightHand.layer = UILayer;
        } else
        {
            pauseMenu.SetActive(false);
            leftHand.layer = defaultLayer;
            rightHand.layer = defaultLayer;
        }
        Debug.Log("Game paused!");
    }

    /** Easy! Just load main menu scene*/
    public void MainMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /** Easy! Just reload current scene. */
    public void ResetCurrent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /** Easy! Just swap current pause menu with controlls menu*/
    public void Controls()
    {
        throw new NotImplementedException("Controlls needs to be implemented");
    }
}
