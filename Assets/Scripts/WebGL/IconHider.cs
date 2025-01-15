using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconHider : MonoBehaviour
{
    [SerializeField] WebGLInput input;
    [SerializeField] GameObject[] icons;


    private void OnEnable()
    {
        input.OnPausePressed += ToggleIcons;
    }

    private void OnDisable()
    {
        input.OnPausePressed -= ToggleIcons;
    }

    private void ToggleIcons()
    {
        foreach (GameObject icon in icons)
            icon.SetActive(!icon.activeSelf);
    }
}
