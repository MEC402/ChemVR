using UnityEngine;

/// <summary>
/// Shows the Instructions popup while the player looks at a transparent panel parented to the
/// back of their left hand (i.e. the panel's normal points at the camera, like checking a watch),
/// then restores whatever visibility the popup had before once they stop looking at it.
/// </summary>
public class WatchGestureController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Child transform parented to the back of the hand. Its facing direction (see " +
        "Panel Normal Axis below) is treated as the watch face's normal.")]
    [SerializeField] private Transform watchPanel;
    [SerializeField] private ToggleTextSimple instructionsPopup;
    [Tooltip("Defaults to Camera.main if left unassigned.")]
    [SerializeField] private Transform headCamera;

    [Header("Gesture Tuning")]
    [Tooltip("Which local axis of the watch panel is its front-facing normal. Depends on how the " +
        "panel mesh/quad was authored - flip this if the gesture triggers backwards.")]
    [SerializeField] private Vector3 panelNormalAxis = Vector3.forward;

    [Tooltip("How close to directly facing the camera the panel's normal needs to be, in degrees.")]
    [SerializeField] private float maxViewAngle = 40f;

    [Tooltip("How close the panel needs to be to the camera to count as 'raised to look at', in meters.")]
    [SerializeField] private float maxViewDistance = 0.6f;

    private bool watchBeingViewed = false;
    private bool instructionsStateBeforeGesture = false;

    void Reset()
    {
        headCamera = Camera.main != null ? Camera.main.transform : null;
    }

    void Update()
    {
        if (watchPanel == null || instructionsPopup == null)
            return;

        Transform cam = headCamera != null ? headCamera : (Camera.main != null ? Camera.main.transform : null);
        if (cam == null)
            return;

        bool lookingAtWatch = IsLookingAtWatch(cam);

        if (lookingAtWatch && !watchBeingViewed)
        {
            instructionsStateBeforeGesture = instructionsPopup.IsTextVisible();
            instructionsPopup.ShowText();
            watchBeingViewed = true;
        }
        else if (!lookingAtWatch && watchBeingViewed)
        {
            if (!instructionsStateBeforeGesture)
                instructionsPopup.HideText();
            watchBeingViewed = false;
        }
    }

    private bool IsLookingAtWatch(Transform cam)
    {
        Vector3 panelToCamera = cam.position - watchPanel.position;
        float distance = panelToCamera.magnitude;

        if (distance > maxViewDistance)
            return false;

        Vector3 panelNormalWorld = watchPanel.TransformDirection(panelNormalAxis.normalized);
        float angle = Vector3.Angle(panelNormalWorld, panelToCamera);

        return angle <= maxViewAngle;
    }
}
