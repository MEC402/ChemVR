using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int startingExperiment = 0;
    [SerializeField] private int startingPoints = 0;
    [SerializeField] private bool initialWearingGloves = false;

    private int currentExperiment;
    private int currentPoints;
    private bool wearingGloves;

    private void Awake()
    {
        currentExperiment = startingExperiment;
        currentPoints = startingPoints;
        wearingGloves = initialWearingGloves;
    }

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.onPointsGained += PointsGained;
        GameEventsManager.instance.playerEvents.onPlayerExperimentChange += ExperimentChange;
        GameEventsManager.instance.playerEvents.onPlayerWearingGlovesChange += WearingGlovesChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onPointsGained -= PointsGained;
        GameEventsManager.instance.playerEvents.onPlayerExperimentChange -= ExperimentChange;
        GameEventsManager.instance.playerEvents.onPlayerWearingGlovesChange -= WearingGlovesChange;
    }

    private void Start()
    {
        GameEventsManager.instance.playerEvents.PlayerExperimentChange(currentExperiment);
        GameEventsManager.instance.playerEvents.PlayerPointsChange(currentPoints);
        GameEventsManager.instance.playerEvents.PlayerWearingGlovesChange(wearingGloves);
    }

    private void PointsGained(int points)
    {
        currentPoints += points;
        GameEventsManager.instance.playerEvents.PlayerPointsChange(currentPoints);
    }

    private void WearingGlovesChange(bool wearingGloves) 
    {
        this.wearingGloves = wearingGloves;
        GameEventsManager.instance.playerEvents.PlayerWearingGlovesChange(wearingGloves);
    }

    private void ExperimentChange(int experiment)
    {
        currentExperiment = experiment;
        GameEventsManager.instance.playerEvents.PlayerExperimentChange(experiment);
    }
}
