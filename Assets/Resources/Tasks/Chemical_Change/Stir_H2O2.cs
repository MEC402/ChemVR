using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Stir_H2O2 : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnStirBeaker += StirEvent;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnStirBeaker -= StirEvent;
    }

    void StirEvent()
    {
        ParticleSystem NaOHps = GameObject.Find("NaOHStirEventParticleSystem")?.GetComponent<ParticleSystem>();
        ParticleSystem H2O2ps = GameObject.Find("H2O2StirEventParticleSystem")?.GetComponent<ParticleSystem>();
        NaOHps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (H2O2ps != null)
        {
            H2O2ps.Play();
            //Debug.Log("Particle system found.");
        }
        else
        {
            Debug.LogWarning("Particle system not found.");
        }

        ChemContainer CC = GameObject.Find("BeakerUp250mL_largerText")?.GetComponent<ChemContainer>();
        //ChemFluid CF = GameObject.Find("BeakerUp250mL_largerText")?.GetComponent<ChemFluid>();
        if (CC != null)
        {
            CC.EmptyChem();
            Debug.Log("ChemFluid set to empty!");
            CC.AddChem(ChemType.ChemicalMix, 54);
            Debug.Log("Color Changed!");
        }
        else
        {
            Debug.LogWarning("ChemFluid not found!");
        }
            FinishTaskStep();
    }

}
