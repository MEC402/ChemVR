using UnityEngine;

public class DevOpsManager : MonoBehaviour
{
    // Function to toggle the devOps boolean
    public void ToggleDevOps()
    {
        DevOpsState.ToggleDevOps();
    }

    // Function to check the current value of devOps
    public bool IsDevOpsEnabled()
    {
        return DevOpsState.IsDevOpsEnabled();
    }
}
