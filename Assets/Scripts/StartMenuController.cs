using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
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
