using UnityEngine;

/// <summary>
/// Task 6 of the Physical &amp; Chemical Changes module: bring the TubesInHolder prefab (test tubes in their rack)
/// to the MIXING station. Completes once the rack has been set down within <see cref="maxDistance"/>
/// metres (horizontally) of the Station2_MIXING anchor. The N / A / UI-Next / B skips of the
/// click-through placeholder are not wired here; use the dev skip (B) if needed.
/// </summary>
public class Set_Up_Test_Tubes_CC : TaskStep
{
    [SerializeField] string stationName = "Station2_MIXING";
    [SerializeField] float maxDistance = 2.5f;
    [SerializeField] float settleSeconds = 0.5f;

    Transform station;
    [SerializeField] string rackName = "TubesInHolder";
    Transform rack;
    bool held;
    float insideTime;

    protected override void SetTaskStepState(string state)
    {
        // No resume state.
    }

    float nextLog;

    void OnEnable()
    {
        Debug.Log("[Set_Up_Test_Tubes_CC] step is active (should be while the popup reads Task 6 of 32)");
        GameEventsManager.instance.webGLEvents.OnObjectGrabbed += OnGrabbed;
        GameEventsManager.instance.webGLEvents.OnObjectReleased += OnReleased;
    }

    void OnDisable()
    {
        GameEventsManager.instance.webGLEvents.OnObjectGrabbed -= OnGrabbed;
        GameEventsManager.instance.webGLEvents.OnObjectReleased -= OnReleased;
    }

    void OnGrabbed(GameObject obj) { if (rack != null && obj == rack.gameObject) held = true; }
    void OnReleased(GameObject obj) { if (rack != null && obj == rack.gameObject) held = false; }

    void Update()
    {
        if (station == null)
        {
            var go = GameObject.Find(stationName);
            if (go != null) station = go.transform;
        }
        if (rack == null)
        {
            var go = GameObject.Find(rackName);
            if (go != null) rack = go.transform;
        }
        if (station == null || rack == null) return;

        // The root pivot of TubesInHolder is not at the rack itself, so measure from the
        // centre of its renderers instead.
        Vector3 center = rack.position;
        var rends = rack.GetComponentsInChildren<Renderer>();
        if (rends.Length > 0)
        {
            Bounds b = rends[0].bounds;
            for (int i = 1; i < rends.Length; i++) b.Encapsulate(rends[i].bounds);
            center = b.center;
        }
        Vector3 d = center - station.position;
        d.y = 0f;
        if (Time.time >= nextLog)
        {
            nextLog = Time.time + 2f;
            Debug.Log("[Set_Up_Test_Tubes_CC] rack is " + d.magnitude.ToString("F2") + " m from " + stationName + " (need <= " + maxDistance + "), held=" + held);
        }
        if (!held && d.magnitude <= maxDistance) insideTime += Time.deltaTime;
        else insideTime = 0f;

        if (insideTime >= settleSeconds)
        {
            enabled = false;
            FinishTaskStep();
        }
    }
}
