using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void StartTutorialBtn() {
        SceneManager.LoadScene("Tutorial");
    }

    public void StartGloveHygieneBtn()
    {
        SceneManager.LoadScene("LabSceneBase");
    }

    public void StartGlasswareBtn()
    {
        SceneManager.LoadScene("GlasswareScene");
    }

    public void StartChemicalChangeBtn()
    {
        SceneManager.LoadScene("ChemicalChangeScene");
    }

}
