#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public EnemyArmRotations eArmRot;
	public GameObject hands;
	public GameObject tPose;
	public GameObject weapon;
	public float maxHealth = 100;
	public float curHealth = 100;
	public bool ragdoll;
	public bool weaponSpawned = false;

	private Collider[] colliders;
	private Rigidbody[] rigid;

	// Start is called before the first frame update
	void Start()
	{
		curHealth = maxHealth;
		colliders = GetComponentsInChildren<Collider>();
		rigid = GetComponentsInChildren<Rigidbody>();
		eArmRot = GetComponent<EnemyArmRotations>();
		RagDollOff();
	}

	// Update is called once per frame
	void Update()
	{
		if (curHealth <= 0f)
		{
			ragdoll = true;
			if (ragdoll)
			{
				RagDollOn();
			}
		}
	}

	void RagDollOn()
	{
		weapon.transform.parent = null;
		weapon.GetComponent<Rigidbody>().isKinematic = false;
		if (weaponSpawned)
		{
			weaponSpawned = false;
					
		}
		
		eArmRot.enabled = false;
		hands.SetActive(false);
		tPose.SetActive(true);
		foreach (Rigidbody rb in rigid)
		{
			rb.isKinematic = false;
		}
	}

	void RagDollOff()
	{
		eArmRot.enabled = true;
		hands.SetActive(true);
		tPose.SetActive(false);
		foreach (Rigidbody rb in rigid)
		{
			rb.isKinematic = true;
		}
	}
}
// This code is written by Peter Thompson
#endregion