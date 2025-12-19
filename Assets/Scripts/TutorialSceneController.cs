using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSceneController : MonoBehaviour
{
    [Header("Scene Flow")]
    [SerializeField] private string nextSceneName = "LabSceneGlasswareUse";

    [Header("Input Settings")]
    [SerializeField] private float holdDuration = 4.0f; // Seconds to hold X or A

    private bool isLoading = false;
    private float holdTimer = 0f;

    void Update()
    {
        // On Quest, A and X are usually joystick buttons 0 and 2.
        bool aOrXHeld =
            Input.GetKey(KeyCode.JoystickButton0) || // A button (right controller)
            Input.GetKey(KeyCode.JoystickButton2);   // X button (left controller)

        if (aOrXHeld)
        {
            holdTimer += Time.deltaTime;

            // If held long enough and we're not already loading, go to next module
            if (holdTimer >= holdDuration && !isLoading)
            {
                OnTutorialComplete();
            }
        }
        else
        {
            // Reset if player lets go before time is complete
            holdTimer = 0f;
        }
    }

    // Called internally once player has held X/A long enough
    private void OnTutorialComplete()
    {
        if (isLoading)
            return;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // Guard against stepping past the last scene in build settings
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning($"No next scene in build settings after index {currentIndex}.");
            return;
        }

        StartCoroutine(LoadSceneAsync(nextIndex));
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        isLoading = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
            yield return null;
    }
}
