using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glove_Hygiene_Task_5 : TaskStep
{
    bool funnelOn;
    bool buretOn;
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        funnelOn = false;
        buretOn = false;
        GameEventsManager.instance.miscEvents.onBuretSnaptoHolder += Buret2Hold;
        GameEventsManager.instance.miscEvents.onFunnelSnaptoBuret += Funnel2Buret;
        GameEventsManager.instance.miscEvents.onBuretUnSnaptoHolder += BuretOFFHold;
        GameEventsManager.instance.miscEvents.onFunnelUnSnaptoBuret += FunnelOFFBuret;
    }

    private void Funnel2Buret()
    {
        funnelOn = true;
        if (buretOn)
        {
            FinishTaskStep();
        }
    }

    private void Buret2Hold()
    {
        buretOn = true;
        if (funnelOn)
        {
            FinishTaskStep();
        }
    }
    private void FunnelOFFBuret()
    {
        funnelOn = false;
    }

    private void BuretOFFHold()
    {
        buretOn = false;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onBuretSnaptoHolder -= Buret2Hold;
        GameEventsManager.instance.miscEvents.onFunnelSnaptoBuret -= Funnel2Buret;
        GameEventsManager.instance.miscEvents.onBuretUnSnaptoHolder -= BuretOFFHold;
        GameEventsManager.instance.miscEvents.onFunnelUnSnaptoBuret -= FunnelOFFBuret;
    }
}
