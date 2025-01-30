using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnAtStation : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject glovePos;
    private GameObject chemicalPos;
    private GameObject glasswarePos;
    private GameObject tutorialPos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("XR Origin (XR Rig)");
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not set.");
        }

        //Set the gameobject for each teleport location
        glovePos = GameObject.Find("Glove Hygiene Spawn");
        chemicalPos = GameObject.Find("Chemical Change Spawn");
        glasswarePos = GameObject.Find("Glassware Use Spawn");
        tutorialPos = null; // TO DO

        GameEventsManager.instance.taskEvents.onTaskStartApproved += startedNewModule;
    }

    private void startedNewModule(string obj)
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
        //Set appropriate Listeners
        if (obj.ToLower().Contains("glo"))
        {
            //Teleport to where you start glove hygiene module
            SceneManager.LoadScene("LabSceneGloveHygiene");
            //mainCamera.transform.SetPositionAndRotation(glovePos.transform.position, glovePos.transform.rotation);
        }
        else if (obj.ToLower().Contains("che"))
        {
            //Teleport to where you start chemical change module
            SceneManager.LoadScene("LabSceneChemicalChange");
            //mainCamera.transform.SetPositionAndRotation(chemicalPos.transform.position, chemicalPos.transform.rotation);
        }
        else if (obj.ToLower().Contains("gla"))
        {
            //Teleport to where you start glassware use module
            SceneManager.LoadScene("LabSceneGlasswareUse"); 
            //mainCamera.transform.SetPositionAndRotation(glasswarePos.transform.position, glasswarePos.transform.rotation);
        }
        else if (obj.ToLower().Contains("tut"))
        {
            SceneManager.LoadScene("LabSceneTutorial");
            //Teleport to where you start tutorial

        }
    }
}
