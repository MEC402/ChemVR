#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

/// <summary>
/// Batch-mode WebGL build of the scenes enabled in Build Settings, using the project's current WebGL player settings.
///   unity build --target WebGL --execute-method BuildWebGL.Build -o <output folder>
/// The output folder is passed by the CLI as -buildOutput.
/// </summary>
public static class BuildWebGL
{
    public static void Build()
    {
        string outPath = null;
        var args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length - 1; i++)
            if (args[i] == "-buildOutput") outPath = args[i + 1];
        if (string.IsNullOrEmpty(outPath)) { Debug.LogError("BuildWebGL: missing -buildOutput <folder>"); EditorApplication.Exit(2); return; }

        var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        if (scenes.Length == 0) { Debug.LogError("BuildWebGL: no enabled scenes in Build Settings"); EditorApplication.Exit(2); return; }
        Debug.Log("BuildWebGL: scenes = " + string.Join(", ", scenes) + "; output = " + outPath);

        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None,
        });

        var summary = report.summary;
        Debug.Log("BuildWebGL: " + summary.result + ", " + summary.totalErrors + " error(s), size " + (summary.totalSize / (1024 * 1024)) + " MB, " + summary.totalTime);
        EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
    }
}
#endif
