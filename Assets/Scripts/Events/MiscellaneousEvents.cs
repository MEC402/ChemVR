using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscellaneousEvents
{
    public event Action onBuretSnaptoHolder;
    public void BuretSnaptoHolder()
    {
        if (onBuretSnaptoHolder != null)
        {
            onBuretSnaptoHolder();
        }
    }

    public event Action onFunnelSnaptoBuret;
    public void FunnelSnaptoBuret()
    {
        if (onFunnelSnaptoBuret != null)
        {
            onFunnelSnaptoBuret();
        }
    }
    public event Action onBuretUnSnaptoHolder;
    public void BuretUnSnaptoHolder()
    {
        if (onBuretUnSnaptoHolder != null)
        {
            onBuretUnSnaptoHolder();
        }
    }

    public event Action onFunnelUnSnaptoBuret;
    public void FunnelUnSnaptoBuret()
    {
        if (onFunnelUnSnaptoBuret != null)
        {
            onFunnelUnSnaptoBuret();
        }
    }

    public event Action onPrinterSlap;
    public void PrinterSlap()
    {
        if (onPrinterSlap != null)
        {
            onPrinterSlap();
        }
    }
}
