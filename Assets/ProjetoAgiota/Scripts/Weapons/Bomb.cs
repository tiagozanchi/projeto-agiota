using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    private const float SELF_DESTROY_TIME = 0.5f;

    private void Start()
    {
        StartCoroutine(DestroyItself(SELF_DESTROY_TIME));
    }
}
