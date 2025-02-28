using UnityEngine;

public class MaterialHold : MonoBehaviour
{
    [SerializeField] Collider triggerCollider;
    [SerializeField] Put_Paper_on_Boat putPaperOnBoat;
    [SerializeField] GameObject materialVisuals;

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScoopInJar += EnableTriggerCollider;
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld += AddMaterialToBoat;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScoopInJar -= EnableTriggerCollider;
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld -= AddMaterialToBoat;
    }

    private void EnableTriggerCollider()
    {
        if (triggerCollider == null) return;

        if (putPaperOnBoat != null && putPaperOnBoat.isInBoat)
            triggerCollider.enabled = true;
    }

    private void AddMaterialToBoat(float amount)
    {
        if (putPaperOnBoat == null) return;

        if (putPaperOnBoat.isInBoat)
            materialVisuals.SetActive(true);
    }
}
