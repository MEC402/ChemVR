using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attach_Funnel : TaskStep
{
    HashSet<GameObject> buretsOnHolders = new HashSet<GameObject>();
    HashSet<GameObject> buretsWithFunnels = new HashSet<GameObject>();
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.SetHint(null);

        GameEventsManager.instance.miscEvents.onBuretSnaptoHolder += Buret2Hold;
        GameEventsManager.instance.miscEvents.onFunnelSnaptoBuret += Funnel2Buret;
        GameEventsManager.instance.miscEvents.onBuretUnSnaptoHolder += BuretOFFHold;
        GameEventsManager.instance.miscEvents.onFunnelUnSnaptoBuret += FunnelOFFBuret;
    }

    private void Funnel2Buret(GameObject funnel, GameObject buret)
    {
        buretsWithFunnels.Add(buret);
        if(buretsOnHolders.Contains(buret))
        {
            FinishTaskStep();
        }
    }

    private void Buret2Hold(GameObject buret, GameObject holder)
    {
        buretsOnHolders.Add(buret);
        if (buretsWithFunnels.Contains(buret))
        {
            FinishTaskStep();
        }
    }
    private void FunnelOFFBuret(GameObject funnel, GameObject buret)
    {
        buretsWithFunnels.Remove(buret);
    }

    private void BuretOFFHold(GameObject buret, GameObject holder)
    {
        buretsOnHolders.Remove(buret);
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onBuretSnaptoHolder -= Buret2Hold;
        GameEventsManager.instance.miscEvents.onFunnelSnaptoBuret -= Funnel2Buret;
        GameEventsManager.instance.miscEvents.onBuretUnSnaptoHolder -= BuretOFFHold;
        GameEventsManager.instance.miscEvents.onFunnelUnSnaptoBuret -= FunnelOFFBuret;
    }
}
