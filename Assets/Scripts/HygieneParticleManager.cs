using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HygieneParticleManager : MonoBehaviour
{
    Renderer myRenderer;

    void Start()
    {
        myRenderer = this.GetComponent<MeshRenderer>();
        if(myRenderer == null)
        {
            Debug.LogError("No Renderer attached");
        }

        GameEventsManager.instance.partEvents.onShowParticles += showMe;
        GameEventsManager.instance.partEvents.onHideParticles += hideMe;
        GameEventsManager.instance.partEvents.onDeleteParticles += deleteMe;
    }

    private void showMe()
    {
        myRenderer.enabled = true;
    }

    private void hideMe()
    {
        myRenderer.enabled = false;
    }

    private void deleteMe()
    {
        Destroy(this);
    }
}
