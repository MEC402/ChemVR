using UnityEngine;

/// <summary>
/// Stops the cleaning brush's solid collider from physically shoving glassware around while
/// scrubbing, without touching either of the brush's colliders (WebGL's raycast grab and VR's
/// XRGrabInteractable both rely on those staying as they are). Registers a runtime
/// Physics.IgnoreCollision between the brush's physical collider and every glassware item's
/// colliders at Start. The brush's separate trigger collider, which ScrubWithBrushObj uses to
/// detect scrubbing, is untouched, so detection keeps working exactly as before.
/// </summary>
public class BrushIgnoreGlasswareCollision : MonoBehaviour
{
    [Tooltip("The brush's solid (non-trigger) collider that would otherwise push glassware around on contact.")]
    [SerializeField] private Collider physicalCollider;

    private void Start()
    {
        if (physicalCollider == null) return;

        ChemContainer[] glassware = FindObjectsByType<ChemContainer>(FindObjectsSortMode.None);
        foreach (ChemContainer container in glassware)
        {
            foreach (Collider col in container.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(physicalCollider, col, true);
            }
        }
    }
}
