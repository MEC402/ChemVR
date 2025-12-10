using UnityEngine;

public class MaterialHold : MonoBehaviour
{
    [SerializeField] Collider triggerCollider;
    [SerializeField] Put_Paper_on_Boat putPaperOnBoat;
    public GameObject materialVisuals;

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScoopInJar += EnableTriggerCollider;
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld += AddMaterialToBoat;

        GameEventsManager.instance.miscEvents.OnAllowBoatTransfer += () => triggerCollider.enabled = false;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScoopInJar -= EnableTriggerCollider;
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld -= AddMaterialToBoat;

        GameEventsManager.instance.miscEvents.OnAllowBoatTransfer -= () => triggerCollider.enabled = false;
    }

    private void EnableTriggerCollider()
    {
        if (triggerCollider == null) return;

        if (putPaperOnBoat != null && putPaperOnBoat.isInBoat)
            triggerCollider.enabled = true;
        Debug.Log("Trigger collider: " + triggerCollider);
    }

    private void AddMaterialToBoat(float amount)
    {
        if (putPaperOnBoat == null) return;

        if (putPaperOnBoat.isInBoat)
            materialVisuals.SetActive(true);
            Debug.Log("material visuals: "+ materialVisuals);
    }
}
