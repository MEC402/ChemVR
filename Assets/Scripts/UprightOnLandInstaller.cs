using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


/// <summary>
/// Drop one of these in a scene to give every grabbable object an <see cref="UprightOnLand"/>
/// at startup, instead of adding the component to each object by hand. Anything that already
/// has the component keeps its own hand-tuned settings.
/// </summary>
public class UprightOnLandInstaller : MonoBehaviour
{
    [Tooltip("Objects that are meant to lie flat (paper, weigh boats, tongs...). These are skipped.")]
    [SerializeField] private List<GameObject> exclude = new();

    [Tooltip("Objects on these layers are skipped.")]
    [SerializeField] private LayerMask excludeLayers;

    [Tooltip("Objects with any of these tags are skipped.")]
    [SerializeField] private List<string> excludeTags = new();

    private void Start()
    {
        int added = 0;
        XRGrabInteractable[] grabbables = FindObjectsByType<XRGrabInteractable>(FindObjectsSortMode.None);

        foreach (XRGrabInteractable grabbable in grabbables)
        {
            GameObject target = grabbable.gameObject;

            if (exclude.Contains(target)) continue;
            if (excludeTags.Contains(target.tag)) continue;
            if ((excludeLayers.value & (1 << target.layer)) != 0) continue;
            if (target.GetComponent<UprightOnLand>() != null) continue;
            if (target.GetComponent<Rigidbody>() == null) continue;

            target.AddComponent<UprightOnLand>();
            added++;
        }

        Debug.Log($"UprightOnLandInstaller: added UprightOnLand to {added} of {grabbables.Length} grabbables.");
    }
}
