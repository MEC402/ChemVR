using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] bool movePos;
    [SerializeField] Vector3 openPos;
    [SerializeField] float rotationAngle;
    [SerializeField] float openSpeed = 1.5f;
    Vector3 startingPos;
    float startingRot;

    bool isOpen;

    private void Start()
    {
        startingPos = transform.position;
        startingRot = transform.rotation.eulerAngles.y;
    }

    public void ToggleOpen()
    {
        StopAllCoroutines();

        if (isOpen)
            StartCoroutine(MoveDoor(startingPos));
        else
            StartCoroutine(MoveDoor(openPos));

        isOpen = !isOpen;
    }

    IEnumerator MoveDoor(Vector3 targetPos)
    {
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
}
