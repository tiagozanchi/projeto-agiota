using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitroController : WeaponController
{
    public GameObject transparentGas;
    public GameObject gas;
    
    void Start()
    {
        getStreetCollider();

        transparentGas = Instantiate(transparentGas);
        transparentGas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;

        showWeaponHint(transparentGas);
        instantiateWeapon(gas);
    }

    void OnDisable()
    {
        transparentGas.SetActive(false);
    }
}
