using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Turn_Hot_Plate_On : TaskStep
{
    protected override void SetTaskStepState(string state)
    {
        throw new System.NotImplementedException();
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnBeakerToLevelThree += CallDelay;
    }
    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnBeakerToLevelThree -= CallDelay;
    }

    void CallDelay()
    {
        StartCoroutine(DelayThenFinish());
    }

    IEnumerator DelayThenFinish()
    {
        yield return new WaitForSeconds(2f); // delay for 2 seconds
        FinishTaskStep();
    }

}
