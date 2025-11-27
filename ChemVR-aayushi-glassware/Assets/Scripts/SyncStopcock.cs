using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SyncStopcock : MonoBehaviour
{
    public GameObject collisionDetector; //stopcock collider
    public GameObject turner;
    public GameObject valve;
    public float offset = 90;
    private XRGrabInteractable grabInteractable;

    //float OGturnerXRotation;
    //float OGturnerYRotation;
    private void OnEnable()
    {
        grabInteractable = collisionDetector.GetComponent<XRGrabInteractable>();
        offset = 90;
        //Initialize grab interactable for listening
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
            return;
        }

        //OGturnerXRotation = turner.transform.localEulerAngles.x;
        //OGturnerYRotation = turner.transform.localEulerAngles.y;
    }


    void Update()
    {
        // Get the current rotation of Door 1
        float colliderRotation = collisionDetector.transform.localEulerAngles.z;

        // Apply the same rotation to Door 2 and Door 3
        turner.transform.localRotation = Quaternion.Euler(0, 0, colliderRotation + offset);
        valve.transform.localRotation = Quaternion.Euler(0, 0, colliderRotation);

        // fix other rotations of collider
        //collisionDetector.transform.localRotation = Quaternion.Euler(OGturnerXRotation, OGturnerYRotation, colliderRotation);
    }
}
