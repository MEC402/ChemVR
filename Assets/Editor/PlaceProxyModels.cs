#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Adds clearly-named proxy models for lab gear that has no real model yet, on the right station bench.
/// Separate from "Place Station Models" so it never disturbs hand-adjusted copies; re-running replaces only
/// the "Station Proxies" root. Every proxy is named "Proxy_*" so it is easy to find and swap later.
///   MIXING: Proxy_Notebook (clip_board prefab)
/// Menu: ChemVR / Place Proxy Models
/// </summary>
public static class PlaceProxyModels
{
    const string RootName = "Station Proxies";
    const float Spacing = 0.25f;

    struct Proxy { public string name; public string prefab; public Vector3 scale; }

    static readonly Dictionary<string, Proxy[]> Plan = new Dictionary<string, Proxy[]>
    {
        { "Station2_MIXING", new[] {
            new Proxy { name = "Proxy_Notebook", prefab = "Assets/Prefabs/WebGL/Glove Hygiene Items/clip_board.prefab" },
        }},
    };

    [MenuItem("ChemVR/Place Proxy Models")]
    public static void Run()
    {
        Scene scene = SceneManager.GetActiveScene();
        foreach (var r in scene.GetRootGameObjects())
            if (r.name == RootName) Object.DestroyImmediate(r);
        Physics.SyncTransforms();

        var root = new GameObject(RootName);
        SceneManager.MoveGameObjectToScene(root, scene);
        Undo.RegisterCreatedObjectUndo(root, "Place Proxy Models");

        foreach (var kv in Plan)
        {
            var anchorGO = GameObject.Find(kv.Key);
            if (anchorGO == null) { Debug.LogWarning("Anchor " + kv.Key + " not found."); continue; }
            Transform a = anchorGO.transform;

            Vector3 center; float topY;
            if (!PlaceStationModels.FindBench(a, root.transform, out center, out topY))
            { center = a.position + a.forward * 0.9f; topY = a.position.y + 0.9f; Debug.LogWarning(kv.Key + ": no bench found; place proxies by hand."); }
            Vector3 toCenter = center - a.position; toCenter.y = 0f;
            Vector3 facing = toCenter.sqrMagnitude > 0.0001f ? toCenter.normalized : a.forward;
            Vector3 axis = Vector3.Cross(Vector3.up, facing);
            Quaternion yaw = Quaternion.LookRotation(-facing);

            // Second row (0.35 m further out) so proxies don't sit on top of the Station Models copies.
            for (int i = 0; i < kv.Value.Length; i++)
            {
                var p = kv.Value[i];
                float side = (i - (kv.Value.Length - 1) * 0.5f) * Spacing;
                Vector3 pos = center + axis * side + facing * 0.35f;
                pos.y = PlaceStationModels.SurfaceAt(pos, topY);

                GameObject go;
                if (p.prefab != null)
                {
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(p.prefab);
                    if (prefab == null) { Debug.LogWarning("Prefab missing: " + p.prefab); continue; }
                    go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
                    go.transform.SetParent(root.transform, true);
                    go.transform.rotation = yaw * prefab.transform.rotation;
                }
                else
                {
                    go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.SetParent(root.transform, true);
                    go.transform.localScale = p.scale;
                    go.transform.rotation = yaw;
                }
                go.name = p.name;
                go.transform.position = pos;
                PlaceStationModels.SitOnSurface(go, pos.y);
                Undo.RegisterCreatedObjectUndo(go, "Place Proxy Models");
            }
            Debug.Log(kv.Key + ": placed " + kv.Value.Length + " proxy model(s) at bench y=" + topY.ToString("F2"));
        }
        EditorSceneManager.MarkSceneDirty(scene);
    }
}
#endif
