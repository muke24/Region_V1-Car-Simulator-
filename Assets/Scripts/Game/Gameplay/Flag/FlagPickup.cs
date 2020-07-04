using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPickup : MonoBehaviour
{
	public GameObject player;
	public CurrentWeapon currentWeapon;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		currentWeapon = player.GetComponent<CurrentWeapon>();
	}

	//void Update()
	//{
	//	//if (player == null)
	//	//{
	//	//	// Throw an error
	//	//	player = GameObject.FindGameObjectWithTag("Player");
	//	//	currentWeapon = player.GetComponent<CurrentWeapon>();
	//	//}
	//}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			currentWeapon.currentWeapon = currentWeapon.secondaryWeapon;
			currentWeapon.flag = true;
			currentWeapon.changeWeapon = true;
			gameObject.SetActive(false);
		}

		//if (other.CompareTag("TeamPlayer"))
		//{

		//}

		//if (other.CompareTag("Enemy"))
		//{

		//}
	}
}
