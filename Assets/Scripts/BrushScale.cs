using UnityEngine;

public class BrushScale : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scale"))
            GameEventsManager.instance.miscEvents.CleanScale();
    }
}
