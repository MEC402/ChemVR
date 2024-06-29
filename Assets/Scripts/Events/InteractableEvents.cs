using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableEvents
{
    public event Action<GameObject> onPlayerGrabInteractable;
    public void PlayerGrabInteractable(GameObject interactable)
    {
        if (onPlayerGrabInteractable != null)
        {
            onPlayerGrabInteractable(interactable);
        }
    }

    public event Action<GameObject> onPlayerDropInteractable;
    public void PlayerDropInteractable(GameObject interactable)
    {
        if (onPlayerDropInteractable != null)
        {
            onPlayerDropInteractable(interactable);
        }
    }

    public event Action<GameObject> onPlayerActivateInteractable;
    public void PlayerActivateInteractable(GameObject interactable)
    {
        if (onPlayerActivateInteractable != null)
        {
            onPlayerActivateInteractable(interactable);
        }
    }
}
