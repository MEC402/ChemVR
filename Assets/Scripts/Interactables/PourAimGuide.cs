using UnityEngine;

/// <summary>
/// Draws a line straight down from a container's pour point so the player can see where the
/// pour will land, and how much slack they have.
///
/// The line is drawn at the width of the container's actual pour cast, so what you see is the
/// volume that has to overlap the target's opening - not a decorative pointer.
///
/// Attach to any GameObject with a ChemContainer. ChemContainer finds it in Start and drives it;
/// no other wiring is needed.
/// </summary>
[RequireComponent(typeof(ChemContainer))]
public class PourAimGuide : MonoBehaviour
{
    [Tooltip("Colour of the guide when the pour would land in a container.")]
    public Color validColor = new Color(0f, 0.9559735f, 0.9646866f, 0.75f); // matches Materials/Highlight.mat

    [Tooltip("Colour of the guide when nothing is lined up underneath.")]
    public Color invalidColor = new Color(1f, 0.35f, 0.35f, 0.45f);

    [Tooltip("Draw the guide at the width of the container's pour cast, so it shows the real " +
             "aiming tolerance. Turn off to use Line Width instead.")]
    public bool matchCastRadius = true;

    [Tooltip("Guide width in metres. Only used when Match Cast Radius is off.")]
    public float lineWidth = 0.01f;

    [Tooltip("How far down the guide is drawn when nothing is lined up underneath.")]
    public float missLength = 0.4f;

    private ChemContainer container;
    private LineRenderer line;
    private Material lineMaterial;

    void Awake()
    {
        container = GetComponent<ChemContainer>();

        // Build our own child renderer rather than adding one to the container, so we never
        // collide with a LineRenderer that already exists on the object.
        GameObject holder = new GameObject("Pour Aim Guide");
        holder.transform.SetParent(transform, false);

        line = holder.AddComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.positionCount = 2;
        line.numCapVertices = 4;
        line.textureMode = LineTextureMode.Stretch;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        line.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;

        lineMaterial = new Material(GuideShader());
        line.material = lineMaterial;
        line.enabled = false;
    }

    /// <summary>
    /// Picks a shader that renders vertex-coloured, unlit geometry under URP.
    /// Falls back progressively in case a project strips one of these from the build.
    /// </summary>
    private static Shader GuideShader()
    {
        Shader s = Shader.Find("Sprites/Default");
        if (s == null) s = Shader.Find("Universal Render Pipeline/Unlit");
        if (s == null) s = Shader.Find("Unlit/Color");
        return s;
    }

    /// <summary>
    /// Called every physics step by ChemContainer.
    /// </summary>
    /// <param name="active">Whether the player is tipping far enough to be aiming.</param>
    /// <param name="origin">World position of the pour point.</param>
    /// <param name="hitPoint">Where the pour cast landed. Ignored when hasTarget is false.</param>
    /// <param name="hasTarget">Whether the cast found a container that can receive fluid.</param>
    public void SetAim(bool active, Vector3 origin, Vector3 hitPoint, bool hasTarget)
    {
        if (line == null) return;

        if (!active)
        {
            line.enabled = false;
            return;
        }

        float width = (matchCastRadius && container != null)
            ? container.pourCastRadius * 1f
            : lineWidth;

        Color color = hasTarget ? validColor : invalidColor;

        line.enabled = true;
        line.SetPosition(0, origin);
        line.SetPosition(1, hasTarget ? hitPoint : origin + Vector3.down * missLength);
        line.startWidth = width;
        line.endWidth = width;
        line.startColor = color;
        line.endColor = color;
    }

    void OnDestroy()
    {
        if (lineMaterial != null) Destroy(lineMaterial);
    }
}
