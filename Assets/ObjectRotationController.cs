using UnityEngine;

public class ObjectRotationController : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] WebGLInput inputScript;
    [SerializeField] Camera playerCamera; // Reference to the camera for relative rotation

    [HideInInspector] public Transform objectToRotate;

    [Header("Rotation Settings")]
    [SerializeField] float rotationSpeed = 100f;
    #endregion

    #region Unity Methods
    private void Update()
    {
        HandleObjectRotation();
    }
    #endregion

    #region Custom Methods
    private void HandleObjectRotation()
    {
        if (inputScript == null || objectToRotate == null || playerCamera == null) return;

        if (inputScript.isRotating)
        {
            inputScript.LookDisable();
            inputScript.MoveDisable();

            Vector2 input = inputScript.rotationInput;
            if (input != Vector2.zero)
            {
                float deltaTime = Time.deltaTime;
                float yaw = -input.x * rotationSpeed * deltaTime;
                float pitch = input.y * rotationSpeed * deltaTime;

                // Get camera-relative axes
                Vector3 camRight = playerCamera.transform.right;
                Vector3 camUp = playerCamera.transform.up;

                // Apply rotation around camera's right (pitch) and up (yaw)
                Quaternion pitchRotation = Quaternion.AngleAxis(pitch, camRight);
                Quaternion yawRotation = Quaternion.AngleAxis(yaw, camUp);

                Quaternion combinedRotation = yawRotation * pitchRotation;

                objectToRotate.rotation = combinedRotation * objectToRotate.rotation;
            }
        }
        else
        {
            inputScript.LookEnable();
            inputScript.MoveEnable();
        }
    }
    #endregion
}
