using UnityEngine;

public class GlassTransfer : MonoBehaviour
{
    GameObject materialVisuals;
    bool canTransferToGlass;

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld += (amount) => materialVisuals = transform.GetChild(0).GetComponent<MaterialHold>().materialVisuals;
        GameEventsManager.instance.miscEvents.OnAllowBoatTransfer += () => canTransferToGlass = true;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnUpdateMaterialHeld -= (amount) => materialVisuals = transform.GetChild(0).GetComponent<MaterialHold>().materialVisuals;
        GameEventsManager.instance.miscEvents.OnAllowBoatTransfer -= () => canTransferToGlass = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canTransferToGlass) return;

        if (other.name.Contains("volumetricFlaskWrapLabel250mL"))
        {
            GameEventsManager.instance.miscEvents.UpdateMaterialHeld(-1);
            materialVisuals.SetActive(false);
            // Debug.Log("Material transferred to glass");

            GameEventsManager.instance.miscEvents.TransferMaterialToGlass();

            canTransferToGlass = false;
        }
    }
}
