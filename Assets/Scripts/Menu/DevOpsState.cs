using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DevOpsState
{
    // The devOps flag, default to false
    private static bool devOps = true; //for friday deadline. change back before student testing builds

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

