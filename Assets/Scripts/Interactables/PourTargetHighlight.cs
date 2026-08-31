using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lights up an object while something is aimed at it.
///
/// Driven by ChemContainer's pour targeting and by PipetteFunctions' aim feedback, so it turns on
/// exactly when the interaction would land here - it is an indicator of the real test, not a
/// proximity guess. Attach to any object with a ChemContainer; no other wiring is needed, and
/// PipetteFunctions adds one on demand where it is missing.
///
/// More than one system can want the same object lit at once, so requests are tracked per caller:
/// a caller switching its own request off never steals a glow another caller still wants.
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

    // Parallel lists of who wants this lit and in what colour. Kept in request order so the most
    // recent request wins the colour; both are only ever a handful of entries long.
    private readonly List<UnityEngine.Object> requesters = new List<UnityEngine.Object>();
    private readonly List<Color> requestedColors = new List<Color>();
    private Color appliedColor;

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
    /// Turns the highlight on or off in this component's own colour.
    /// Cheap to call repeatedly - repeats are ignored.
    /// </summary>
    public void SetHighlighted(bool on)
    {
        SetHighlighted(this, on, highlightColor);
    }

    /// <summary>
    /// Turns the highlight on or off on behalf of one caller, in the colour that caller wants.
    /// The object stays lit while any caller still wants it lit. Cheap to call every frame.
    /// </summary>
    /// <param name="source">Whoever is asking. Each caller's request is tracked separately.</param>
    /// <param name="on">Whether this caller wants the object lit.</param>
    /// <param name="color">Emission colour to use. Values above 1 read as a stronger glow.</param>
    public void SetHighlighted(UnityEngine.Object source, bool on, Color color)
    {
        if (source == null) source = this;

        // A caller destroyed while it still wanted the glow would otherwise keep it on forever.
        for (int i = requesters.Count - 1; i >= 0; i--)
        {
            if (requesters[i] == null)
            {
                requesters.RemoveAt(i);
                requestedColors.RemoveAt(i);
            }
        }

        int index = requesters.IndexOf(source);
        if (on)
        {
            if (index < 0)
            {
                requesters.Add(source);
                requestedColors.Add(color);
            }
            else
            {
                requestedColors[index] = color;
            }
        }
        else if (index >= 0)
        {
            requesters.RemoveAt(index);
            requestedColors.RemoveAt(index);
        }

        bool wanted = requesters.Count > 0;
        Apply(wanted, wanted ? requestedColors[requestedColors.Count - 1] : appliedColor);
    }

    private void Apply(bool on, Color color)
    {
        if (instancedMaterials == null) return;
        if (highlighted == on && (!on || appliedColor == color)) return;
        highlighted = on;
        appliedColor = color;

        for (int i = 0; i < instancedMaterials.Length; i++)
        {
            Material m = instancedMaterials[i];
            if (m == null || !m.HasProperty(EmissionColor)) continue;

            if (on)
            {
                m.EnableKeyword("_EMISSION");
                m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                m.SetColor(EmissionColor, color);
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
