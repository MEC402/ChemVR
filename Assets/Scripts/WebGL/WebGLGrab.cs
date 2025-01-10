using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WebGLGrab : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] WebGLInput webGLInput;
    [SerializeField] Transform holdPoint;
    [SerializeField] Image playerIcon;
    [SerializeField] Sprite defaultIcon, openIcon, closedIcon;

    [Header("Settings")]
    [Tooltip("How far away can the player grab objects using a raycast")]
    [SerializeField] float grabRange = 2f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask holdableLayer;

    bool canInteract = false;
    bool canGrab = false;
    bool isHoldingObject = false;

    Transform heldObject = null;
    Ray centerRay;
    #endregion

    #region Unity Methods
    private void Update()
    {
        MoveObjectToHoldPoint();

        // Lock out interactions if holding an object
        if (isHoldingObject)
        {
            playerIcon.sprite = closedIcon;

            ReleaseHeldObject();

            return;
        }

        CheckRaycast();

        ObjectGrab();
        ObjectInteract();
    }
    #endregion

    #region Custom Methods
    private void CheckRaycast()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        // Create a ray from the center of the screen
        centerRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        // Perform the raycast
        if (Physics.Raycast(centerRay, out RaycastHit hit, grabRange))
        {
            if (((1 << hit.collider.gameObject.layer) & interactableLayer) != 0)
            {
                canInteract = true;
                playerIcon.sprite = openIcon;
            }
            else if (((1 << hit.collider.gameObject.layer) & holdableLayer) != 0)
            {
                canGrab = true;
                playerIcon.sprite = openIcon;
            }
            else
                ResetIconAndFlags();
        }
        else
            ResetIconAndFlags();
    }

    private void ResetIconAndFlags()
    {
        canInteract = false;
        canGrab = false;
        playerIcon.sprite = defaultIcon;
    }

    private void ObjectGrab()
    {
        if (!canGrab || isHoldingObject) return;

        if (webGLInput.isInteracting)
        {
            if (Physics.Raycast(centerRay, out RaycastHit hit, grabRange))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Holdable"))
                {
                    heldObject = hit.collider.transform;
                    isHoldingObject = true;

                    Debug.Log("Interacting");

                    // Disable physics on the held object
                    Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.isKinematic = true;

                    // Reset parent and position later via lerp
                    heldObject.SetParent(holdPoint);
                    heldObject.position = holdPoint.position;

                    // Update icon to show "holding"
                    playerIcon.sprite = closedIcon;
                }
            }
        }
    }

    private void MoveObjectToHoldPoint()
    {
        if (heldObject == null) return;

        // Directly set the position and rotation of the held object to the hold point
        heldObject.position = holdPoint.position;
        heldObject.rotation = holdPoint.rotation;
    }

    private void ObjectInteract()
    {
        if (!canInteract || isHoldingObject) return;

        if (webGLInput.isInteracting)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabRange))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    //TODO: Implement interaction logic here
                }
            }
        }
    }

    public void ReleaseHeldObject()
    {
        if (heldObject == null) return;

        if (!webGLInput.isInteracting) return;

        // Enable physics back on the held object
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        // Detach the object and clear reference
        heldObject.SetParent(null);
        heldObject = null;

        isHoldingObject = false;

        // Reset icon
        playerIcon.sprite = defaultIcon;
    }
    #endregion
}