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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CAR_TAG))
        {
            CarsController controller = other.gameObject.GetComponent<CarsController>();
            controller.TakeDamage();
        }
    }
}
