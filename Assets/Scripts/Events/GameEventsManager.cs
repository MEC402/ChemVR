using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public ChemistryEvents chemistryEvents;
    public TaskEvents taskEvents;
    public InteractableEvents interactableEvents;
    public PlayerEvents playerEvents;
    public InputEvents inputEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;

        // initialize all events
        chemistryEvents = new ChemistryEvents();
        taskEvents = new TaskEvents();
        interactableEvents = new InteractableEvents();
        playerEvents = new PlayerEvents();
        inputEvents = new InputEvents();
    }
}
