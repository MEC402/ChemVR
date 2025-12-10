using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonManager : MonoBehaviour
{
    public GameObject settingsCanvas;
    private bool settingsActive = false;

    public void TurnOnCanvas()
    {
        if (!settingsActive)
        {
            settingsCanvas.SetActive(true);
            settingsActive = true;
        }
    }

    public void TurnOffCanvas()
    {
        if (settingsActive)
        {
            settingsCanvas.SetActive(false);
            settingsActive = false;
        }
    }
}
