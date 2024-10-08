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

    // Burret related events
    public event Action<GameObject, GameObject> onBuretSnaptoHolder;
    public void BuretSnaptoHolder(GameObject buret, GameObject holder)
    {
        if (onBuretSnaptoHolder != null)
        {
            onBuretSnaptoHolder(buret, holder);
        }
    }
    public event Action<GameObject, GameObject> onBuretUnSnaptoHolder;
    public void BuretUnSnaptoHolder(GameObject buret, GameObject holder)
    {
        if (onBuretUnSnaptoHolder != null)
        {
            onBuretUnSnaptoHolder(buret, holder);
        }
    }
    // Funnel related events
    public event Action<GameObject, GameObject> onFunnelSnaptoBuret;
    public void FunnelSnaptoBuret(GameObject funnel, GameObject buret)
    {
        if (onFunnelSnaptoBuret != null)
        {
            onFunnelSnaptoBuret(funnel, buret);
        }
    }
    public event Action<GameObject, GameObject> onFunnelUnSnaptoBuret;
    public void FunnelUnSnaptoBuret(GameObject funnel, GameObject buret)
    {
        if (onFunnelUnSnaptoBuret != null)
        {
            onFunnelUnSnaptoBuret(funnel, buret);
        }
    }
    // Printer related events

    public event Action onPrinterSlap;
    public void PrinterSlap()
    {
        if (onPrinterSlap != null)
        {
            onPrinterSlap();
        }
    }

    // Table related events
    public event Action<GameObject, GameObject> onItemOnTable;
    public void ItemOnTable(GameObject item, GameObject table)
    {
        if (onItemOnTable != null)
        {
            onItemOnTable(item, table);
        }
    }
    public event Action<GameObject, GameObject> onItemOffTable;
    public void ItemOffTable(GameObject item, GameObject table)
    {
        if (onItemOffTable != null)
        {
            onItemOffTable(item, table);
        }
    }

    // Pencil to paper event
    public event Action onPencilOnPaper;
    public void PencilOnPaper()
    {
        if (onPencilOnPaper != null)
        {
            onPencilOnPaper();
        }
    }

    public event Action onTextPopUp;
    public void TextPopUp()
    {
        if (onTextPopUp != null)
        {
            onTextPopUp();
        }
    }

    // Set target for hint
    public event Action<GameObject> onSetHint;
    public void SetHint(GameObject target)
    {
        if (onSetHint != null)
        {
            onSetHint(target);
        }
    }
}
