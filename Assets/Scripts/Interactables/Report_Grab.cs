using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Report_Grab : MonoBehaviour
{
    public void Report_on_Grab()
    {
        GameEventsManager.instance.interactableEvents.PlayerGrabInteractable(this.gameObject);
    }
    public void Report_on_Drop()
    {
        GameEventsManager.instance.interactableEvents.PlayerDropInteractable(this.gameObject);
    }
    public void Report_on_Activate()
    {
        GameEventsManager.instance.interactableEvents.PlayerActivateInteractable(this.gameObject);
    }
}
