using UnityEngine;

public class ObjectRotationController : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] WebGLInput inputScript;
    [SerializeField] Camera playerCamera; // Reference to the camera for relative rotation

    [HideInInspector] public Transform objectToRotate;
    [HideInInspector] public string objectToRotateName;
    public Transform myLocalRotateObj;

    [Header("Rotation Settings")]
    [SerializeField] float rotationSpeed = 100f;
        static bool isFlip = false;
    #endregion

    #region Unity Methods
    private void Update()
    {
        HandleObjectRotation();
        if (Input.GetKeyDown(KeyCode.T))
        {
            TRotation();
        }
    }
    #endregion

    #region Custom Methods
     public static bool IsRunningOnWebGL()
     {
         return Application.platform == RuntimePlatform.WebGLPlayer;
     }


    public void ReceiveHeldObject(Transform obj)
    {
        myLocalRotateObj = obj;
        Debug.Log("Got object: " + obj.name);
        objectToRotateName = obj.name;
        // now you can store it and rotate as needed
    }

    public bool TRotation()
    {
        bool aresult = false;
        if (objectToRotateName.ToLower().Contains("beaker"))
        /*        write an if statement to check to see if WebGLInput  public bool isInteracting = true; if true && the obj has a chem container script, then rotate around X or Z axis
                WebGlGrab heldObject = hit.collider.transform; stores the game obj being held.can we use this to ref the collider?
                in WebGL Grab there is also bool isHoldingObject = false; incase the WebGLInput wont work.
                sink rotate on Y to 270 (add 90 to Y)
                DI rotate on X to 0 from -90
                flask/soap rotate on X to -90 from 0
                Grad Cylinder X to -180 from -90
                beaker z to 90 from 0
                copper sulfate x to 0 from -90

                add 90 to y : sink
                add 90 to Z : beaker
                substract 90 (add -90) to X : DI rotate, flask, grad cylinder, copper sulfate*/

        {
            Debug.Log("add 90 to z axis");

            Vector3 rot = myLocalRotateObj.localEulerAngles;   // current rotation (x, y, z)
            rot.z += 90f;                           // add to X (10° for example)
            myLocalRotateObj.localEulerAngles = rot;           // apply it back
	    aresult = true;
	    return aresult;

        }
        if (objectToRotateName.ToLower().Contains("solid"))
	{
            Debug.Log("add -90 to x axis");

            Vector3 rot = myLocalRotateObj.localEulerAngles;   // current rotation (x, y, z)
            rot.x = -90f;                           // add to X (10° for example)
            //rot.y = -90f;                           // add to X (10° for example)
            rot.z = -90f;                           // add to X (10° for example)
            myLocalRotateObj.localEulerAngles = rot;           // apply it back
	    aresult = true;
	    return aresult;
        }

        if (objectToRotateName.ToLower().Contains("flask")
	     || objectToRotateName.ToLower().Contains("soap")
             || objectToRotateName.ToLower().Contains("graduated")
	     || objectToRotateName.ToLower().Contains("di"))
        {
            Debug.Log("add -90 to x axis");

            Vector3 rot = myLocalRotateObj.localEulerAngles;   // current rotation (x, y, z)
            rot.x -= 90f;                           // add to X (10° for example)
            myLocalRotateObj.localEulerAngles = rot;           // apply it back
	    aresult = true;
        }
        if (objectToRotateName.ToLower().Contains("sink"))
        {
            Debug.Log("add 90 to y axis");

            Vector3 rot = myLocalRotateObj.localEulerAngles;   // current rotation (x, y, z)
            rot.y += 90f;                           // add to X (10° for example)
            myLocalRotateObj.localEulerAngles = rot;           // apply it back
	    aresult = true;
        }
	return aresult;
    }
    private void HandleObjectRotation()
    {
        if (inputScript == null || objectToRotate == null || playerCamera == null) return;

        if (inputScript.isRotating)
        {

            Debug.Log("Else completed when you hit R");
            inputScript.LookDisable(); //move the disables to a seperate check. if you hit r but it runs the rest of the code, it may not reenable the look/move. add an enable to the end of the R key on glassware rotation?
            inputScript.MoveDisable();

	  // do specific rotations for specific objects
          /*if ( IsRunningOnWebGL())
	  {
	    //if (!isFlip) 
	    {
              if (TRotation()) {
	      isFlip = true;
	      inputScript.isRotating = false;
              return;
	      }
	    }
	  }
	  */

            Vector2 input = inputScript.rotationInput;

            if (input != Vector2.zero)
            {


                Debug.Log("input != completed when you hit R");

                //Debug.Log("Else completed when you hit R");
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
	    isFlip = false;
        }
    }
    #endregion
}
