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
    }

    private void OnDestroy()
    {
        Destroy(material);
    }
}
