using UnityEngine;

/// <summary>
/// Stops the cleaning brush's solid collider from physically colliding with the scale's support
/// surface. Without this, that solid-on-solid contact stops the brush before its separate trigger
/// tip (which BrushScale uses to detect scrubbing) can reach far enough to overlap the scale's
/// trigger collider, so the "clean the scale" step could never register.
/// </summary>
public class BrushIgnoreScaleCollision : MonoBehaviour
{
    [Tooltip("The brush's solid (non-trigger) collider that would otherwise be blocked by the scale.")]
    [SerializeField] private Collider physicalCollider;

    private void Start()
    {
        if (physicalCollider == null) return;

        foreach (GameObject scale in GameObject.FindGameObjectsWithTag("Scale"))
        {
            foreach (Collider col in scale.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(physicalCollider, col, true);
            }
        }
    }
}
