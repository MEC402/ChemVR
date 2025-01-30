using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StopcockController : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;
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
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void ReleaseHinge()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
}
