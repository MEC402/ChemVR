using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Hold_AX_To_Continue_Tut : TaskStep
{    
    public GameObject timerPrefab;
    private GameObject createdTimer;
    bool cancelled = false;
    float secondLength = .33f;

    private Transform mainCamera;
    private Vector3 localOffset = new Vector3(0, 0, .6f);

    void OnEnable()
    {
        mainCamera = GameObject.Find("Main Camera").transform;
        GameEventsManager.instance.inputEvents.onAButtonPressed += loadNextScene;
        GameEventsManager.instance.inputEvents.onXButtonPressed += loadNextScene;

        GameEventsManager.instance.inputEvents.onAButtonReleased += unloadNextScene;
        GameEventsManager.instance.inputEvents.onXButtonReleased += unloadNextScene;
    }
    void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onAButtonPressed -= loadNextScene;
        GameEventsManager.instance.inputEvents.onXButtonPressed -= loadNextScene;

        GameEventsManager.instance.inputEvents.onAButtonReleased -= unloadNextScene;
        GameEventsManager.instance.inputEvents.onXButtonReleased -= unloadNextScene;
    }

    private void loadNextScene(InputAction.CallbackContext obj)
    {
        if (timerPrefab != null)
        {

            createdTimer = Instantiate(timerPrefab, mainCamera.position, mainCamera.rotation, mainCamera);
            createdTimer.transform.localPosition = localOffset;
            createdTimer.transform.localRotation = Quaternion.identity;
        }
        cancelled = false;
        StartCoroutine(CountRoutine());
    }

    private void unloadNextScene(InputAction.CallbackContext obj)
    {
        cancelled = true;
        StopCoroutine(CountRoutine());
        if (createdTimer != null)
        {
            Destroy(createdTimer);
        }
    }
    private IEnumerator CountRoutine()
    {
        yield return new WaitForSeconds(secondLength);
        yield return new WaitForSeconds(secondLength);
        yield return new WaitForSeconds(secondLength);
        if(cancelled == false)
        {
            SceneManager.LoadScene("LabSceneGloveHygiene");
        }
    }

    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
}
