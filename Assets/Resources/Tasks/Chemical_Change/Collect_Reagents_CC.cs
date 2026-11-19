using UnityEngine;

/// <summary>
/// Task 9 of the Physical &amp; Chemical Changes module (Collect the reagents): bring the three reagent bottles
/// (K2CR2O4, NAOH, H2SO4) to the MIXING station. Finishes once all three are set down (not held) within
/// <see cref="maxDistance"/> metres (horizontally) of the Station2_MIXING anchor.
/// </summary>
public class Collect_Reagents_CC : TaskStep
{
    [SerializeField] string stationName = "Station2_MIXING";
    [SerializeField] string[] bottleNames = { "K2CR2O4", "NAOH", "H2SO4" };
    [SerializeField] float maxDistance = 2.5f;
    [SerializeField] float settleSeconds = 0.5f;

    Transform station;
    Transform[] bottles;
    bool[] held;
    float insideTime;
    float nextLog;

    protected override void SetTaskStepState(string state) { }

    void OnEnable()
    {
        bottles = new Transform[bottleNames.Length];
        held = new bool[bottleNames.Length];
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
        if (bottles == null) return;
        for (int i = 0; i < bottles.Length; i++)
            if (bottles[i] != null && (obj == bottles[i].gameObject || obj.transform.IsChildOf(bottles[i])))
                held[i] = value;
    }
    void OnGrabbed(GameObject obj) { SetHeld(obj, true); }
    void OnReleased(GameObject obj) { SetHeld(obj, false); }

    static Transform FindByName(string name)
    {
        var go = GameObject.Find(name);
        if (go != null) return go.transform;
        foreach (var t in FindObjectsByType<Transform>(FindObjectsSortMode.None))
            if (string.Equals(t.name, name, System.StringComparison.OrdinalIgnoreCase)) return t;
        return null;
    }

    static Vector3 Center(Transform t)
    {
        var rends = t.GetComponentsInChildren<Renderer>();
        if (rends.Length == 0) return t.position;
        Bounds b = rends[0].bounds;
        for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);
        return b.center;
    }

    void Update()
    {
        if (station == null) station = FindByName(stationName);
        if (station == null) return;

        bool all = true;
        string status = "";
        for (int i = 0; i < bottles.Length; i++)
        {
            if (bottles[i] == null) bottles[i] = FindByName(bottleNames[i]);
            if (bottles[i] == null) { all = false; status += bottleNames[i] + ": not found; "; continue; }
            Vector3 d = Center(bottles[i]) - station.position;
            d.y = 0f;
            status += bottleNames[i] + ": " + d.magnitude.ToString("F2") + " m, held=" + held[i] + "; ";
            if (held[i] || d.magnitude > maxDistance) all = false;
        }

        if (Time.time >= nextLog)
        {
            nextLog = Time.time + 2f;
            Debug.Log("[Collect_Reagents_CC] (need <= " + maxDistance + " m from " + stationName + ") " + status);
        }

        if (all) insideTime += Time.deltaTime; else insideTime = 0f;
        if (insideTime >= settleSeconds)
        {
            enabled = false;
            FinishTaskStep();
        }
    }
}
