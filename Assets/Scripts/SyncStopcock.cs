using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SyncStopcock : MonoBehaviour
{
    public GameObject collisionDetector; //stopcock collider
    public GameObject turner;
    public GameObject valve;
    private XRGrabInteractable grabInteractable;

    private void OnEnable()
    {
        grabInteractable = collisionDetector.GetComponent<XRGrabInteractable>();

        //Initialize grab interactable for listening
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing.");
            return;
        }
         /*
        collisionDetector.transform.localRotation = Quaternion.Euler(0, 90, 0);
        turner.transform.localRotation = Quaternion.Euler(0, 0, 90);
        valve.transform.localRotation = Quaternion.Euler(0, 0, 90);
         */
    }


    void Update()
    {
        // Get the current rotation of Door 1
        float colliderRotation = collisionDetector.transform.localEulerAngles.z;

        // Apply the same rotation to Door 2 and Door 3
        turner.transform.localRotation = Quaternion.Euler(0, 0, -colliderRotation);
        valve.transform.localRotation = Quaternion.Euler(0, 0, -colliderRotation + 90);
    }
}
