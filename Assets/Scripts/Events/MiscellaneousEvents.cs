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
            AudioEventManager.GloveSound();
            onPutOnLeftGlove();
        }
    }

    public event Action onPutOnRightGlove;
    public void PutOnRightGlove()
    {
        if (onPutOnRightGlove != null)
        {
            AudioEventManager.GloveSound();
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

    // Glassware related events
    public event Action<GameObject, bool> OnJarClosed;
    public void JarClosed(GameObject jar, bool isClosed) => OnJarClosed?.Invoke(jar, isClosed);

    public event Action OnScoopInJar;
    public void ScoopInJar() => OnScoopInJar?.Invoke();

    public event Action<float> OnUpdateMaterialHeld;
    public void UpdateMaterialHeld(float amount) => OnUpdateMaterialHeld?.Invoke(amount);

    public event Action OnAllowBoatTransfer;
    public void AllowBoatTransfer() => OnAllowBoatTransfer?.Invoke();

    public event Action OnCleanScale;
    public void CleanScale() => OnCleanScale?.Invoke();

    // Flask related events
    public event Action<bool> OnEnableFlaskTrigger;
    public void EnableFlaskTrigger(bool enable) => OnEnableFlaskTrigger?.Invoke(enable);

    public event Action OnTransferMaterialToGlass;
    public void TransferMaterialToGlass() => OnTransferMaterialToGlass?.Invoke();

    #region Scale Events
    public event Action OnScalePowerOn;
    public void ScalePowerOn() => OnScalePowerOn?.Invoke();

    public event Action OnScalePowerOff;
    public void ScalePowerOff() => OnScalePowerOff?.Invoke();

    public event Action OnScaleTare;
    public void ScaleTare() => OnScaleTare?.Invoke();

    public event Action OnScaleMode;
    public void ScaleMode() => OnScaleMode?.Invoke();

    public event Action<string> OnScaleModeChanged;
    public void ScaleModeChanged(string mode) => OnScaleModeChanged?.Invoke(mode);

    public event Action<bool> OnPaperInBoat;
    public void PaperInBoat(bool inBoat) => OnPaperInBoat?.Invoke(inBoat);

    public event Action OnObjectOnScale;
    public void ObjectOnScale() => OnObjectOnScale?.Invoke();
    #endregion
}
