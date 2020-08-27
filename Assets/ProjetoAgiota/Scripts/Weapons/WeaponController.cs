using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    private Collider streetCollider;

    protected void getStreetCollider() 
    {
        GameObject street = GameObject.FindGameObjectWithTag("Street");
        streetCollider = street.GetComponent<Collider>();
        if (street == null || streetCollider == null) Debug.LogError("There's no object with tag Street, or street has no collider");
    }

    protected void showWeaponHint(GameObject hintObj) 
    {
        if (hintObj == null) return;

        Ray movementRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit streetData;

        if (streetCollider.Raycast(movementRay, out streetData, 100))
        {
            Vector3 newPos = new Vector3(streetData.point.x, 0.5f, streetData.point.z);          
            hintObj.SetActive(true);
            hintObj.transform.position = newPos;
        }
        else
        {
            hintObj.SetActive(false);
        }
    }

    protected void instantiateWeapon (GameObject weapon)
    {
        if (weapon == null) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (streetCollider.Raycast(ray, out hit, 100))
            {
                GameObject gameObject = Instantiate(weapon);
                Vector3 newPos = new Vector3(hit.point.x, 0.5f, hit.point.z);
                gameObject.transform.position = newPos;
            }
        }
    }
}
