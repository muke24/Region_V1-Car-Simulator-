using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeapon : MonoBehaviour
{
    public Animator anim;

    public int currentWeapon = 0;

    public int sniper = 1;
    public int pistol = 2;
    public int flagPistol = 3;

    public bool changeWeapon = false;

    public GameObject sniperGO;
    public GameObject pistolGO;
    public GameObject flagPistolGO;

    public Text flagText;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = sniper;
        flagText = GameObject.FindGameObjectWithTag("GamePlayCanvas").GetComponentInChildren<Text>();
        flagText.enabled = false;
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

        if (currentWeapon == flagPistol)
        {
            flagText.enabled = true;
        }
        else
        {
            flagText.enabled = false;
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
        anim.SetBool("Exit", true);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("TakeAwaySniper") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99)
        {
            sniperGO.SetActive(false);
            flagPistolGO.SetActive(true);
            changeWeapon = false;
            anim.SetBool("Exit", false);
        }        
    }
}
