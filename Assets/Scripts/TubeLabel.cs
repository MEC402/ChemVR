using System;
using TMPro;
using UnityEngine;

/// <summary>
/// A written label ("1", "2", "3") on a test tube. Hidden until the Sharpie touches the tube
/// (see <see cref="SharpieMarker"/>), then shown and always turned toward the player.
/// </summary>
public class TubeLabel : MonoBehaviour
{
    [Tooltip("The label object (a TextMeshPro 3D text) shown once the tube has been marked.")]
    public GameObject label;

    /// <summary>Raised the first time a tube is labeled, with its number.</summary>
    public static event Action<TubeLabel> OnLabeled;

    [Tooltip("How far from the tube's centre line the label sits (metres), on the side facing the player.")]
    public float surfaceOffset = 0.034f;

    public bool IsLabeled { get; private set; }
    float yOffset;

    /// <summary>The tube's number, taken from the label text.</summary>
    public string Number
    {
        get
        {
            var t = label != null ? label.GetComponent<TMP_Text>() : null;
            return t != null ? t.text : "";
        }
    }

    void Awake()
    {
        if (label != null)
        {
            yOffset = label.transform.position.y - transform.position.y;
            label.SetActive(false);
        }
    }

    public void Reveal()
    {
        if (IsLabeled || label == null) return;
        IsLabeled = true;
        label.SetActive(true);
        OnLabeled?.Invoke(this);
    }

    public void Hide()
    {
        IsLabeled = false;
        if (label != null) label.SetActive(false);
    }

    void LateUpdate()
    {
        if (!IsLabeled || label == null) return;
        var cam = Camera.main;
        if (cam == null) return;
        // Keep the text on the side of the tube facing the player and turned toward them (yaw only).
        Vector3 toCam = cam.transform.position - transform.position;
        toCam.y = 0f;
        if (toCam.sqrMagnitude < 0.0001f) return;
        toCam.Normalize();
        label.transform.position = transform.position + toCam * surfaceOffset + Vector3.up * yOffset;
        label.transform.rotation = Quaternion.LookRotation(-toCam, Vector3.up);
    }
}
