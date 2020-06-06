using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
	private Player player;
	private GameObject playerGO;

	public GameObject weapon;
	public GameObject oldWeapon;
	public Camera playerCam;
	public GameObject playerRagdoll;
	public bool isDead = false;
	public bool createPlayerRagdoll = false;
	public bool destroyPlayer = false;

	// Start is called before the first frame update
	void Start()
	{
		player = GetComponent<Player>();
		playerGO = gameObject;
	}

	// Update is called once per frame
	void Update()
	{
		if (player.curHealth == 0)
		{
			isDead = true;
			if (isDead)
			{
				createPlayerRagdoll = true;
				if (createPlayerRagdoll)
				{
					Instantiate<GameObject>(playerRagdoll, playerGO.transform.position, playerGO.transform.rotation);
					Instantiate<GameObject>(weapon, oldWeapon.transform.position, oldWeapon.transform.rotation);

					//playerRagdoll.transform.position = playerGO.transform.position;
					//playerRagdoll.transform.rotation = playerGO.transform.rotation;

					//weapon.transform.position = oldWeapon.transform.position;
					//weapon.transform.rotation = oldWeapon.transform.rotation;

					createPlayerRagdoll = false;
					destroyPlayer = true;
				}
			}

			if (destroyPlayer)
			{
				//Destroy(playerGO);
				playerGO.SetActive(false);
			}
		}
	}
}
