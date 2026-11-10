#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Puts a copy of each station's models on the bench in front of that station, unless a matching
/// object is already within NearRadius of the station anchor. Run AFTER "ChemVR > Add Station Labels".
/// Re-running replaces the previous "Station Models" root. SINK is intentionally left alone, and
/// PPE items are only reported (copies of goggles/coat/gloves carry task wiring).
/// Menu: ChemVR / Place Station Models
/// </summary>
public static class PlaceStationModels
{
    const string RootName = "Station Models";
    const float NearRadius = 2.5f;   // an existing matching object this close counts as "already there"
    const float BenchForward = 0.9f; // metres in front of the anchor (anchor forward) to look for the bench
    const float Spacing = 0.22f;     // sideways gap between items
    const float RowSpacing = 0.25f;  // gap between rows
    const int PerRow = 5;

    struct Item { public string prefab; public int count; }

    static readonly Dictionary<string, Item[]> Plan = new Dictionary<string, Item[]>
    {
        // MIXING bench. Notebook, Sharpie and pH paper have no model yet (see docs/module_environment.md).
        { "Station2_MIXING", new[] {
            new Item { prefab = "Assets/Prefabs/Interactables/Graduated_Glassware/GraduatedCylinderMesh_Fluid.prefab", count = 1 },
            new Item { prefab = "Assets/Prefabs/DI_Water_New_Two.prefab", count = 1 },            // squirt-bottle stand-in
            new Item { prefab = "Assets/Prefabs/Tools (Particle)/titration_dropper.prefab", count = 1 }, // bench pipette
            new Item { prefab = "Assets/Prefabs/WebGL/Glove Hygiene Items/pencil.prefab", count = 1 },
        }},
        // TUBE bench: FlasksInHolder (tubes + rack) already exists; add the stir rod.
        { "Station5_TUBE", new[] {
            new Item { prefab = "Assets/Prefabs/Interactables/StirRod.prefab", count = 1 },
        }},
        // HOOD: three reagent bottles, two trays, three droppers.
        { "Station3_HOOD", new[] {
            new Item { prefab = "Assets/Prefabs/Tools (Static)/Yellow ChemBottle Variant.prefab", count = 1 }, // 0.1 M K2CrO4
            new Item { prefab = "Assets/Prefabs/Tools (Static)/White ChemBottle Variant .prefab", count = 1 }, // 1 M NaOH
            new Item { prefab = "Assets/Prefabs/Tools (Static)/ChemBottle.prefab", count = 1 },                // 3 M H2SO4
            new Item { prefab = "Assets/Prefabs/Interactables/tray.prefab", count = 2 },                       // Acids, Bases
            new Item { prefab = "Assets/Prefabs/Tools (Particle)/titration_dropper.prefab", count = 3 },
        }},
    };

    [MenuItem("ChemVR/Place Station Models")]
    public static void Run()
    {
        Scene scene = SceneManager.GetActiveScene();
        foreach (var r in scene.GetRootGameObjects())
            if (r.name == RootName) Object.DestroyImmediate(r);

        Physics.SyncTransforms();
        var all = new List<Transform>();
        foreach (var r in scene.GetRootGameObjects())
            all.AddRange(r.GetComponentsInChildren<Transform>(true));

        var root = new GameObject(RootName);
        SceneManager.MoveGameObjectToScene(root, scene);
        Undo.RegisterCreatedObjectUndo(root, "Place Station Models");

        int placed = 0;
        foreach (var kv in Plan)
        {
            var anchorGO = GameObject.Find(kv.Key);
            if (anchorGO == null) { Debug.LogWarning("Anchor " + kv.Key + " not found; run Add Station Labels first."); continue; }
            Transform a = anchorGO.transform;

            // Work out what is missing, then compute all bench positions before spawning anything.
            var toSpawn = new List<GameObject>();
            foreach (var item in kv.Value)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(item.prefab);
                if (prefab == null) { Debug.LogWarning("Prefab missing: " + item.prefab); continue; }
                int have = CountNearby(all, a.position, prefab, root.transform);
                for (int i = have; i < item.count; i++) toSpawn.Add(prefab);
                if (have > 0) Debug.Log(kv.Key + ": " + have + " x " + prefab.name + " already within " + NearRadius + " m");
            }
            if (toSpawn.Count == 0) continue;

            // Find the bench top around the station (any direction), then lay items out along its front.
            Vector3 center; float topY;
            bool found = FindBench(a, root.transform, out center, out topY);
            if (!found)
            {
                Debug.LogWarning(kv.Key + ": no bench surface found within 1.8 m; using fallback 0.9 m in front. Place by hand.");
                center = a.position + a.forward * BenchForward; topY = a.position.y + 0.9f;
            }
            Vector3 toCenter = center - a.position; toCenter.y = 0f;
            Vector3 facing = toCenter.sqrMagnitude > 0.0001f ? toCenter.normalized : a.forward;
            Vector3 axis = Vector3.Cross(Vector3.up, facing); // along the front edge of the bench

            var positions = new List<Vector3>();
            for (int i = 0; i < toSpawn.Count; i++)
            {
                int row = i / PerRow, col = i % PerRow;
                int inRow = Mathf.Min(PerRow, toSpawn.Count - row * PerRow);
                float side = (col - (inRow - 1) * 0.5f) * Spacing;
                Vector3 p = center + axis * side + facing * (row * RowSpacing);
                positions.Add(new Vector3(p.x, SurfaceAt(p, topY), p.z));
            }
            for (int i = 0; i < toSpawn.Count; i++)
            {
                var go = (GameObject)PrefabUtility.InstantiatePrefab(toSpawn[i], scene);
                go.transform.SetParent(root.transform, true);
                // Keep the prefab's own tilt (e.g. -90 on X for imported models); only add a yaw to face the station.
                Quaternion yaw = Quaternion.LookRotation(-facing);
                go.transform.rotation = yaw * toSpawn[i].transform.rotation;
                go.transform.position = positions[i];
                SitOnSurface(go, positions[i].y);
                Undo.RegisterCreatedObjectUndo(go, "Place Station Models");
                placed++;
            }
            Debug.Log(kv.Key + ": placed " + toSpawn.Count + " model(s)");
        }

