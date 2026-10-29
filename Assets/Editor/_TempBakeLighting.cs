// TEMPORARY batchmode entry point: rebake lighting for every scene so each
// scene owns its own LightingData asset instead of borrowing another scene's.
// Delete this file (and its .meta) once the bake has completed.

using System;
using System.Diagnostics;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public static class _TempBakeLighting
{
    // Cannot be opened - unresolved merge conflict markers (cf4a6893)
    const string Skip = "WebLabChemChange-Zack";

    public static void BakeAll()
    {
        var overall = Stopwatch.StartNew();
        var sb = new StringBuilder();
        int baked = 0, skippedNoStatics = 0, skippedBroken = 0, failed = 0;

        var guids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" });
        Debug.Log($"[BAKE] {guids.Length} scenes found");

        foreach (var g in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(g);
            if (path.Contains(Skip))
            {
                skippedBroken++;
                Debug.Log($"[BAKE] SKIP (merge conflict): {path}");
                continue;
            }

            try
            {
                var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

                int statics = 0;
                foreach (var mr in Object.FindObjectsByType<MeshRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                    if (GameObjectUtility.AreStaticEditorFlagsSet(mr.gameObject, StaticEditorFlags.ContributeGI))
                        statics++;

                if (statics == 0)
                {
                    skippedNoStatics++;
                    Debug.Log($"[BAKE] skip (no ContributeGI renderers): {scene.name}");
                    continue;
                }

                var sw = Stopwatch.StartNew();
                Debug.Log($"[BAKE] START {scene.name}  statics={statics}  lightmapper={LightmapEditorSettings.lightmapper}");

                bool ok = Lightmapping.Bake();
                sw.Stop();

                if (ok)
                {
                    EditorSceneManager.SaveScene(scene);
                    baked++;
                    var ld = Lightmapping.lightingDataAsset;
                    Debug.Log($"[BAKE] DONE  {scene.name}  in {sw.Elapsed.TotalMinutes:F1} min  " +
                              $"lightmaps={(LightmapSettings.lightmaps?.Length ?? 0)}  " +
                              $"LightingData={(ld != null ? AssetDatabase.GetAssetPath(ld) : "<null>")}");
                }
                else
                {
                    failed++;
                    Debug.Log($"[BAKE] FAILED {scene.name} after {sw.Elapsed.TotalMinutes:F1} min");
                }
            }
            catch (Exception e)
            {
                failed++;
                Debug.Log($"[BAKE] ERROR {path} : {e.Message}");
            }
        }

        overall.Stop();
        sb.AppendLine("=== BAKE SUMMARY ===");
        sb.AppendLine($"  baked            : {baked}");
        sb.AppendLine($"  skipped(no GI)   : {skippedNoStatics}");
        sb.AppendLine($"  skipped(broken)  : {skippedBroken}");
        sb.AppendLine($"  failed           : {failed}");
        sb.AppendLine($"  total time       : {overall.Elapsed.TotalMinutes:F1} min");
        Debug.Log(sb.ToString());

        if (failed > 0) EditorApplication.Exit(1);
    }
}
