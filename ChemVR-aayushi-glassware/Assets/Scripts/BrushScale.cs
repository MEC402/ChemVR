using UnityEngine;

public class BrushScale : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Scale"))
            GameEventsManager.instance.miscEvents.CleanScale();
    }
}
