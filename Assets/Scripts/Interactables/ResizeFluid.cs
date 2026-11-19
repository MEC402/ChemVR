using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeFluid : MonoBehaviour
{
    [Range(0, 1)]
    public float fill = 0;

    public Color color;
    private Color topColor;
    private Color sideColor;
    public Vector2 remapBounds;

    public Material material;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        material = renderer.material;
        sideColor = color;
        topColor = color * 1.1f;
    }

    private void Update()
    {
        sideColor = color;
        topColor = color * 1.1f;
        material.SetFloat("_Fill", fill);
        material.SetColor("_TopColor", topColor);
        material.SetColor("_SideColor", sideColor);
        material.SetVector("_RemapBounds", remapBounds);

        // Plain Lit materials (no Chem Fluid shader) have no _TopColor/_SideColor, so tint
        // them directly. Lets simple meshes such as the dropper's liquid column show the chem colour.
        if (!material.HasProperty("_SideColor") && material.HasProperty("_BaseColor"))
        {
            material.SetColor("_BaseColor", color);
        }
    }

    private void OnDestroy()
    {
        Destroy(material);
    }

    public void SetColor(Color newColor)
    {
        // A fluid that starts hidden is activated and tinted in the same call, before Start has run.
        if (material == null) material = GetComponent<Renderer>().material;
        color = newColor;
        sideColor = color;
        topColor = color * 1.1f;
        material.SetColor("_TopColor", topColor);
        material.SetColor("_SideColor", sideColor);
    }
}
