using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirWithStirRod : MonoBehaviour
{
    /*How Stir Rod now works is the container to be stirred needs the "StirAccepter" script attached to a 
        child gameObject named "StirAccepter", with the Is Stir Target checked and the stir Name determined, so that we can automatically match
        stirring targets together. That way if multiple stirs need done, we can force the interactions to be with the right containers, rather than
        this working on any potential stir target.
    */
    [SerializeField] private string stirName;
    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("StirAccepter"))
        {
            if(other.TryGetComponent<StirAccepter>(out StirAccepter stirAccepter))
            {
                if(stirAccepter.TryStirTarget(stirName))
                {
                    GameEventsManager.instance.miscEvents.StirBeaker();
                }
            }
        }
    }
}
