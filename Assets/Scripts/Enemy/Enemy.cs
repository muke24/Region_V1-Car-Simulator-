#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public EnemyArmRotations eArmRot;
	public GameObject player;
	public Sniper sniper;
	public Animator anim;
	public GameObject hands;
	public GameObject weapon;
	public float maxHealth = 100;
	public float curHealth = 100;
	public bool ragdoll;
	public bool force;

	private GameObject tPose;
	private Collider[] colliders;
	private Rigidbody[] rigid;

	private void Awake()
	{
		colliders = GetComponentsInChildren<Collider>();
		rigid = GetComponentsInChildren<Rigidbody>();
		eArmRot = GetComponent<EnemyArmRotations>();
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
		sniper = player.GetComponentInChildren<Sniper>();
		tPose = transform.Find("HeadPivot").transform.Find("T-Pose").gameObject;
	}

	// Start is called before the first frame update
	void Start()
	{
		RagDollOff();
		curHealth = maxHealth;
		ragdoll = false;
		tPose.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (curHealth <= 0)
		{
			RagDollOn();

			hands.SetActive(false);
			tPose.SetActive(true);

			//ragdoll = true;
			//if (ragdoll)
			//{
			//	RagDollOn();
			//}
		}
		if (curHealth > 0)
		{
			RagDollOff();

			//hands.SetActive(true);
			//tPose.SetActive(false);
		}
		//if (!ragdoll)
		//{
		//	RagDollOff();
		//}
	}

	void RagDollOn()
	{
		rigid = GetComponentsInChildren<Rigidbody>();

		anim.enabled = false;

		weapon.transform.parent = null;
		weapon.GetComponent<Rigidbody>().isKinematic = false;

		GetComponent<Rigidbody>().rotation = transform.rotation;

		eArmRot.enabled = false;
		//hands.SetActive(false);
		//tPose.SetActive(true);
		force = true;
		if (force)
		{
			foreach (Rigidbody rb in rigid)
			{
				rb.isKinematic = false;
				//GetComponent<Rigidbody>().AddForce(player.transform.forward * sniper.ragdollForce, ForceMode.Force);
			}

			force = false;
		}

		Destroy(gameObject, 10);
	}

	void RagDollOff()
	{
		eArmRot.enabled = true;
		//hands.SetActive(true);
		//tPose.SetActive(false);
		foreach (Rigidbody rb in rigid)
		{
			rb.isKinematic = true;
		}
	}
}
// This code is written by Peter Thompson
#endregion