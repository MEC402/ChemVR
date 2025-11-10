using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class JarLid : MonoBehaviour
{
    #region Variables
    [SerializeField] GameObject parentJar;

    [SerializeField] Rigidbody jarRB;
    [SerializeField] Rigidbody lidRB;

    private FixedJoint joint;

    private bool isHeld = false;

    Quaternion initialRotation; //initial rotation of the lid
    Vector3 initialPos; //initial position of the lid

    private XRGrabInteractable grabInteractable; //XRGrabInteractable of attached gameObject

    bool touching;
    #endregion

    #region Unity Methods
    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        initialRotation = transform.localRotation;
        //initialPos = transform.localPosition;
        initialPos = new Vector3(0.00f, 0.00f, 0.0292f);
        // Vector3(0, 0, 0.03391998);

        UsesGravity(false);
        lidRB = this.gameObject.GetComponent<Rigidbody>();

    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // WebGL listeners
        GameEventsManager.instance.webGLEvents.OnObjectGrabbed += WebGLGrab;
        GameEventsManager.instance.webGLEvents.OnObjectReleased += WebGLRelease;
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);

        // WebGL listeners
        GameEventsManager.instance.webGLEvents.OnObjectGrabbed -= WebGLGrab;
        GameEventsManager.instance.webGLEvents.OnObjectReleased -= WebGLRelease;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parentJar)
        {
            touching = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == parentJar)
        {
            touching = false;
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Handles the grab of the lid for VR controls. If it is grabbed, it will use gravity. If it is touching the jar, it will not use gravity.
    /// </summary>
    /// <param name="arg0">The event arguments for the grab event.</param>
    private void OnGrab(SelectEnterEventArgs arg0)
    {
        UsesGravity(true);
    }

    /// <summary>
    /// Handles the release of the lid for VR controls. If it is released, it will not use gravity. If it is touching the jar, it will not use gravity.
    /// </summary>
    /// <param name="arg0">The event arguments for the release event.</param>
    private void OnRelease(SelectExitEventArgs arg0)
    {
        if (touching)
        {
            UsesGravity(false);
        }
        else
        {
            UsesGravity(true);
        }
    }

    /// <summary>
    /// Handles the grab of the lid for WebGL controls. If it is grabbed, it will use gravity. If it is touching the jar, it will not use gravity.
    /// </summary>
    /// <param name="grabbedObject">The object that was grabbed.</param>
    private void WebGLGrab(GameObject grabbedObject)
    {
        if (grabbedObject == gameObject)
        {
            UsesGravity(true);
            isHeld = true;
            GameEventsManager.instance.inputEvents.onRTriggerPressed += ResetLid;
        }

    }

    /// <summary>
    /// Handles the release of the lid for WebGL controls. If it is released, it will not use gravity. If it is touching the jar, it will not use gravity.
    /// </summary>
    /// <param name="releasedObject">The object that was released.</param>
    private void WebGLRelease(GameObject releasedObject)
    {
        if (releasedObject == gameObject)
        {
            if (touching)
            {
                 UsesGravity(false);
             }
            else
            {
            UsesGravity(true);
         }
            isHeld = false;
            GameEventsManager.instance.inputEvents.onRTriggerPressed -= ResetLid;
        }
    }

    /// <summary>
    /// Handles the gravity of the lid. If it is grabbed, it will use gravity. If it is not grabbed and touching the jar, it will not use gravity.
    /// Will also reset the scale of the lid to 5,5,5 if it has changed, and reset the position and rotation of the lid to its initial position and rotation.
    /// </summary>
    /// <param name="useGravity">Whether or not the lid should use gravity.</param>
    private void UsesGravity(bool useGravity)
    {
        if (TryGetComponent<Rigidbody>(out var rb))
            rb.useGravity = useGravity;

        /* // Reset scale if it has changed
         if (transform.localScale != new Vector3(5, 5, 5))
             transform.localScale = new Vector3(5, 5, 5);*/

        if (!useGravity)
        {
            // Reset the parent object
            transform.SetParent(parentJar.transform);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.SetLocalPositionAndRotation(initialPos, initialRotation);

            joint = lidRB.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = jarRB;



            // Freeze pos and rotation

        }
        else
        {
            //rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
            transform.SetParent(null);
            if (joint != null)
            {
                Destroy(joint);
                joint = null;
            }

        }

        // Trigger the jar closed event
        GameEventsManager.instance.miscEvents.JarClosed(parentJar, !useGravity);
    }
    


    public void ResetLid(InputAction.CallbackContext context)
    {
        if (isHeld)
        {
            GameObject player = GameObject.Find("FP Player");
            if (player != null)
            {
                WebGLGrab webGrab = player.GetComponent<WebGLGrab>();
                if (webGrab != null)
                {
                    webGrab.ForceReleaseObject();
                    UsesGravity(false);
                    isHeld = false;
                    GameEventsManager.instance.inputEvents.onRTriggerPressed -= ResetLid;
                }
            }

        }
    }
    #endregion
}
