using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lights up a container while another container is aimed at it.
///
/// Driven by ChemContainer's pour targeting, so it turns on exactly when a pour would land here
/// - it is an indicator of the real test, not a proximity guess. Attach to any object with a
/// ChemContainer; no other wiring is needed.
/// </summary>
public class PourTargetHighlight : MonoBehaviour
{
    [Tooltip("Emission colour applied while this container is being aimed at.")]
    public Color highlightColor = new Color(0f, 0.9559735f, 0.9646866f); // matches Materials/Highlight.mat

    [Tooltip("Renderers to tint. Left empty, every enabled mesh renderer on this object and its " +
             "children is used.")]
    public Renderer[] renderersToTint;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private Material[] instancedMaterials;
    private Color[] originalEmission;
    private bool[] originalEmissionEnabled;
    private bool highlighted;

    void Awake()
    {
        Renderer[] targets = (renderersToTint != null && renderersToTint.Length > 0)
            ? renderersToTint
            : GetComponentsInChildren<Renderer>(true);

        // Accessing .materials instances the materials for this object only, so tinting one
        // flask never bleeds into the others sharing the same glass material.
        List<Material> mats = new List<Material>();
        foreach (Renderer r in targets)
        {
            if (r == null || r is ParticleSystemRenderer) continue;
            // The Opening, Pour Point and Hit Marker children carry disabled marker renderers.
            // Instancing their materials would cost us memory for something never drawn.
            if (!r.enabled) continue;

            foreach (Material m in r.materials)
            {
                if (m != null) mats.Add(m);
            }
        }

        instancedMaterials = mats.ToArray();
        originalEmission = new Color[instancedMaterials.Length];
        originalEmissionEnabled = new bool[instancedMaterials.Length];

        for (int i = 0; i < instancedMaterials.Length; i++)
        {
            Material m = instancedMaterials[i];
            originalEmissionEnabled[i] = m.IsKeywordEnabled("_EMISSION");
            originalEmission[i] = m.HasProperty(EmissionColor) ? m.GetColor(EmissionColor) : Color.black;
        }
    }

    /// <summary>
    /// Turns the highlight on or off. Cheap to call repeatedly - repeats are ignored.
    /// </summary>
    public void SetHighlighted(bool on)
    {
        if (highlighted == on || instancedMaterials == null) return;
        highlighted = on;

        for (int i = 0; i < instancedMaterials.Length; i++)
        {
            Material m = instancedMaterials[i];
            if (m == null || !m.HasProperty(EmissionColor)) continue;

            if (on)
            {
                m.EnableKeyword("_EMISSION");
                m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                m.SetColor(EmissionColor, highlightColor);
            }
            else
            {
                m.SetColor(EmissionColor, originalEmission[i]);
                if (!originalEmissionEnabled[i]) m.DisableKeyword("_EMISSION");
            }
        }
    }

    void OnDestroy()
    {
        if (instancedMaterials == null) return;
        foreach (Material m in instancedMaterials)
        {
            if (m != null) Destroy(m);
        }
    }
}
