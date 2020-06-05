using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
	private Transform player;
	private NavMeshAgent agent;
	private Animator anim;
	private Rigidbody rigid;
	private Transform head;
	private Enemy enemy;
	private CurrentCar currentCar;

	public GameObject bodyLook;
	public float lerpSpeed;
	public float stoppingDistance = 5f;
	public float shootingRangeRate = 3f;

	public bool searchMode;
	public bool followPlayerMode;
	public bool readyToShootMode;

	// Start is called before the first frame update
	void Start()
	{
		rigid = GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		head = GetComponentInChildren<Transform>(GameObject.Find("LookDirection").transform);
		enemy = GetComponent<Enemy>();
		currentCar = GameObject.FindGameObjectWithTag("Manager").GetComponent<CurrentCar>();
	}

	// Update is called once per frame
	void Update()
	{
		AIMovement();

		AIShoot();
	}

	void AIMovement()
	{
		if (!Car.inCar)
		{
			float distance = Vector3.Distance(transform.position, player.position);
			rigid.rotation = transform.rotation;

			if (enemy.curHealth > 0)
			{
				if (distance > stoppingDistance)
				{
					agent.enabled = true;
					agent.updatePosition = true;
					agent.SetDestination(player.position);
					anim.SetBool("Moving", true);
					anim.SetBool("Aim", false);
				}
				if (distance <= stoppingDistance)
				{
					Quaternion originalRot = transform.rotation;
					bodyLook.transform.LookAt(player);
					Quaternion NewRot = transform.rotation;
					transform.rotation = originalRot;
					transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, lerpSpeed * Time.deltaTime);

					this.transform.rotation = new Quaternion(this.transform.rotation.x, bodyLook.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
					agent.updatePosition = false;
					agent.enabled = false;
					anim.SetBool("Moving", false);
					anim.SetBool("Aim", true);
				}
			}
			else
			{
				agent.enabled = false;
			}
		}

		if (Car.inCar)
		{
			float distance = Vector3.Distance(transform.position, currentCar.currentCar.transform.position);
			rigid.rotation = transform.rotation;

			if (enemy.curHealth > 0)
			{
				if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 200)
				{
					if (distance > stoppingDistance)
					{
						agent.enabled = true;
						agent.updatePosition = true;
						agent.SetDestination(currentCar.currentCar.transform.position);
						anim.SetBool("Moving", true);
						anim.SetBool("Aim", false);
					}
					if (distance <= stoppingDistance)
					{
						Quaternion originalRot = transform.rotation;
						bodyLook.transform.LookAt(currentCar.currentCar.transform.position);
						Quaternion NewRot = transform.rotation;
						transform.rotation = originalRot;
						transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, lerpSpeed * Time.deltaTime);

						this.transform.rotation = new Quaternion(this.transform.rotation.x, bodyLook.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
						agent.updatePosition = false;
						agent.enabled = false;
						anim.SetBool("Moving", false);
						anim.SetBool("Aim", true);
					}
				}				
			}
			else
			{
				agent.enabled = false;
			}
		}
	}

	void AIShoot()
	{
		shootingRangeRate -= Time.deltaTime;
		if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 200)
		{

		}
		if (shootingRangeRate <= 0f)
		{
			shootingRangeRate = 3f;
			RaycastHit hit;

			if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, 1000))
			{
				if (hit.transform.gameObject.tag == "Player")
				{

				}
			}
		}
	}
}
