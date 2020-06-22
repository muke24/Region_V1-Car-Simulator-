using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
	//public CurrentWeapon currentWeapon;
	public GameObject flag;

	public Material enemyMat;

	// Start is called before the first frame update
	void Start()
	{
		if (!GameMode.captureTheFlag)
		{
			Destroy(gameObject);
		}

		if (GameMode.captureTheFlag)
		{
			enemyMat = Resources.Load<Material>("Materials/Base/EnemyBase");

			Renderer renderer = GetComponent<Renderer>();
			renderer.material = enemyMat;

			//currentWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<CurrentWeapon>();

			if (GameMode.captureTheFlag)
			{
				flag = GameObject.FindGameObjectWithTag("Flag");
			}
			if (!GameMode.captureTheFlag)
			{
				flag = null;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		/* v Change this to work with the enemy v */

		//if (currentWeapon.currentWeapon == currentWeapon.secondaryWeapon && currentWeapon.flag)
		//{
		//    if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 3)
		//    {
		//        currentWeapon.currentWeapon = currentWeapon.mainWeapon;
		//        currentWeapon.changeWeapon = true;
		//        currentWeapon.flag = false;
		//        flag.SetActive(true);
		//    }
		//}
	}
}
