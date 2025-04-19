using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] bool movePos;
    [SerializeField] AudioSource doorAudio;
    [SerializeField, Tooltip("The hinge joint that the door is attached to. Use this if the door is a hinge door.")]
    HingeJoint hingeJoint;

    [Header("Settings")]
    [SerializeField] Vector3 openPos;
    [SerializeField] float rotationAngle;
    [SerializeField] float openSpeed = 1.5f;

    private Vector3 startingPos;
    private float startingRot;
    private bool isOpen;
    private Transform hingePivot; // New variable for manual rotation handling

    #endregion

    #region Unity Methods
    private void Start()
    {
        startingPos = transform.localPosition;
        startingRot = transform.localRotation.eulerAngles.y;

        // If the object has a hinge joint, create an empty pivot object at the joint position
        if (hingeJoint != null)
        {
            hingePivot = new GameObject($"{gameObject.name}_HingePivot").transform;
            hingePivot.position = hingeJoint.connectedBody != null ?
                hingeJoint.connectedBody.transform.TransformPoint(hingeJoint.connectedAnchor) :
                hingeJoint.connectedAnchor;
            hingePivot.parent = transform.parent; // Keep it in the same hierarchy level

            // Make the door a child of the pivot so we can rotate it easily
            transform.parent = hingePivot;
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Toggles the door open and closed.
    /// </summary>
    public void ToggleOpen()
    {
        StopAllCoroutines();
        Debug.Log("ToggleOpen called");

        if (isOpen)
            StartCoroutine(MoveDoor(startingPos, startingRot));
        else
            StartCoroutine(MoveDoor(openPos, startingRot + rotationAngle));

        isOpen = !isOpen;
    }

    /// <summary>
    /// Moves the door to the target position. If movePos is true, the door will move to the target position. If movePos is false, the door will rotate to the target position.
    /// </summary>
    /// <param name="targetPos">The position to move the door to.</param>
    /// <param name="targetRot">The target rotation angle.</param>
    /// <returns></returns>
    IEnumerator MoveDoor(Vector3 targetPos, float targetRot)
    {
        if (doorAudio != null)
        {
            doorAudio.pitch = Random.Range(0.9f, 1.1f);
            doorAudio.Play();
        }

        if (movePos)
        {
            while (Vector3.Distance(transform.localPosition, targetPos) > 0.01f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, openSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            float currentRot = hingeJoint != null ? hingePivot.localRotation.eulerAngles.y : transform.localRotation.eulerAngles.y;

            while (Mathf.Abs(Mathf.DeltaAngle(currentRot, targetRot)) > 0.5f)
            {
                currentRot = Mathf.LerpAngle(currentRot, targetRot, openSpeed * Time.deltaTime);

                if (hingeJoint != null)
                {
                    hingePivot.localRotation = Quaternion.Euler(0, currentRot, 0);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0, currentRot, 0);
                }

                yield return null;
            }
        }
    }
    #endregion
}

