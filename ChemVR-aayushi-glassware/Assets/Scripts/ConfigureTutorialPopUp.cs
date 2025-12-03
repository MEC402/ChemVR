using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigureTutorialPopUp : MonoBehaviour
{
    private bool skipFirst = true;

    public void Start()
    {
        foreach (Transform child in transform)
        {
            if (skipFirst) // The first tutorial text should always be visible.
            {
                skipFirst = false;
            } else
            {
                GameObject text = child.transform.GetChild(0).gameObject;
                text.SetActive(false);
            }
        }
    }
}
