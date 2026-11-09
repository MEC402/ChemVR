using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialSceneController : MonoBehaviour
{
    [Header("Scene Flow")]
    [SerializeField] private string nextSceneName = "LabSceneGlasswareUse";

    [Header("Input Settings")]
    [SerializeField] private float holdDuration = 4.0f; // Seconds to hold X or A once the tutorial is done

    [Tooltip("Escape hatch: holding this long always advances, even if the tutorial " +
        "gate below hasn't been met, in case someone gets stuck on a broken step.")]
    [SerializeField] private float forceAdvanceHoldDuration = 10.0f;

    [Header("Tutorial Gate")]
    [Tooltip("A/X only advances the scene once this task has finished its last step, " +
        "so people can't accidentally hold the button and skip the tutorial.")]
    [SerializeField] private string tutorialTaskId = "Tutorial_Task";

    [Header("Loading Indicator")]
    [Tooltip("Root object of the hold-to-advance spinner. Shown while A/X is held, hidden when released.")]
    [SerializeField] private GameObject loadingIndicator;
    [Tooltip("Optional radial progress image (Image.Type = Filled) showing how close the hold is to firing.")]
    [SerializeField] private Image progressFillImage;
    [Tooltip("Optional icon that spins continuously while the indicator is visible.")]
    [SerializeField] private Transform spinnerIcon;
    [SerializeField] private float spinnerDegreesPerSecond = 360f;

    private bool isLoading = false;
    private float holdTimer = 0f;
    private bool tutorialComplete = false;
    private bool aHeld = false;
    private bool xHeld = false;

    private void OnEnable()
    {
        GameEventsManager.instance.taskEvents.onTaskStateChange += OnTaskStateChange;

        GameEventsManager.instance.inputEvents.onAButtonPressed += OnAPressed;
        GameEventsManager.instance.inputEvents.onAButtonReleased += OnAReleased;
        GameEventsManager.instance.inputEvents.onXButtonPressed += OnXPressed;
        GameEventsManager.instance.inputEvents.onXButtonReleased += OnXReleased;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.taskEvents.onTaskStateChange -= OnTaskStateChange;

        GameEventsManager.instance.inputEvents.onAButtonPressed -= OnAPressed;
        GameEventsManager.instance.inputEvents.onAButtonReleased -= OnAReleased;
        GameEventsManager.instance.inputEvents.onXButtonPressed -= OnXPressed;
        GameEventsManager.instance.inputEvents.onXButtonReleased -= OnXReleased;
    }

    private void OnAPressed(InputAction.CallbackContext context) => aHeld = true;
    private void OnAReleased(InputAction.CallbackContext context) => aHeld = false;
    private void OnXPressed(InputAction.CallbackContext context) => xHeld = true;
    private void OnXReleased(InputAction.CallbackContext context) => xHeld = false;

    private void OnTaskStateChange(Task task)
    {
        if (task.info.id != tutorialTaskId)
            return;

        if (task.state == TaskState.CAN_FINISH || task.state == TaskState.FINISHED)
            tutorialComplete = true;
    }

    void Update()
    {
        bool aOrXHeld = aHeld || xHeld;

        if (aOrXHeld)
        {
            holdTimer += Time.deltaTime;

            // Whichever threshold currently applies is what the spinner fills toward.
            float activeThreshold = tutorialComplete ? holdDuration : forceAdvanceHoldDuration;
            ShowLoadingIndicator(holdTimer / activeThreshold);

            if (isLoading)
                return;

            // Normal path: only once the tutorial's last step is done.
            // Escape hatch: a longer hold always advances, in case the tutorial gate never fires.
            bool normalAdvance = tutorialComplete && holdTimer >= holdDuration;
            bool forceAdvance = holdTimer >= forceAdvanceHoldDuration;

            if (normalAdvance || forceAdvance)
            {
                OnTutorialComplete();
            }
        }
        else
        {
            // Reset if player lets go before time is complete
            holdTimer = 0f;
            HideLoadingIndicator();
        }
    }

    private void ShowLoadingIndicator(float progress)
    {
        if (loadingIndicator != null)
            loadingIndicator.SetActive(true);

        if (progressFillImage != null)
            progressFillImage.fillAmount = Mathf.Clamp01(progress);

        if (spinnerIcon != null)
            spinnerIcon.Rotate(Vector3.forward, -spinnerDegreesPerSecond * Time.deltaTime);
    }

    private void HideLoadingIndicator()
    {
        if (loadingIndicator != null)
            loadingIndicator.SetActive(false);
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
