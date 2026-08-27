using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Stands an object back up after it is dropped. Once the rigidbody has come to rest
/// against something, the object is rotated by the smallest amount that puts its "up"
/// axis back towards the ceiling, so its yaw (the way the player was facing it) is kept.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class UprightOnLand : MonoBehaviour
{
    public enum UprightReference
    {
        StartingRotation,   //however the object was placed in the scene IS upright for it
        ModelUpAxis,        //use the hand-entered axis below
    }

    [Header("What counts as upright")]
    [Tooltip("StartingRotation reads the pose the object was placed at in the scene, which works no matter which way the mesh was authored. Only use ModelUpAxis if the object is placed lying down but should stand up.")]
    [SerializeField] private UprightReference uprightFrom = UprightReference.StartingRotation;
    [Tooltip("Only used with ModelUpAxis: the axis of THIS model that should point at the ceiling.")]
    [SerializeField] private Vector3 localUpAxis = Vector3.up;
    [Tooltip("Tilts smaller than this (degrees) are left alone.")]
    [SerializeField] private float ignoreTiltBelow = 3f;

    [Header("When to correct")]
    [Tooltip("Only correct after the player drops it. Off = also stands it back up if something else knocks it over.")]
    [SerializeField] private bool onlyAfterRelease = true;
    [Tooltip("Speed (m/s) the object must be under to count as landed.")]
    [SerializeField] private float settleSpeed = 0.08f;
    [Tooltip("Angular speed (rad/s) the object must be under to count as landed.")]
    [SerializeField] private float settleAngularSpeed = 0.6f;
    [Tooltip("How long it has to sit still, touching something, before it is corrected.")]
    [SerializeField] private float settleTime = 0.25f;

    [Header("How to correct")]
    [Tooltip("Seconds the correction takes. 0 snaps instantly.")]
    [SerializeField] private float rightingDuration = 0.25f;
    [Tooltip("Keep the bottom of the object at the same height while it rotates, so it doesn't sink into the table.")]
    [SerializeField] private bool keepBottomHeight = true;

    private Rigidbody myRb;
    private XRGrabInteractable grabInteractable;
    private SnapToTray snapToTray;
    private Collider[] myColliders;

    private int contactCount;       //how many things we are resting against
    private float slowTimer;        //how long we have been slow AND touching
    private bool watching;          //released and waiting to land
    private bool righting;          //correction in progress

    private void Awake()
    {
        myRb = GetComponent<Rigidbody>();
        snapToTray = GetComponent<SnapToTray>();
        myColliders = GetComponentsInChildren<Collider>();

        if (uprightFrom == UprightReference.StartingRotation) CaptureUprightFromCurrentRotation();

        if (localUpAxis == Vector3.zero) localUpAxis = Vector3.up;
        localUpAxis.Normalize();

        watching = !onlyAfterRelease;
    }

    private void OnEnable()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
        contactCount = 0;
        slowTimer = 0f;
    }

    //WebGLGrab invokes selectEntered/selectExited with empty args, so never read from them here.
    private void OnGrab(SelectEnterEventArgs args)
    {
        watching = false;
        slowTimer = 0f;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        watching = true;
        slowTimer = 0f;
    }

    private void FixedUpdate()
    {
        if (righting || !watching) return;

        //Don't fight anything that is holding the transform itself. WebGLGrab parents the object
        //to the hold point and makes it kinematic; XRI does the same for kinematic grab movement.
        if (myRb.isKinematic) { slowTimer = 0f; return; }
        if (grabInteractable != null && grabInteractable.isSelected) { slowTimer = 0f; return; }
        if (snapToTray != null && snapToTray.GetIsSnapped()) { slowTimer = 0f; return; }

        bool landed = contactCount > 0 &&
                      myRb.velocity.magnitude < settleSpeed &&
                      myRb.angularVelocity.magnitude < settleAngularSpeed;

        if (!landed) { slowTimer = 0f; return; }

        slowTimer += Time.fixedDeltaTime;
        if (slowTimer < settleTime) return;

        slowTimer = 0f;
        if (Vector3.Angle(transform.rotation * localUpAxis, Vector3.up) < ignoreTiltBelow)
        {
            if (onlyAfterRelease) watching = false; //already standing, nothing to do
            return;
        }

        StartCoroutine(RightSelf());
    }

    private IEnumerator RightSelf()
    {
        righting = true;

        bool wasKinematic = myRb.isKinematic;
        myRb.velocity = Vector3.zero;
        myRb.angularVelocity = Vector3.zero;
        myRb.isKinematic = true;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = GetUprightRotation();
        float bottomY = GetBottomY();

        float elapsed = 0f;
        while (elapsed < rightingDuration)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / rightingDuration);
            if (keepBottomHeight) HoldBottomAt(bottomY);
            yield return null;
        }

        transform.rotation = targetRotation;
        if (keepBottomHeight) HoldBottomAt(bottomY);

        myRb.isKinematic = wasKinematic;
        myRb.velocity = Vector3.zero;
        myRb.angularVelocity = Vector3.zero;

        righting = false;
        if (onlyAfterRelease) watching = false;
    }

    /// <summary>
    /// Treats however the object is oriented right now as its upright pose, by recording which of
    /// its local axes is currently pointing at the ceiling. Meshes authored on their side, or with
    /// -Z up, come out correct without anyone entering an axis by hand. Call this again if an
    /// object is repositioned at runtime and its resting pose changes.
    /// </summary>
    public void CaptureUprightFromCurrentRotation()
    {
        localUpAxis = Quaternion.Inverse(transform.rotation) * Vector3.up;
    }

    /// <summary>
    /// The smallest rotation that puts localUpAxis back on world up. Spin about that axis is untouched,
    /// so the object keeps whatever heading the player left it at.
    /// </summary>
    private Quaternion GetUprightRotation()
    {
        Vector3 currentUp = transform.rotation * localUpAxis;
        return Quaternion.FromToRotation(currentUp, Vector3.up) * transform.rotation;
    }

    private float GetBottomY()
    {
        float bottom = float.MaxValue;
        foreach (Collider col in myColliders)
        {
            if (col == null || col.isTrigger || !col.enabled) continue;
            bottom = Mathf.Min(bottom, col.bounds.min.y);
        }
        return bottom == float.MaxValue ? transform.position.y : bottom;
    }

    private void HoldBottomAt(float bottomY)
    {
        float current = GetBottomY();
        transform.position += new Vector3(0f, bottomY - current, 0f);
    }

    private void OnCollisionEnter(Collision collision) => contactCount++;

    private void OnCollisionExit(Collision collision) => contactCount = Mathf.Max(0, contactCount - 1);
}
