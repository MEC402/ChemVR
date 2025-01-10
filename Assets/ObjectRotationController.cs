using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] WebGLInput inputScript; // Reference to the input script managing player inputs.

    public Transform objectToRotate; // The object that will be rotated.

    [Header("Rotation Settings")]
    [SerializeField] float rotationSpeed = 100f; // Speed at which the object rotates.

    /// <summary>
    /// Monitors player input and applies rotation to the object when appropriate.
    /// </summary>
    private void Update()
    {
        HandleObjectRotation();
    }

    /// <summary>
    /// Rotates the specified object based on the player's mouse movement.
    /// Only rotates the object when the player is actively rotating and providing input.
    /// </summary>
    private void HandleObjectRotation()
    {
        // Exit if there are missing references.
        if (inputScript == null || objectToRotate == null) return;

        // Rotate the object when the player is actively rotating and providing look input.
        if (inputScript.isRotating && inputScript.rotationInput != Vector2.zero)
        {
            inputScript.LookDisable();
            // Rotate around the world's up axis for horizontal input (yaw).
            objectToRotate.Rotate(Vector3.up, -inputScript.rotationInput.x * rotationSpeed * Time.deltaTime, Space.World);

            // Rotate around the object's local right axis for vertical input (pitch).
            objectToRotate.Rotate(Vector3.right, inputScript.rotationInput.y * rotationSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            inputScript.LookEnable();
        }
    }
}
