using UnityEngine;

public class ScoopulaScoop : MonoBehaviour
{
    [SerializeField] float minScoop, maxScoop;
    [SerializeField] GameObject materialToHold;
    float amountToScoop;
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

        amountToScoop = Random.Range(minScoop, maxScoop);

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
