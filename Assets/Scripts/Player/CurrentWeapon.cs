using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentWeapon : MonoBehaviour
{
    public Animator sniperAnim;
    public Animator pistolAnim;
    public Animator flagAnim;

    public int currentWeapon = 0;

    public int sniper = 1;
    public int pistol = 2;
    public int flagPistol = 3;

    public bool changeWeapon = false;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = sniper;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon == sniper && changeWeapon)
        {
            SniperSwitchOut();
        }

        if (currentWeapon == pistol && changeWeapon)
        {
            PistolSwitchOut();
        }

        if (currentWeapon == flagPistol && changeWeapon)
        {
            FlagSwitchOut();
        }
    }

    void SniperSwitchOut()
    {
        changeWeapon = false;

    }

    void PistolSwitchOut()
    {
        changeWeapon = false;
    }

    void FlagSwitchOut()
    {
        changeWeapon = false;
    }
}
