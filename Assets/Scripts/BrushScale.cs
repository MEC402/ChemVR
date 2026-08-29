using UnityEngine;

public class BrushScale : MonoBehaviour
{
    [SerializeField] private GameObject scale;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == scale)
            GameEventsManager.instance.miscEvents.CleanScale();
    }
}
