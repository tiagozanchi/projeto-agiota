using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Weapon
{
    private const float SELF_DESTROY_TIME = 10f;

    private void Start()
    {
        StartCoroutine(DestroyItself(SELF_DESTROY_TIME));
    }

}
