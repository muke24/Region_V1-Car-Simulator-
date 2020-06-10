using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeapon : MonoBehaviour
{
    public Animator animator;
    public AnimatorController sniperAnim;
    public AnimatorController pistolAnim;
    public AnimatorController flagAnim;

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
            SniperSwitch();
        }

        if (currentWeapon == pistol && changeWeapon)
        {
            PistolSwitch();
        }

        if (currentWeapon == flagPistol && changeWeapon)
        {
            FlagSwitch();
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

    void SniperSwitch()
    {
        animator.SetBool("Exit", false);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeAwayFlag") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99)
        {
            sniperGO.SetActive(true);
            flagPistolGO.SetActive(false);
            changeWeapon = false;
            animator.runtimeAnimatorController = sniperAnim;
        }
    }

    void PistolSwitch()
    {
        changeWeapon = false;
    }

    void FlagSwitch()
    {
        animator.SetBool("Exit", true);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeAwaySniper") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99)
        {
            sniperGO.SetActive(false);
            flagPistolGO.SetActive(true);
            changeWeapon = false;
            animator.runtimeAnimatorController = flagAnim;
        }        
    }
}
