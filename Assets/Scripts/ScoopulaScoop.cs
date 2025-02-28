using UnityEngine;

public class ScoopulaScoop : MonoBehaviour
{
    [SerializeField] float amountToScoop = 0.5f;
    [SerializeField] GameObject materialToHold;
    bool canTransfer;

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnScoopInJar += ScoopInJar;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnScoopInJar -= ScoopInJar;
    }

    private void ScoopInJar()
    {
        if (materialToHold == null) return;

        materialToHold.SetActive(true);
        canTransfer = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canTransfer) return;

        if (other.name.Contains("boat"))
        {
            materialToHold.SetActive(false);

            GameEventsManager.instance.miscEvents.UpdateMaterialHeld(amountToScoop);

            canTransfer = false;
        }
    }
}
