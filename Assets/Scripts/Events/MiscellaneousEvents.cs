using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscellaneousEvents
{
    // Glove related events
    public event Action onPutOnLeftGlove;
    public void PutOnLeftGlove()
    {
        if (onPutOnLeftGlove != null)
        {
            onPutOnLeftGlove();
        }
    }

    public event Action onPutOnRightGlove;
    public void PutOnRightGlove()
    {
        if (onPutOnRightGlove != null)
        {
            onPutOnRightGlove();
        }
    }

    public event Action onTakeOffLeftGlove;
    public void TakeOffLeftGlove()
    {
        if (onTakeOffLeftGlove != null)
        {
            onTakeOffLeftGlove();
        }
    }

    public event Action onTakeOffRightGlove;
    public void TakeOffRightGlove()
    {
        if (onTakeOffRightGlove != null)
        {
            onTakeOffRightGlove();
        }
    }

    // Funnel related events
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

    // Printer related events
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