        // PPE and SINK: report only.
        foreach (var name in new[] { "goggles", "labcoat", "glovebox" })
            ReportNearest(all, name, "Station1_PPE");

        FixModels(root.transform);
        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log("Place Station Models: placed " + placed + " model(s) under '" + RootName + "'. Check bench heights in the Scene view.");
    }

    // ---- Visibility + stability fix, safe to run on already-placed (and hand-adjusted) objects ----
    // The player camera does not render layers ChemIgnore (10) or HoldableCollision (12), which the dropper and
    // stir-rod prefabs use, so those copies were invisible in the Game view. Move them to "Holdable" (7).
    // Droppers are tall and tip over when upright; lay them flat, keeping their footprint where it was.
    [MenuItem("ChemVR/Fix Station Model Visibility")]
    public static void FixExisting()
    {
        foreach (var r in SceneManager.GetActiveScene().GetRootGameObjects())
            if (r.name == RootName) { FixModels(r.transform); EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene()); return; }
        Debug.LogWarning("No '" + RootName + "' root in the active scene.");
    }

    static void FixModels(Transform root)
    {
        int holdable = LayerMask.NameToLayer("Holdable");
        if (holdable < 0) { Debug.LogError("Layer 'Holdable' not found."); return; }
        Physics.SyncTransforms();
        int fixedCount = 0;
        foreach (Transform t in root)
        {
            bool dropper = t.name.StartsWith("titration_dropper");
            bool rod = t.name.StartsWith("StirRod");
            if (!dropper && !rod) continue;

            Undo.RecordObject(t.gameObject, "Fix Station Model");
            foreach (var c in t.GetComponentsInChildren<Transform>(true))
            {
                Undo.RecordObject(c.gameObject, "Fix Station Model");
                c.gameObject.layer = holdable;
            }

            if (dropper)
            {
                Bounds before = GetBounds(t);
                // Skip if already lying flat (bounds taller than wide means upright).
                if (before.size.y > Mathf.Max(before.size.x, before.size.z))
                {
                    Undo.RecordObject(t, "Fix Station Model");
                    t.RotateAround(before.center, Vector3.forward, 90f);
                    Physics.SyncTransforms();
                    Bounds after = GetBounds(t);
                    // Keep the footprint centred where it was and rest the lowest point on the surface below.
                    t.position += new Vector3(before.center.x - after.center.x, 0f, before.center.z - after.center.z);
                    Physics.SyncTransforms();
                    after = GetBounds(t);
                    float surface = before.min.y;
                    foreach (var h in Physics.RaycastAll(new Vector3(after.center.x, before.max.y + 0.5f, after.center.z),
                                                         Vector3.down, 3f, ~0, QueryTriggerInteraction.Ignore))
                        if (!h.collider.transform.IsChildOf(t) && Mathf.Abs(h.point.y - before.min.y) < 0.15f) surface = Mathf.Max(surface, h.point.y);
                    t.position += Vector3.up * (surface + 0.005f - after.min.y);
                }
            }
            fixedCount++;
        }
        Debug.Log("Fix Station Model Visibility: updated " + fixedCount + " object(s) (layer -> Holdable; droppers laid flat).");
    }

    static Bounds GetBounds(Transform t)
    {
        var rs = t.GetComponentsInChildren<Renderer>();
        Bounds b = rs[0].bounds;
        foreach (var r in rs) b.Encapsulate(r.bounds);
        return b;
    }

    static int CountNearby(List<Transform> all, Vector3 center, GameObject prefab, Transform ignoreRoot)
    {
        int n = 0;
        foreach (var t in all)
        {
            if (t == null || t.IsChildOf(ignoreRoot)) continue;
            bool match = t.name.StartsWith(prefab.name) ||
                         PrefabUtility.GetCorrespondingObjectFromSource(t.gameObject) == prefab;
            if (!match) continue;
            if (Vector3.Distance(t.position, center) <= NearRadius) n++;
        }
        return n;
    }

    internal static bool Mine(Transform t, Transform ignoreRoot) { return t != null && t.IsChildOf(ignoreRoot); }

    // Sample rays on rings around the anchor; the most common surface height 0.7-1.2 m above the floor is the bench.
    internal static bool FindBench(Transform a, Transform ignoreRoot, out Vector3 center, out float topY)
    {
        var pts = new List<Vector3>();
        for (float r = 0.5f; r <= 1.8f; r += 0.2f)
            for (int deg = 0; deg < 360; deg += 15)
            {
                Vector3 d = Quaternion.Euler(0f, deg, 0f) * Vector3.forward;
                Vector3 o = a.position + d * r + Vector3.up * 2.2f;
                foreach (var h in Physics.RaycastAll(o, Vector3.down, 3.5f, ~0, QueryTriggerInteraction.Ignore))
                {
                    if (Mine(h.collider.transform, ignoreRoot)) continue;
                    float rel = h.point.y - a.position.y;
                    if (rel >= 0.7f && rel <= 1.2f) pts.Add(h.point);
                }
            }
        center = a.position; topY = a.position.y + 0.9f;
        if (pts.Count == 0) return false;

        var bins = new Dictionary<int, int>();
        foreach (var q in pts) { int k = Mathf.RoundToInt(q.y / 0.05f); bins[k] = bins.ContainsKey(k) ? bins[k] + 1 : 1; }
        int bestK = 0, bestN = -1;
        foreach (var kv in bins) if (kv.Value > bestN) { bestN = kv.Value; bestK = kv.Key; }

        Vector3 sum = Vector3.zero; int n = 0; float ySum = 0f;
        foreach (var q in pts)
            if (Mathf.Abs(Mathf.RoundToInt(q.y / 0.05f) - bestK) <= 1) { sum += q; ySum += q.y; n++; }
        center = sum / n; topY = ySum / n;
        Debug.Log(a.name + ": bench surface y=" + topY.ToString("F2") + " at " + center + " (" + n + " ray hits)");
        return true;
    }

    // Height of the surface at p near the bench height; falls back to the bench's average height.
    internal static float SurfaceAt(Vector3 p, float topY)
    {
        foreach (var h in Physics.RaycastAll(new Vector3(p.x, topY + 1.0f, p.z), Vector3.down, 2f, ~0, QueryTriggerInteraction.Ignore))
            if (Mathf.Abs(h.point.y - topY) < 0.06f) return h.point.y;
        return topY;
    }

    // Lift the object so its lowest collider/renderer point rests on y (pivots are often not at the base).
    internal static void SitOnSurface(GameObject go, float y)
    {
        var rs = go.GetComponentsInChildren<Renderer>();
        if (rs.Length == 0) return;
        Bounds b = rs[0].bounds;
        foreach (var r in rs) b.Encapsulate(r.bounds);
        go.transform.position += Vector3.up * (y + 0.005f - b.min.y);
    }

    static void ReportNearest(List<Transform> all, string objName, string anchorName)
    {
        var anchor = GameObject.Find(anchorName);
        if (anchor == null) return;
        float best = float.MaxValue;
        foreach (var t in all)
            if (t.name.ToLower().StartsWith(objName))
                best = Mathf.Min(best, Vector3.Distance(t.position, anchor.transform.position));
        Debug.Log("PPE check: nearest '" + objName + "' is " + (best == float.MaxValue ? "not in scene" : best.ToString("F1") + " m from " + anchorName));
    }
}
#endif
