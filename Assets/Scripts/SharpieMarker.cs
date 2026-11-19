using UnityEngine;

/// <summary>
/// Put on the Sharpie. Whenever its tip touches a test tube (anything under a <see cref="TubeLabel"/>),
/// that tube's label is revealed.
/// </summary>
public class SharpieMarker : MonoBehaviour
{
    [Tooltip("Transform at the felt tip. Found automatically by name (Sharpie_Tip) if empty.")]
    public Transform tip;
    [Tooltip("Radius (metres) around the tip that counts as touching.")]
    public float touchRadius = 0.02f;

    readonly Collider[] hits = new Collider[16];

    void Start()
    {
        if (tip == null)
        {
            foreach (var t in GetComponentsInChildren<Transform>())
                if (t.name == "Sharpie_Tip") { tip = t; break; }
        }
    }

    void Update()
    {
        if (tip == null) return;
        int n = Physics.OverlapSphereNonAlloc(tip.position, touchRadius, hits, ~0, QueryTriggerInteraction.Collide);
        for (int i = 0; i < n; i++)
        {
            var label = hits[i].GetComponentInParent<TubeLabel>();
            if (label != null && !label.IsLabeled)
            {
                Debug.Log("[SharpieMarker] labeling " + label.name + " (touched " + hits[i].name + ")");
                label.Reveal();
            }
        }
    }
}
