using UnityEditor;
using UnityEngine;

public class FixNullLayers : EditorWindow
{
    [MenuItem("Tools/Fix Null Layers")]
    public static void ShowWindow()
    {
        GetWindow<FixNullLayers>("Fix Null Layers");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Objects with Null or Invalid Layers"))
        {
            FixObjectsWithNullLayers();
        }
    }

    private static void FixObjectsWithNullLayers()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int fixedCount = 0;

        foreach (GameObject obj in allObjects)
        {
            // Check if the layer's name is blank or invalid
            if (string.IsNullOrEmpty(LayerMask.LayerToName(obj.layer)))
            {
                obj.layer = 0; // Assign to "Default" layer (layer index 0)
                fixedCount++;
            }
        }

        Debug.Log($"{fixedCount} objects were fixed and assigned to the 'Default' layer.");
    }
}
