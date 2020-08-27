using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected const string CAR_TAG = "Car";

    protected IEnumerator DestroyItself(float selfDestroyTime)
    {
        yield return new WaitForSeconds(selfDestroyTime);
        Destroy(gameObject);
    }

    protected void checkCollisionAndDealDamage(Collider other, float damage)
    {
        if (other.CompareTag(CAR_TAG))
        {
            CarsController controller = other.gameObject.GetComponent<CarsController>();
            controller.TakeDamage(damage);
        }
    }
}
