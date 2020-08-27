using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : WeaponController
{

    public GameObject bomb;
    public GameObject crosshair;

    
    // Start is called before the first frame update
    void Start()
    {
        getStreetCollider();

        crosshair = Instantiate(crosshair);
        crosshair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;

        showWeaponHint(crosshair);
        instantiateWeapon(bomb);
    }

    void OnDisable()
    {
        crosshair.SetActive(false);
    }
}
