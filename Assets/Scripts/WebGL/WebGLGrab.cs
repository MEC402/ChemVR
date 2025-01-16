using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class WebGLGrab : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] WebGLInput webGLInput;
    [SerializeField] HygieneManager hygieneManager;
    [SerializeField] ObjectRotationController objectRotationController;
    [SerializeField] Transform holdPoint;
    [SerializeField] Image playerIcon;
    [SerializeField] Sprite defaultIcon, openIcon, closedIcon, interactIcon;

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

    #region Input Handling
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
        //TODO: Implement if needed
    }
    #endregion

    #region Interaction Logic
    /// <summary>
    /// Checks if there's an interactable or holdable object in front of the player and updates the UI icon accordingly.
    /// </summary>
    private void CheckRaycast()
    {
        // Default icon if nothing is detected
        playerIcon.sprite = defaultIcon;

        if (!mainCamera) return;

        // Create a ray from the center of the screen
        centerRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(centerRay, out RaycastHit hit, grabRange, ~0, QueryTriggerInteraction.Ignore))
        {
            if (((1 << hit.collider.gameObject.layer) & interactableLayer) != 0)
                playerIcon.sprite = interactIcon;
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
        if (!mainCamera || webGLInput.isPaused) return;

        // Fire a ray from the center of the screen
        centerRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        RaycastHit hit; // Store the hit object

        // Hygiene check if gloves are worn
        if (ActiveItemsCanvas.Instance.isWearingGloves)
        {
            if (Physics.Raycast(centerRay, out hit, grabRange, ~0, QueryTriggerInteraction.Ignore))
            {
                Vector3 touchPoint = hit.point;
                hygieneManager.AddPoint(touchPoint, hit.collider.gameObject);
            }
        }

        if (Physics.Raycast(centerRay, out hit, grabRange, ~0, QueryTriggerInteraction.Ignore))
        {
            // Check if the hit object is in the holdable layer
            if (((1 << hit.collider.gameObject.layer) & holdableLayer) != 0)
            {
                ProcessHoldableObject(hit);
                return;
            }

            // Check if the hit object is in the interactable layer
            if (((1 << hit.collider.gameObject.layer) & interactableLayer) != 0)
            {
                ProcessInteractableObject(hit);
                return;
            }
        }
    }

    /// <summary>
    /// Called once when the interact button is pressed and if we are already holding an object.
    /// </summary>
    private void AttemptRelease()
    {
        // if (!isHoldingObject || heldObject == null || webGLInput.isRotating) return;
        if (!isHoldingObject || heldObject == null) return;

        if (webGLInput.isRotating)
        {
            webGLInput.isRotating = false;

            webGLInput.LookEnable();
            webGLInput.MoveEnable();
        }

        // Enable physics back on the held object
        if (heldObject.TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = false;

        // Detach the object
        heldObject.SetParent(null);

        if (heldObject.TryGetComponent(out XRBaseInteractable xrInteractable))
            xrInteractable.selectExited.Invoke(new SelectExitEventArgs());

        heldObject = null;
        isHoldingObject = false;

        objectRotationController.objectToRotate = null;

        // Update the icon
        playerIcon.sprite = defaultIcon;
    }
    #endregion

    #region Object Handling
    /// <summary>
    /// Processes a holdable object once confirmed it's valid.
    /// </summary>
    /// <param name="hit">The RaycastHit from the raycast.</param>
    private void ProcessHoldableObject(RaycastHit hit)
    {
        heldObject = hit.collider.transform;
        isHoldingObject = true;

        if (heldObject.TryGetComponent<Rigidbody>(out var rb))
        {
            // Disable any object rotation/force
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;

            rb.isKinematic = true; // Disable physics while holding
        }

        // Parent the object to the hold point
        heldObject.SetParent(holdPoint);

        if (heldObject.TryGetComponent(out XRBaseInteractable xrInteractable))
            xrInteractable.selectEntered.Invoke(new SelectEnterEventArgs());

        // Snap to hold point
        heldObject.SetPositionAndRotation(holdPoint.position, holdPoint.rotation);
        objectRotationController.objectToRotate = heldObject;

        playerIcon.sprite = closedIcon;
    }

    /// <summary>
    /// Processes an interactable object once confirmed it's valid.
    /// </summary>
    /// <param name="hit">The RaycastHit from the raycast.</param>
    private void ProcessInteractableObject(RaycastHit hit)
    {
        var hitObject = hit.collider.gameObject;
        if (hitObject.TryGetComponent(out WearGoggles wearGoggles))
            wearGoggles.WebPutOn();
        else if (hitObject.TryGetComponent(out WearCoat wearCoat))
            wearCoat.WebPutOn();
        else if (hitObject.TryGetComponent(out AddGloves addGloves))
            addGloves.WebPutOnLeftGloves();
        else if (hitObject.TryGetComponent(out PrinterSlap printerSlap))
            GameEventsManager.instance.miscEvents.PrinterSlap();
        else if (hitObject.TryGetComponent(out RemoveGloves removeGloves))
            removeGloves.WebTakeOffGloves();
        else if (hitObject.TryGetComponent(out DoorOpen doorOpen))
            doorOpen.ToggleOpen();
        else if (hitObject.TryGetComponent(out StopCockController _))
            StopCockAdjuster(hitObject);
    }


    /// <summary>
    /// Adjusts the stop cock by toggling the hinge and flow.
    /// </summary>
    /// <param name="stopCock">The stop cock to adjust</param>
    private void StopCockAdjuster(GameObject stopCock)
    {
        stopCock.GetComponent<StopCockController>().WebToggleHinge();

        stopCock.GetComponentInParent<RotatingValveController>().CalculateFlow(stopCock.transform.localEulerAngles.z);
    }

    /// <summary>
    /// Keeps the held object in the hold position every frame.
    /// </summary>
    private void MoveObjectToHoldPoint()
    {
        if (!heldObject) return;

        heldObject.position = holdPoint.position;
    }
    #endregion
}
