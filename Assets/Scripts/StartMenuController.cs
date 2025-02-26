using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{

    public GameObject settingsCanvas;
    private bool settingsActive = false;
    bool isLoading = false;

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
        // if (!isLoading)
        //     SceneManager.LoadScene("LabSceneTutorial");

        if (!isLoading)
            StartCoroutine(LoadSceneAsync("LabSceneTutorial"));
    }

    public void StartGloveHygieneBtn()
    {
        // if (!isLoading)
        //     SceneManager.LoadScene("LabSceneGloveHygiene");

        if (!isLoading)
            StartCoroutine(LoadSceneAsync("LabSceneGloveHygiene"));
    }

    public void StartGlasswareBtn()
    {
        // if (!isLoading)
        // SceneManager.LoadScene("LabSceneGlasswareUse");

        if (!isLoading)
            StartCoroutine(LoadSceneAsync("LabSceneGlasswareUse"));
    }

    public void StartChemicalChangeBtn()
    {
        // if (!isLoading)
        // SceneManager.LoadScene("LabSceneChemicalChange");

        if (!isLoading)
            StartCoroutine(LoadSceneAsync("LabSceneChemicalChange"));
    }

    public void MainMenuBtn()
    {
        // if (!isLoading)
        // SceneManager.LoadScene("Start Menu");

        if (!isLoading)
            StartCoroutine(LoadSceneAsync("Start Menu"));
    }

    public void ExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
    }

    /// <summary>
    /// Loads the specified scene asynchronously. This allows the VR headset to remain responsive while the scene is loading. When the scene is loaded,
    /// the headset will automatically switch to the new scene.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    /// <returns></returns>
    IEnumerator LoadSceneAsync(string sceneName)
    {
        isLoading = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
            yield return null;
    }
}
