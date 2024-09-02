using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEvents
{
    public event Action onShowParticles;
    public void ShowParticles()
    {
        if (onShowParticles != null)
        {
            onShowParticles();
        }
    }

    public event Action onHideParticles;
    public void HideParticles()
    {
        if (onHideParticles != null)
        {
            onHideParticles();
        }
    }

    public event Action onDeleteParticles;
    public void DeleteParticles()
    {
        if (onDeleteParticles != null)
        {
            onDeleteParticles();
        }
    }
}
