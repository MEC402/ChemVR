using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    private Animator anime;

    void Start()
    {
        anime = this.GetComponent<Animator>();
    }
}
