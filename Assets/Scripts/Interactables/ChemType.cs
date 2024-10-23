using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ChemType : int
{
    /* Full Name */
    WATER = 0,
    HYDROCHLORIC_ACID = 1,
    SODIUM_HYDROXIDE = 2,
    COPPER_SULFATE = 3,
    GLUCOSE = 4,

    /* Abbreviation Aliases */
    H2O = WATER,
    HCl = HYDROCHLORIC_ACID,
    NaOH = SODIUM_HYDROXIDE,
    CuSO4 = COPPER_SULFATE,
    C6H12O6 = GLUCOSE,
}
