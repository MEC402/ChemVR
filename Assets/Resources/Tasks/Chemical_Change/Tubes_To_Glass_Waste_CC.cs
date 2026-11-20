using UnityEngine;

/// <summary>
/// Module 3 cleanup step (task 30): finishes once every tube in <see cref="tubeNames"/> has been dropped into the
/// glass-waste can by the sink (the space enclosed by the can's "TrashColliders" walls) and is no longer held.
/// </summary>
public class Tubes_To_Glass_Waste_CC : TaskStep
{
    [SerializeField] string[] tubeNames = { "Tube 1", "Tube 2" };
    [SerializeField] string binColliderRoot = "TrashColliders";
    [SerializeField] float settleSeconds = 0.5f;

    Transform bin;
    Transform[] tubes;
    bool[] held;
    float insideTime;
    float nextLog;

    protected override void SetTaskStepState(string state) { }

    void OnEnable()
    {
        tubes = new Transform[tubeNames.Length];
        held = new bool[tubeNames.Length];
        GameEventsManager.instance.webGLEvents.OnObjectGrabbed += OnGrabbed;
        GameEventsManager.instance.webGLEvents.OnObjectReleased += OnReleased;
    }

    void OnDisable()
    {
        GameEventsManager.instance.webGLEvents.OnObjectGrabbed -= OnGrabbed;
        GameEventsManager.instance.webGLEvents.OnObjectReleased -= OnReleased;
    }

    void SetHeld(GameObject obj, bool value)
    {
        if (tubes == null) return;
        for (int i = 0; i < tubes.Length; i++)
            if (tubes[i] != null && (obj == tubes[i].gameObject || obj.transform.IsChildOf(tubes[i]))) held[i] = value;
    }
    void OnGrabbed(GameObject obj) { SetHeld(obj, true); }
    void OnReleased(GameObject obj) { SetHeld(obj, false); }

    // The tube's own position (its pivot is near the base). Renderer bounds are unusable here: the tube carries a
    // pour-effect renderer whose bounds span much of the lab.
    static Vector3 Center(Transform t) { return t.position + Vector3.up * 0.1f; }

    void Update()
    {
        if (bin == null)
        {
            var go = GameObject.Find(binColliderRoot);
            if (go != null) bin = go.transform;
        }
        if (bin == null) return;

        // The space enclosed by the can's wall/floor colliders.
        var cols = bin.GetComponentsInChildren<Collider>();
        if (cols.Length == 0) return;
        Bounds inner = cols[0].bounds;
        for (int i = 1; i < cols.Length; i++) inner.Encapsulate(cols[i].bounds);

        bool all = true;
        string status = "";
        for (int i = 0; i < tubes.Length; i++)
        {
            if (tubes[i] == null)
            {
                var go = GameObject.Find(tubeNames[i]);
                if (go != null) tubes[i] = go.transform;
            }
            if (tubes[i] == null) { all = false; continue; }
            bool inside = inner.Contains(Center(tubes[i]));
            status += tubeNames[i] + ": inside=" + inside + " held=" + held[i] + "; ";
            if (!inside || held[i]) all = false;
        }

        if (Time.time >= nextLog)
        {
            nextLog = Time.time + 2f;
            Debug.Log("[Tubes_To_Glass_Waste_CC] " + status);
        }

        if (all) insideTime += Time.deltaTime; else insideTime = 0f;
        if (insideTime >= settleSeconds)
        {
            enabled = false;
            FinishTaskStep();
        }
    }
}
