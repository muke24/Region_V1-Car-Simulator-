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
    public AnimatorController meleeAnim;

    public int currentWeapon = 0;

    public int mainWeapon = 1;
    public int secondaryWeapon = 2;
    public int melee = 3;
    public bool flagPistol = false;

    public bool changeWeapon = false;

    public GameObject mainGO;
    public GameObject secondaryGO;
    public GameObject flagPistolGO;
    public GameObject meleeGO;

    public Text flagText;
    public Text weaponText;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = mainWeapon;
        flagText = GameObject.FindGameObjectWithTag("GamePlayCanvas").GetComponentInChildren<Text>();
        flagText.enabled = false;        
    }

    // Update is called once per frame
    void Update()
    {
		#region Weapon Change with scroll
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentWeapon < 3)
            {
                currentWeapon++;
            }
            else
            {
                currentWeapon = 1;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentWeapon > 1)
            {
                currentWeapon--;
            }
            else
            {
                currentWeapon = 3;
            }
        }
		#endregion

		#region Weapon Check
		if (currentWeapon == mainWeapon && changeWeapon)
        {
            MainSwitch();
        }		

		if (currentWeapon == secondaryWeapon && changeWeapon && !flagPistol)
        {
            SecondarySwitch();
        }

        if (currentWeapon == secondaryWeapon && changeWeapon && flagPistol)
        {
            FlagSwitch();
        }

		#endregion

		#region Check if player has flag
		if (currentWeapon == secondaryWeapon && flagPistol)
        {
            flagText.enabled = true;
        }
        else
        {
            flagText.enabled = false;
        }
		#endregion


		#region Weapon Text Change for debugging
		if (currentWeapon == 0)
        {
            weaponText.text = "None";
        }
        if (currentWeapon == 1)
        {
            weaponText.text = "Main Weapon";
        }
        if (currentWeapon == 2 && !flagPistol)
        {
            weaponText.text = "Second Weapon";
        }
        if (currentWeapon == 2 && flagPistol)
        {
            weaponText.text = "Flag";
        }
        if (currentWeapon == 3)
        {
            weaponText.text = "Melee";
        }
		#endregion
	}

	void MainSwitch()
    {
        animator.SetBool("Exit", false);

        if (animator.name != sniperAnim.name)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeAwayWeapon") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99)
            {
                mainGO.SetActive(true);
                flagPistolGO.SetActive(false);
                secondaryGO.SetActive(false);
                meleeGO.SetActive(false);
                changeWeapon = false;
                animator.runtimeAnimatorController = sniperAnim;
                animator.SetBool("Exit", true);
                animator.SetBool("Start", true);
            }
        }        
    }

    void SecondarySwitch()
    {
        animator.SetBool("Exit", true);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeAwaySniper") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99)
        {
            mainGO.SetActive(false);
            secondaryGO.SetActive(true);
            changeWeapon = false;
            animator.runtimeAnimatorController = pistolAnim;
        }
    }

    void FlagSwitch()
    {
        animator.SetBool("Exit", true);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeAwaySniper") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99)
        {
            mainGO.SetActive(false);
            flagPistolGO.SetActive(true);
            changeWeapon = false;
            animator.runtimeAnimatorController = pistolAnim;
        }        
    }
    void MeleeSwitch()
    {
        animator.SetBool("Exit", true);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeAwaySniper") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99)
        {
            mainGO.SetActive(false);
            flagPistolGO.SetActive(true);
            changeWeapon = false;
            animator.runtimeAnimatorController = pistolAnim;
        }
    }
}
