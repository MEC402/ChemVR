using UnityEngine;

public class DevOpsManager : MonoBehaviour
{
    // Static instance for the Singleton pattern
    public static DevOpsManager instance;
    public MeshRenderer CheckMark;

    // The devOps flag, default to false
    private bool devOps = false;

    // Ensure the instance persists between scene loads
    private void Awake()
    {
        // Check if instance already exists and destroy the new one if so
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Preserve this object across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy the duplicate instance
        }
    }

    // Function to toggle the devOps boolean
    public void ToggleDevOps()
    {
        devOps = !devOps;
        //Debug.Log("DevOps mode is now: " + (devOps ? "On" : "Off"));

        //Toggle checkbox on and off
        CheckMark.enabled = devOps;
    }

    // Function to check the current value of devOps
    public bool IsDevOpsEnabled()
    {
        return devOps;
    }
}
