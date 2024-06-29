using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{
    public event Action<int> onPlayerExperimentChange;
    public void PlayerExperimentChange(int experimentNumber)
    {
        if (onPlayerExperimentChange != null)
        {
            onPlayerExperimentChange(experimentNumber);
        }
    }

    public event Action<int> onPointsGained;
    public void PointsGained(int points)
    {
        if (onPointsGained != null)
        {
            onPointsGained(points);
        }
    }

    public event Action<int> onPlayerPointsChange;
    public void PlayerPointsChange(int points)
    {
        if (onPlayerPointsChange != null)
        {
            onPlayerPointsChange(points);
        }
    }

    public event Action<bool> onPlayerWearingGlovesChange;
    public void PlayerWearingGlovesChange(bool wearingGloves)
    {
        if (onPlayerWearingGlovesChange != null)
        {
            onPlayerWearingGlovesChange(wearingGloves);
        }
    }
}
