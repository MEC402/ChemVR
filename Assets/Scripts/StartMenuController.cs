using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{

    public GameObject settingsCanvas;
    private bool settingsActive = false;

    private void Awake()
    {
        if (settingsCanvas.activeInHierarchy)
        {
            settingsActive = true;
        }
    }

    public void TurnOnCanvas()
    {
        if (!settingsActive)
        {
            settingsCanvas.SetActive(true);
            settingsActive = true;
        }
    }

    public void TurnOffCanvas()
    {
        if (settingsActive)
        {
            settingsCanvas.SetActive(false);
            settingsActive = false;
        }
    }
    public void StartTutorialBtn()
    {
        SceneManager.LoadScene("LabSceneTutorial");
    }

    public void StartGloveHygieneBtn()
    {
        SceneManager.LoadScene("LabSceneGloveHygiene");
    }

    public void StartGlasswareBtn()
    {
        SceneManager.LoadScene("LabSceneGlasswareUse");
    }

    public void StartChemicalChangeBtn()
    {
        SceneManager.LoadScene("LabSceneChemicalChange");
    }

    public void MainMenuBtn()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void ExitBtn()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }
}
