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
    XRBaseInteractable xrInteractable;

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
        //TODO: Implement if needed
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
        centerRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(centerRay, out RaycastHit hit, grabRange))
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
        if (!mainCamera) return;

        // Fire a ray from the center of the screen
        centerRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));



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

            xrInteractable = heldObject.GetComponent<XRBaseInteractable>();
            xrInteractable.selectEntered.Invoke(new SelectEnterEventArgs());

            // Snap to hold point
            heldObject.position = holdPoint.position;
            heldObject.rotation = holdPoint.rotation;

            objectRotationController.objectToRotate = heldObject;

            playerIcon.sprite = closedIcon;
        }
        else if (Physics.Raycast(centerRay, out hit, grabRange, interactableLayer))
        {
            if (hit.collider.gameObject.GetComponent<WearGoggles>())
                hit.collider.gameObject.GetComponent<WearGoggles>().WebPutOn();
            else if (hit.collider.gameObject.GetComponent<WearCoat>())
                hit.collider.gameObject.GetComponent<WearCoat>().WebPutOn();
            else if (hit.collider.gameObject.GetComponent<AddGloves>())
                hit.collider.gameObject.GetComponent<AddGloves>().WebPutOnLeftGloves();
            else if (hit.collider.gameObject.GetComponent<PrinterSlap>())
                GameEventsManager.instance.miscEvents.PrinterSlap();
            else if (hit.collider.gameObject.GetComponent<RemoveGloves>())
                hit.collider.gameObject.GetComponent<RemoveGloves>().WebTakeOffGloves();
            else if (hit.collider.gameObject.GetComponent<DoorOpen>())
                hit.collider.gameObject.GetComponent<DoorOpen>().ToggleOpen();
        }

        if (!ActiveItemsCanvas.Instance.isWearingGloves)
            return;

        if (Physics.Raycast(centerRay, out hit, grabRange))
        {
            Vector3 touchPoint = hit.point;
            hygieneManager.AddPoint(touchPoint, hit.collider.gameObject);
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

        xrInteractable = heldObject.GetComponent<XRBaseInteractable>();
        xrInteractable.selectExited.Invoke(new SelectExitEventArgs());

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
    }
    #endregion
}
