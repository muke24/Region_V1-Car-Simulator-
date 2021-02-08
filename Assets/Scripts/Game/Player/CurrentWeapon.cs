#region This code is written by Peter Thompson
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeapon : MonoBehaviour
{
	public Animator animator;
	public RuntimeAnimatorController sniperAnim;
	public RuntimeAnimatorController pistolAnim;
	public RuntimeAnimatorController meleeAnim;

	public int currentWeapon = 0;

	public int mainWeapon = 1;
	public int secondaryWeapon = 2;
	public int meleeWeapon = 3;
	public bool flag = false;

	public bool changeWeapon = false;

	public GameObject mainGO;
	public GameObject secondaryGO;
	public GameObject pistolArm;
	public GameObject flagGO;
	public GameObject meleeGO;

	public Text flagText;
	public Text weaponText;

	// Start is called before the first frame update
	void Start()
	{
		currentWeapon = mainWeapon;
		flagText = GameObject.FindGameObjectWithTag("GamePlayCanvas").GetComponentInChildren<Text>();
		flagText.enabled = false;
		weaponText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Canvas").Find("FPSCanv").Find("CurrentWeaponText").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update()
	{
		#region Weapon Change with scroll
		if (Input.GetAxis("Mouse ScrollWheel") < 0 && !flag)
		{
			if (currentWeapon < 3)
			{
				currentWeapon++;
			}
			else
			{
				currentWeapon = 1;
			}

			changeWeapon = true;
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0 && !flag)
		{
			if (currentWeapon > 1)
			{
				currentWeapon--;
			}
			else
			{
				currentWeapon = 3;
			}

			changeWeapon = true;
		}
		#endregion

		#region Weapon Check
		if (currentWeapon == mainWeapon && changeWeapon)
		{
			MainSwitch();
		}

		if (currentWeapon == secondaryWeapon && changeWeapon && !flag)
		{
			SecondarySwitch();
		}

		if (currentWeapon == secondaryWeapon && changeWeapon && flag)
		{
			FlagSwitch();
		}
		if (currentWeapon == meleeWeapon && changeWeapon)
		{
			MeleeSwitch();
		}

		#endregion

		#region Check if player has flag
		if (currentWeapon == secondaryWeapon && flag)
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
		if (currentWeapon == 2 && !flag)
		{
			weaponText.text = "Second Weapon";
		}
		if (currentWeapon == 2 && flag)
		{
			weaponText.text = "Flag";
		}
		if (currentWeapon == 3)
		{
			weaponText.text = "Melee";
		}
		#endregion
	}

	/// <summary>
	/// Switch to primary weapon
	/// </summary>
	void MainSwitch()
	{
		animator.SetBool("Exit", true);

		if (animator.name != sniperAnim.name)
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeAwayWeapon") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				mainGO.SetActive(true);
				flagGO.SetActive(false);
				pistolArm.SetActive(false);
				secondaryGO.SetActive(false);
				meleeGO.SetActive(false);
				changeWeapon = false;
				animator.runtimeAnimatorController = sniperAnim;
				animator.SetBool("Exit", false);
				animator.Play("TakeOutSniper", 0);
			}
		}
	}

	/// <summary>
	/// Switch to secondary weapon
	/// </summary>
	void SecondarySwitch()
	{
		animator.SetBool("Exit", true);

		if (animator.name != pistolAnim.name || animator.name == pistolAnim.name && flag)
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeAwayWeapon") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				mainGO.SetActive(false);
				flagGO.SetActive(false);
				pistolArm.SetActive(true);
				secondaryGO.SetActive(true);
				meleeGO.SetActive(false);
				changeWeapon = false;
				animator.runtimeAnimatorController = pistolAnim;
				animator.SetBool("Exit", false);
				animator.Play("TakeOutPistol", 0);
			}
		}
	}

	/// <summary>
	/// Switch to flag
	/// </summary>
	void FlagSwitch()
	{
		animator.SetBool("Exit", true);

		if (animator.name != pistolAnim.name || animator.name == pistolAnim.name && !flag)
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeAwayWeapon") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				mainGO.SetActive(false);
				flagGO.SetActive(true);
				pistolArm.SetActive(false);
				secondaryGO.SetActive(true);
				meleeGO.SetActive(false);
				changeWeapon = false;
				animator.runtimeAnimatorController = pistolAnim;
				animator.SetBool("Exit", false);
				animator.Play("TakeOutPistol", 0);
			}
		}
	}

	/// <summary>
	/// Switch to melee
	/// </summary>
	void MeleeSwitch()
	{
		animator.SetBool("Exit", true);

		if (animator.name != meleeAnim.name)
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeAwayWeapon") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				mainGO.SetActive(false);
				flagGO.SetActive(false);
				pistolArm.SetActive(false);
				secondaryGO.SetActive(false);
				meleeGO.SetActive(true);
				changeWeapon = false;
				animator.runtimeAnimatorController = pistolAnim;
				animator.SetBool("Exit", false);
				animator.Play("TakeOutMelee", 0);
			}
		}
	}
}
#endregion