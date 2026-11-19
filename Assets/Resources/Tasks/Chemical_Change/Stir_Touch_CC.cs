using UnityEngine;

/// <summary>
/// Module 3 "stir" step: finishes as soon as the stir rod touches a test tube (any object, or parent of the
/// touched collider, whose name contains <see cref="targetNameContains"/>). Used by tasks 15, 20 and 25.
/// A small relay component is added to the rod at runtime to report its collisions.
/// </summary>
public class Stir_Touch_CC : TaskStep
{
    [SerializeField] string rodName = "StirRod";
    [SerializeField] string targetNameContains = "Tube "; // trailing space: matches "Tube 1", not the rack "TubesInHolder"

    StirRodTouchRelay relay;

    protected override void SetTaskStepState(string state) { }

    CapsuleCollider rodCapsule;
    static readonly Collider[] hits = new Collider[32];
    float nextLog;

    void OnEnable()
    {
        Debug.Log("[Stir_Touch_CC] step is active; touch the stir rod to a test tube");
        StirRodTouchRelay.OnTouched += OnTouched;
    }
    void OnDisable() { StirRodTouchRelay.OnTouched -= OnTouched; }

    void Update()
    {
        if (rodCapsule != null) PollOverlap();
        if (relay != null) return;
        var rod = GameObject.Find(rodName);
        if (rod == null) { if (Time.time > nextLog) { nextLog = Time.time + 3f; Debug.LogWarning("[Stir_Touch_CC] '" + rodName + "' not found"); } return; }
        rodCapsule = rod.GetComponentInChildren<CapsuleCollider>();
        var body = rod.GetComponentInParent<Rigidbody>();
        var host = body != null ? body.gameObject : rod;
        relay = host.GetComponent<StirRodTouchRelay>();
        if (relay == null) relay = host.AddComponent<StirRodTouchRelay>();
    }

    // The rod can be carried (kinematic) or sit among kinematic/sleeping bodies, where collision callbacks never come,
    // so also test the rod's capsule against everything every frame.
    void PollOverlap()
    {
        var c = rodCapsule;
        Transform t = c.transform;
        Vector3 ls = t.lossyScale;
        float radius = c.radius * (c.direction == 0 ? Mathf.Max(ls.y, ls.z) : c.direction == 1 ? Mathf.Max(ls.x, ls.z) : Mathf.Max(ls.x, ls.y)) + 0.01f;
        Vector3 axis = c.direction == 0 ? t.right : c.direction == 1 ? t.up : t.forward;
        float axisScale = c.direction == 0 ? ls.x : c.direction == 1 ? ls.y : ls.z;
        float half = Mathf.Max(0f, c.height * axisScale * 0.5f - radius);
        Vector3 center = t.TransformPoint(c.center);
        int n = Physics.OverlapCapsuleNonAlloc(center + axis * half, center - axis * half, radius, hits, ~0, QueryTriggerInteraction.Collide);
        for (int i = 0; i < n; i++)
        {
            if (hits[i] == null || hits[i].transform.IsChildOf(rodCapsule.transform)) continue;
            OnTouched(hits[i]);
            if (!enabled) return;
        }
    }

    void OnTouched(Collider other)
    {
        for (Transform t = other.transform; t != null; t = t.parent)
        {
            if (t.name.IndexOf(targetNameContains, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Debug.Log("[Stir_Touch_CC] stir rod touched '" + t.name + "'");
                FinishTaskStep();
                return;
            }
        }
    }
}

/// <summary>Reports every collision / trigger contact of the object it sits on.</summary>
public class StirRodTouchRelay : MonoBehaviour
{
    public static event System.Action<Collider> OnTouched;
    void OnCollisionEnter(Collision c) { OnTouched?.Invoke(c.collider); }
    void OnTriggerEnter(Collider c) { OnTouched?.Invoke(c); }
}
