#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Adds numbered, named workstation anchors + floating labels to the ACTIVE scene.
/// Anchor positions/rotations are copied from the TeleAnchor_* objects in LabSceneGlasswareUse.
/// Re-running replaces the previous "Station Anchors" root, so it is safe to repeat.
///   1 PPE   2 MIXING   3 HOOD   4 SINK   5 TUBE (new, offset from MIXING)
/// Menu: ChemVR / Add Station Labels
/// </summary>
public static class AddStationLabels
{
    const string SourceScene = "Assets/Scenes/LabSceneGlasswareUse.unity";
    const string AnchorPrefab = "Assets/VRTemplateAssets/Prefabs/Teleport/Teleport Anchor.prefab";
    const string RootName = "Station Anchors";

    // TUBE sits beside MIXING, on the side away from PPE. Edit here if it collides with furniture.
    static readonly Vector3 TubeOffsetFromMixing = new Vector3(-1.5f, 0f, 0f);
    const float LabelHeight = 2.3f;
    const float TubeStandDistance = 1.0f;

    struct Station { public int number; public string label; public string source; }

    static readonly Station[] Stations =
    {
        new Station { number = 1, label = "PPE",    source = "TeleAnchor_PPE" },
        new Station { number = 2, label = "MIXING", source = "TeleAnchor_Glassware" },
        new Station { number = 3, label = "HOOD",   source = "TeleAnchor_Hood" },
        new Station { number = 4, label = "SINK",   source = "TeleAnchor_Sink" },
        new Station { number = 5, label = "TUBE",   source = null }, // derived from MIXING
    };

    [MenuItem("ChemVR/Add Station Labels")]
    public static void Run()
    {
        Scene target = SceneManager.GetActiveScene();
        if (target.path == SourceScene)
        {
            EditorUtility.DisplayDialog("Add Station Labels",
                "Open the Module 3 scene first (this is the Glassware source scene).", "OK");
            return;
        }

        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AnchorPrefab);
        if (prefab == null) { Debug.LogError("Teleport Anchor prefab not found: " + AnchorPrefab); return; }

        // Read source anchor poses from the Glassware scene (additive, closed afterwards).
        Scene src = EditorSceneManager.OpenScene(SourceScene, OpenSceneMode.Additive);
        var poses = new System.Collections.Generic.Dictionary<string, (Vector3 p, Quaternion r)>();
        foreach (var root in src.GetRootGameObjects())
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
                if (t.name.StartsWith("TeleAnchor_") && !poses.ContainsKey(t.name))
                    poses[t.name] = (t.position, t.rotation);
        EditorSceneManager.CloseScene(src, true);
        SceneManager.SetActiveScene(target);

        foreach (var s in Stations)
            if (s.source != null && !poses.ContainsKey(s.source))
            { Debug.LogError("Missing " + s.source + " in " + SourceScene); return; }

        // Replace any previous run.
        foreach (var root in target.GetRootGameObjects())
            if (root.name == RootName) Object.DestroyImmediate(root);

        var parent = new GameObject(RootName);
        SceneManager.MoveGameObjectToScene(parent, target);
        Undo.RegisterCreatedObjectUndo(parent, "Add Station Labels");

        var mixing = poses["TeleAnchor_Glassware"];

        // TUBE station: stand ~1 m from the existing test-tube holder, on the MIXING side, facing it.
        // Falls back to a fixed offset from MIXING if "TubesInHolder" is not in the scene.
        (Vector3 p, Quaternion r) tube = (mixing.p + mixing.r * TubeOffsetFromMixing, mixing.r);
        GameObject holder = GameObject.Find("TubesInHolder");
        if (holder != null)
        {
            Vector3 h = holder.transform.position;
            Vector3 toMixing = new Vector3(mixing.p.x - h.x, 0f, mixing.p.z - h.z);
            if (toMixing.sqrMagnitude < 0.0001f) toMixing = Vector3.back;
            toMixing.Normalize();
            Vector3 stand = new Vector3(h.x, mixing.p.y, h.z) + toMixing * TubeStandDistance;
            tube = (stand, Quaternion.LookRotation(-toMixing));
            Debug.Log("TUBE station placed at TubesInHolder " + h);
        }
        else Debug.LogWarning("TubesInHolder not found; TUBE station placed by fixed offset from MIXING.");
        foreach (var s in Stations)
        {
            (Vector3 p, Quaternion r) pose = s.source != null ? poses[s.source] : tube;

            var anchor = (GameObject)PrefabUtility.InstantiatePrefab(prefab, target);
            anchor.name = $"Station{s.number}_{s.label}";
            anchor.transform.SetParent(parent.transform, false);
            anchor.transform.SetPositionAndRotation(pose.p, pose.r);

            var labelGO = new GameObject("Label");
            labelGO.transform.SetParent(anchor.transform, false);
            labelGO.transform.position = pose.p + Vector3.up * LabelHeight;
            var tmp = labelGO.AddComponent<TextMeshPro>();
            tmp.text = $"<size=400%><b>{s.number}</b></size>\n{s.label}";
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 6f;
            tmp.color = Color.white;
            tmp.outlineWidth = 0.25f;
            tmp.outlineColor = new Color32(0, 0, 0, 255);
            tmp.rectTransform.sizeDelta = new Vector2(4f, 3f);
            tmp.textWrappingMode = TextWrappingModes.NoWrap;
            labelGO.AddComponent<StationLabelBillboard>();
        }

        EditorSceneManager.MarkSceneDirty(target);
        Debug.Log("Added 5 station anchors + labels under '" + RootName + "'. Verify positions, especially 5_TUBE.");
    }
}
#endif
