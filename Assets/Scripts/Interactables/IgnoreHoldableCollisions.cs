using UnityEngine;

/// <summary>
/// Stops a hand tool from physically pushing other glassware around.
///
/// A held tool is kinematic, so anything its solid colliders touch gets shoved out of the way -
/// a dropper dipped into a rack tube would knock the whole rack about. The tool still has to be
/// grabbable (the desktop grab ray only sees solid colliders on the Holdable layer), so instead of
/// removing its colliders this makes them ignore every other Holdable and Chem-layer collider
/// (bottles, tubes, racks, container openings). Triggers are untouched, so drawing and dispensing
/// still work, and the tool still rests on benches and the floor.
///
/// Colliders spawned after Start are not covered.
/// </summary>
public class IgnoreHoldableCollisions : MonoBehaviour
{
    private void Start()
    {
        int holdable = LayerMask.NameToLayer("Holdable");
        int chem = LayerMask.NameToLayer("Chem");

        Collider[] mine = GetComponentsInChildren<Collider>(true);
        foreach (Collider other in FindObjectsByType<Collider>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            if (!other.enabled || other.isTrigger) continue; // triggers never push anything
            int layer = other.gameObject.layer;
            if (layer != holdable && layer != chem) continue;

            foreach (Collider m in mine)
            {
                if (m == other || m.isTrigger || !m.enabled || !m.gameObject.activeInHierarchy) continue;
                Physics.IgnoreCollision(m, other, true);
            }
        }
    }
}
