using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourActivator : MonoBehaviour
{
    [SerializeField]
    private bool activated = false;

    public bool IsActivated() {
        return activated;
    }

    public void Activate() {
        activated = true;
    }

    public void Deactivate() {
        activated = false;
    }

    public void Toggle() {
        activated = !activated;
    }
}
