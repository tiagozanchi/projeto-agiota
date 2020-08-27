using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class BlockController : WeaponController
{

    public GameObject transparentBlock;
    public GameObject block;

    // Start is called before the first frame update
    void Start()
    {
        getStreetCollider();

        transparentBlock = Instantiate(transparentBlock);
        transparentBlock.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;

        showWeaponHint(transparentBlock);
        instantiateWeapon(block);
    }
}
