using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private SoundManager.Sounds _soundToPlayOnHit;
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
            if (damage < 0)
            {
                controller.Boost();
                Destroy(gameObject);
            }
            else
            {
                controller.TakeDamage(damage);
            }
            GameManager.Instance.SoundManager.Play(_soundToPlayOnHit, GameManager.Instance.GetComponent<AudioSource>());
        }
    }
}
