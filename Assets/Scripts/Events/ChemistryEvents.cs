using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistryEvents
{
    public event Action<ChemContainer, ChemFluid> onPourOut;
    public void PourOut(ChemContainer origin, ChemFluid portion) {
        if (onPourOut != null) {
            onPourOut(origin, portion);
        }
    }

    public event Action<ChemContainer, ChemFluid> onPourIn;
    public void PourIn(ChemContainer recipient, ChemFluid portion) {
        if (onPourIn != null) {
            onPourIn(recipient, portion);
        }
    }

    public event Action<ChemContainer, ChemFluid> onFluidDispose;
    public void FluidDispose(ChemContainer disposal, ChemFluid portion) {
        if (onFluidDispose != null) {
            onFluidDispose(disposal, portion);
        }
    }

}
