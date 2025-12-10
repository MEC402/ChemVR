using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupToggle : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    


    // Start is called before the first frame update
    void Start()
    {
        if (myCollider == null)
        {
            Debug.Log(this.gameObject.name + " does not have the toggle collider set. Please assign.");
        }
    }


    public void ToggleCollider()
    {
        if (myCollider != null)
        {
            if (!myCollider.enabled)
            {
                myCollider.enabled = true;
            }
            else if (myCollider.enabled)
            {
                myCollider.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("Cannot toggle collider because it is not assigned.");
        }
    }
}
