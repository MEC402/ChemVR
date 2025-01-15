using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StopCockController : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    bool isLocked;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void Start()
    {
        LockHinge();
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        ReleaseHinge();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        LockHinge();
    }

    private void LockHinge()
    {
        Debug.Log("Locking hinge");
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void ReleaseHinge()
    {
        Debug.Log("Releasing hinge");
        rb.constraints = RigidbodyConstraints.None;
    }

    public void WebToggleHinge()
    {
        if (isLocked)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
            isLocked = false;
        }
        else
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            isLocked = true;
        }
    }
}
