using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject sniper;
    public GameObject assaultRifle;
    public GameObject pistol;
    public GameObject fists;
    public bool weaponChanging = false;

    
    public int weaponID = 0;
    public int prevWeaponID = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponCheck();
    }

    void WeaponCheck()
    {
        if (fists.activeSelf)
        {
            weaponID = 0;
        }

        if (sniper.activeSelf)
        {
            weaponID = 1;
        }

        if (assaultRifle.activeSelf)
        {
            weaponID = 2;
        }

        if (pistol.activeSelf)
        {
            weaponID = 3;
        }
    }
}
