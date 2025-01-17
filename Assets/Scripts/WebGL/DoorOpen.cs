using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] bool movePos;
    [SerializeField] AudioSource doorAudio;

    [Header("Settings")]
    [SerializeField] Vector3 openPos;
    [SerializeField] float rotationAngle;
    [SerializeField] float openSpeed = 1.5f;

    Vector3 startingPos;
    float startingRot;

    bool isOpen;
    #endregion

    #region Unity Methods
    private void Start()
    {
        startingPos = transform.position;
        startingRot = transform.rotation.eulerAngles.y;
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Toggles the door open and closed.
    /// </summary>
    public void ToggleOpen()
    {
        StopAllCoroutines();

        if (isOpen)
            StartCoroutine(MoveDoor(startingPos));
        else
            StartCoroutine(MoveDoor(openPos));

        isOpen = !isOpen;
    }

    /// <summary>
    /// Moves the door to the target position. If movePos is true, the door will move to the target position. If movePos is false, the door will rotate to the target position.
    /// </summary>
    /// <param name="targetPos">The position to move the door to.</param>
    /// <returns></returns>
    IEnumerator MoveDoor(Vector3 targetPos)
    {
        doorAudio.pitch = Random.Range(0.9f, 1.1f);
        doorAudio.Play();

        if (movePos)
        {
            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, openSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            float targetRot = isOpen ? startingRot : startingRot + rotationAngle;

            while (Mathf.Abs(transform.rotation.eulerAngles.y - targetRot) > 0.01f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRot, 0), openSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
    #endregion
}
