using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourActivator : MonoBehaviour
{
    [SerializeField]
    private bool activated = false;
    public bool isPercentActivated; // Does this use something that controls flow or is it like a cap?
    float flow; // This is a float from 0 to 1

    public void setFlow(float flow)
    {
        this.flow = flow;
    }

    public float getFlow()
    {
        return flow;
    }

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
