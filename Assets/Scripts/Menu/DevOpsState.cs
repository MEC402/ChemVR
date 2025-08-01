using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DevOpsState
{
    // The devOps flag, default to false
    private static bool devOps = false; //change this line for default on build

    // Function to toggle the devOps boolean
    public static void ToggleDevOps()
    {
        devOps = !devOps;
    }

    // Function to check the current value of devOps
    public static bool IsDevOpsEnabled()
    {
        return devOps;
    }
}

