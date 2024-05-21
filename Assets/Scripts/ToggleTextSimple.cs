using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using TMPro;



public class ToggleTextSimple : MonoBehaviour
{
    GameObject text;

    public void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            TextToggle();
        }
    }

    void Start()
    {
        text = this.gameObject.transform.GetChild(0).gameObject;
    }
    void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onXButtonPressed += TextToggle;
        GameEventsManager.instance.taskEvents.onAdvanceTask += popUp;
    }

    //This code makes sure that if you complete a task the next popup appears without the need to toggle
    private void popUp(string obj)
    {
        if (this.isActiveAndEnabled && !(text.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text == "Welcome to the tutorial!\n\nYou can hide this popup with the primary button on your left controller, (X).\n\nTry hiding this popup and re-opening it by pressing (X) twice!"))
        {
            text.SetActive(true);
        }
    }

    void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onXButtonPressed -= TextToggle;
    }

    void TextToggle(InputAction.CallbackContext context)
    {
            text.SetActive(!text.activeInHierarchy);
    }
    void TextToggle()
    {
        text.SetActive(!text.activeInHierarchy);
    }
}
