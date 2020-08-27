using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrant : Weapon
{
    private const float SELF_DESTROY_TIME = 10f;
    private const float recoilDamage = 22f;

    private void Start()
    {
        StartCoroutine(DestroyItself(SELF_DESTROY_TIME));
    }

    private void OnTriggerEnter(Collider other)
    {
        checkCollisionAndDealDamage(other, recoilDamage);
    }
}
