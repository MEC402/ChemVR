using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put on a grabbable rack/holder that has loose Rigidbody items (e.g. test tubes) as children.
/// While the holder is carried (WebGLGrab makes it kinematic and parents it to the hold point) the
/// child Rigidbodies are made kinematic so the items travel with it instead of falling out.
/// Items grabbed out of the rack individually are no longer children and are unaffected.
/// </summary>
public class RackCarry : MonoBehaviour
{
    private readonly List<Rigidbody> frozen = new List<Rigidbody>();

    private void Start()
    {
        if (GameEventsManager.instance == null) return;
        GameEventsManager.instance.webGLEvents.OnObjectGrabbed += OnGrabbed;
        GameEventsManager.instance.webGLEvents.OnObjectReleased += OnReleased;
    }

    private void OnDestroy()
    {
        if (GameEventsManager.instance == null) return;
        GameEventsManager.instance.webGLEvents.OnObjectGrabbed -= OnGrabbed;
        GameEventsManager.instance.webGLEvents.OnObjectReleased -= OnReleased;
    }

    private void OnGrabbed(GameObject obj)
    {
        if (obj != gameObject) return;
        frozen.Clear();
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            if (rb.gameObject == gameObject || rb.isKinematic) continue;
            rb.isKinematic = true;
            frozen.Add(rb);
        }
    }

    private void OnReleased(GameObject obj)
    {
        if (obj != gameObject) return;
        foreach (var rb in frozen)
            if (rb != null) rb.isKinematic = false;
        frozen.Clear();
    }
}
