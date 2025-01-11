using UnityEngine;
using UnityEngine.UI;

public class WebGLGrab : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] WebGLInput webGLInput;
    [SerializeField] ObjectRotationController objectRotationController;
    [SerializeField] Transform holdPoint;
    [SerializeField] Image playerIcon;
    [SerializeField] Sprite defaultIcon;
    [SerializeField] Sprite openIcon;
    [SerializeField] Sprite closedIcon;

    [Header("Settings")]
    [Tooltip("How far away can the player grab objects using a raycast")]
    [SerializeField] float grabRange = 2f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask holdableLayer;

    bool isHoldingObject = false;
    Transform heldObject = null;

    Camera mainCamera;
    Ray centerRay;
    #endregion

    #region Unity Methods
    private void Start()
    {
        mainCamera = Camera.main;

        if (!mainCamera)
            Debug.LogError("Main camera not found!");
    }

    private void OnEnable()
    {
        webGLInput.OnInteractPressed += HandleInteractPressed;
        webGLInput.OnInteractReleased += HandleInteractReleased;
    }

    private void OnDisable()
    {
        webGLInput.OnInteractPressed -= HandleInteractPressed;
        webGLInput.OnInteractReleased -= HandleInteractReleased;
    }

    private void Update()
    {
        // If we're holding an object, keep it aligned to the holdPoint
        if (isHoldingObject)
        {
            MoveObjectToHoldPoint();
            playerIcon.sprite = closedIcon;
        }
        else
            CheckRaycast(); // Not holding anything, so keep checking the raycast to set the player's icon for potential interactions
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Called once when the Interact button is pressed.
    /// </summary>
    private void HandleInteractPressed()
    {
        if (isHoldingObject)
            AttemptRelease();
        else
            AttemptGrab();
    }

    /// <summary>
    /// Called once when the Interact button is released.
    /// </summary>
    private void HandleInteractReleased()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Checks if there's an interactable or holdable object in front of the player and updates the UI icon accordingly.
    /// </summary>
    private void CheckRaycast()
    {
        // Default icon if nothing is detected
        playerIcon.sprite = defaultIcon;

        if (!mainCamera) return;

        // Create a ray from the center of the screen
        centerRay = mainCamera.ScreenPointToRay(
            new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(centerRay, out RaycastHit hit, grabRange))
        {
            if (((1 << hit.collider.gameObject.layer) & interactableLayer) != 0)
                playerIcon.sprite = openIcon;
            else if (((1 << hit.collider.gameObject.layer) & holdableLayer) != 0)
                playerIcon.sprite = openIcon;
            else
                playerIcon.sprite = defaultIcon;
        }
    }

    /// <summary>
    /// Called once when the interact button is pressed and 
    /// we are not currently holding an object.
    /// </summary>
    private void AttemptGrab()
    {
        if (!mainCamera) return;

        // Fire a ray from the center of the screen
        centerRay = mainCamera.ScreenPointToRay(
            new Vector2(Screen.width / 2f, Screen.height / 2f));

        // Check if we hit a holdable object
        if (Physics.Raycast(centerRay, out RaycastHit hit, grabRange, holdableLayer))
        {
            heldObject = hit.collider.transform;
            isHoldingObject = true;

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();

            if (rb != null)
                rb.isKinematic = true;  // Disable physics while holding

            // Parent the object to the hold point
            heldObject.SetParent(holdPoint);

            // Snap to hold point
            heldObject.position = holdPoint.position;
            heldObject.rotation = holdPoint.rotation;

            objectRotationController.objectToRotate = heldObject;

            playerIcon.sprite = closedIcon;
        }
    }

    /// <summary>
    /// Called once when the interact button is pressed and if we are already holding an object.
    /// </summary>
    private void AttemptRelease()
    {
        if (!isHoldingObject || heldObject == null || webGLInput.isRotating) return;

        // Enable physics back on the held object
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();

        if (rb != null)
            rb.isKinematic = false;

        // Detach the object
        heldObject.SetParent(null);
        heldObject = null;
        isHoldingObject = false;

        objectRotationController.objectToRotate = null;

        // Update the icon
        playerIcon.sprite = defaultIcon;
    }

    /// <summary>
    /// Keeps the held object in the hold position every frame.
    /// </summary>
    private void MoveObjectToHoldPoint()
    {
        if (!heldObject) return;

        heldObject.position = holdPoint.position;

        if (!webGLInput.isRotating)
            heldObject.rotation = holdPoint.rotation;
    }
    #endregion
}
