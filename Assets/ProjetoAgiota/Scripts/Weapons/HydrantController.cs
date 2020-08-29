using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrantController : WeaponController
{
    public GameObject hydrant;
    public GameObject hydrantTransparent;

    // Start is called before the first frame update
    void Start()
    {
        getStreetCollider();

        hydrantTransparent = Instantiate(hydrantTransparent);
        hydrantTransparent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;

        showWeaponHint(hydrantTransparent);
        instantiateWeapon(hydrant);
    }

    protected override void showWeaponHint(GameObject hintObj)
    {
        if (hintObj == null) return;

        Ray movementRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit streetData;

        if (streetCollider.Raycast(movementRay, out streetData, 100))
        {
            bool onLeft = streetData.point.x <= 0;
            float xPos = onLeft ? -15f : 15f;
            Vector3 newPos = new Vector3(xPos, 0.5f, streetData.point.z);
            hintObj.SetActive(true);
            hintObj.transform.position = newPos;
            int yRot = onLeft ? 0 : 180;
            hintObj.transform.rotation = Quaternion.Euler(0, yRot, 0);
        }
        else
        {
            hintObj.SetActive(false);
        }
    }

    protected override void instantiateWeapon(GameObject weapon) 
    {
        if (weapon == null) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (streetCollider.Raycast(ray, out hit, 100))
            {
                GameObject gameObject = Instantiate(weapon);
                bool onLeft = hit.point.x <= 0;
                float xPos = onLeft ? -15f : 15f;
                Vector3 newPos = new Vector3(xPos, 0.5f, hit.point.z);
                gameObject.transform.position = newPos;

                if (!onLeft) 
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                OnUse?.Invoke(_weaponCooldown);
            }
        }
    }
    void OnDisable()
    {
        if(hydrantTransparent != null) hydrantTransparent.SetActive(false);
    }
}
