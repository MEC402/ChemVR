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
            inputScript.LookDisable(); //move the disables to a seperate check. if you hit r but it runs the rest of the code, it may not reenable the look/move. add an enable to the end of the R key on glassware rotation?
            inputScript.MoveDisable();

            Vector2 input = inputScript.rotationInput;

            //write an if statement to check to see if WebGLInput  public bool isInteracting = true; if true && the obj has a chem container script, then rotate around X or Z axis
            //WebGlGrab heldObject = hit.collider.transform; stores the game obj being held. can we use this to ref the collider?
            //in WebGL Grab there is also bool isHoldingObject = false; incase the WebGLInput wont work.
            //sink rotate on Y to 270



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
//objectToRotate.localScale = new Vector3(objectToRotate.localScale.x, -objectToRotate.localScale.y, objectToRotate.localScale.z);
        }
        else
        {
            inputScript.LookEnable();
            inputScript.MoveEnable();
        }
    }
    #endregion
}
