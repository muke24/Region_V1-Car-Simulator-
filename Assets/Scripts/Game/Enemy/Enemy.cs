#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	public bool isDead = false;
	public EnemyArmRotations eArmRot;
	public GameObject player;
	public Animator anim;
	public GameObject hands;
	public GameObject weapon;
	public float maxHealth = 100;
	public float curHealth = 100;
	public bool ragdoll;
	public bool force;
	public Slider healthSlider;
	public Text healthText;

	public int spawnId;
	private bool timerOn = false;
	private float timer = 5f;

	private GameObject tPose;
	private Collider[] colliders;
	private Rigidbody[] rigid;
	private EnemyAI enemyAI;
	private NavMeshAgent agent;
	private SpawnPoints spawnPoints = null;

	// Start is called before the first frame update
	void Start()
	{
		colliders = GetComponentsInChildren<Collider>();
		rigid = GetComponentsInChildren<Rigidbody>();
		eArmRot = GetComponent<EnemyArmRotations>();
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
		tPose = transform.Find("HeadPivot").transform.Find("T-Pose").gameObject;

		curHealth = maxHealth;
		ragdoll = false;
		tPose.SetActive(false);
		enemyAI = GetComponent<EnemyAI>();
		agent = GetComponent<NavMeshAgent>();
		spawnPoints = GameObject.FindGameObjectWithTag("SpawnPoints").GetComponent<SpawnPoints>();

		RagDollOff();
	}

	// Update is called once per frame
	void Update()
	{
		if (curHealth <= 0)
		{
			RagDollOn();

			hands.SetActive(false);
			tPose.SetActive(true);
		}
		if (curHealth > 0)
		{
			RagDollOff();
		}

		healthSlider.value = curHealth;
		healthText.text = (Mathf.RoundToInt(curHealth)).ToString() + " / " + (Mathf.RoundToInt(maxHealth)).ToString();

		if (timerOn)
		{
			if (timer >= 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				timerOn = false;
				timer = 5f;
				Respawn();
			}
		}
	}

	void RagDollOn()
	{
		healthSlider.gameObject.SetActive(false);
		healthText.gameObject.SetActive(false);

		rigid = GetComponentsInChildren<Rigidbody>();

		enemyAI.enabled = false;
		agent.enabled = false;

		anim.enabled = false;

		weapon.transform.parent = null;
		weapon.GetComponent<Rigidbody>().isKinematic = false;

		GetComponent<Rigidbody>().rotation = transform.rotation;

		eArmRot.enabled = false;

		force = true;
		if (force)
		{
			foreach (Rigidbody rb in rigid)
			{
				rb.isKinematic = false;
			}

			force = false;
		}

		timerOn = true;

		Destroy(gameObject, 10);
	}

	void RagDollOff()
	{
		eArmRot.enabled = true;

		foreach (Rigidbody rb in rigid)
		{
			rb.isKinematic = true;
		}
	}

	void Respawn()
	{
		spawnPoints.respawnEnemy[spawnId] = true;
		spawnPoints.RespawnEnemyAfterDeath();
	}
}
// This code is written by Peter Thompson
#endregion