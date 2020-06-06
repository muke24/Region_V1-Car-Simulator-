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
	private Vector3 playersSeenPosition;

	public ParticleSystem shootAnim;
	public Transform weapon;
	public GameObject bodyLook;
	public float lerpSpeed;
	public float stoppingDistance = 5f;
	public float castRayRate = 3f;
	public float viewDistance = 150f;
	public float damage = 49f;
	public float enemyAccuracy = 1f;

	public bool searchMode = true;
	public bool lookAtPlayerMode = false;
	public bool followPlayerMode = false;
	//public bool readyToShootMode = false;

	public bool getPlayerPos = false;
	public bool findPlayer = false;

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
		castRayRate -= Time.deltaTime;

		if (searchMode)
		{
			AISearch();
		}
		if (lookAtPlayerMode)
		{
			LookAtPlayer();
		}
		if (followPlayerMode)
		{
			FollowPlayerMode();
		}

		//AIMovement();
	}


	void AIMovement()
	{
		if (!Car.inCar)
		{
			float stopDistance = Vector3.Distance(transform.position, player.position);
			rigid.rotation = transform.rotation;

			if (enemy.curHealth > 0)
			{
				if (stopDistance > stoppingDistance)
				{

					agent.enabled = true;
					agent.updatePosition = true;
					agent.SetDestination(player.position);
					anim.SetBool("Moving", true);
					anim.SetBool("Aim", false);

				}
				if (stopDistance <= stoppingDistance)
				{

					Quaternion originalRot = transform.rotation;
					bodyLook.transform.LookAt(player);
					Quaternion NewRot = transform.rotation;
					transform.rotation = originalRot;
					transform.rotation = Quaternion.Lerp(originalRot, NewRot, lerpSpeed * Time.deltaTime);

					transform.rotation = new Quaternion(transform.rotation.x, bodyLook.transform.rotation.y, transform.rotation.z, transform.rotation.w);
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
						bodyLook.transform.LookAt(player);
						Quaternion NewRot = transform.rotation;
						transform.rotation = originalRot;
						transform.rotation = Quaternion.Lerp(originalRot, NewRot, lerpSpeed * Time.deltaTime);

						transform.rotation = new Quaternion(transform.rotation.x, bodyLook.transform.rotation.y, transform.rotation.z, transform.rotation.w);
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

	void AISearch()
	{
		if (enemy.curHealth > 0)
		{
			RaycastHit hit;

			if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= viewDistance)
			{
				if (castRayRate <= 0f)
				{
					castRayRate = 3f;
					if (Physics.Raycast(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position - transform.position, out hit, viewDistance))
					{
						if (hit.transform.gameObject.tag == "Player")
						{
							searchMode = false;
							lookAtPlayerMode = true;
							getPlayerPos = true;
							castRayRate = 3f;
							Debug.Log("Raycast hit " + hit.transform.name + ". Player has been seen. " + gameObject.name + " is now looking at player's last seen position");
						}
						else
						{
							castRayRate = 3f;
							Debug.Log("Didnt hit player. Raycast hit " + hit.transform.name);
						}
					}
				}
			}
		}
	}

	void LookAtPlayer()
	{


		if (enemy.curHealth > 0)
		{
			agent.updatePosition = false;
			agent.enabled = false;
			anim.SetBool("Moving", false);
			anim.SetBool("Aim", false);

			if (getPlayerPos)
			{
				playersSeenPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
				getPlayerPos = false;
				findPlayer = true;
			}

			if (findPlayer)
			{
				Debug.Log("Rotating towards player's last seen position");
				Vector3 direction = playersSeenPosition - transform.position;
				direction.y = 0;

				Quaternion toRotation = Quaternion.LookRotation(direction);
				transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * (lerpSpeed / 360));

				if (transform.rotation == toRotation)
				{
					RaycastHit hit;

					if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= viewDistance)
					{
						if (Physics.Raycast(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position - transform.position, out hit, viewDistance))
						{
							if (hit.transform.gameObject.tag == "Player")
							{
								searchMode = false;
								lookAtPlayerMode = false;
								getPlayerPos = false;
								findPlayer = false;
								followPlayerMode = true;
								Debug.Log("Raycast hit " + hit.transform.name + ". Enemy entering followPlayerMode!");
							}
							else
							{
								castRayRate = 3f;

								searchMode = true;
								lookAtPlayerMode = false;
								getPlayerPos = false;
								findPlayer = false;
								followPlayerMode = false;

								Debug.Log("Didnt hit player. Raycast hit " + hit.transform.name + ". Enemy going back into search mode");
							}
						}
					}
				}
			}
		}
	}

	void FollowPlayerMode()
	{
		if (enemy.curHealth > 0)
		{
			if (castRayRate <= 0f)
			{
				castRayRate = 3f;

				#region Cast Ray
				RaycastHit hit;

				if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= viewDistance)
				{
					if (Physics.Raycast(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position - transform.position, out hit, viewDistance))
					{
						if (hit.transform.gameObject.tag == "Player")
						{
							castRayRate = 3f;

							searchMode = false;
							lookAtPlayerMode = false;
							getPlayerPos = false;
							findPlayer = false;
							followPlayerMode = true;
							Debug.Log("Raycast hit " + hit.transform.name + ". Enemy is still in followPlayerMode and chasing the player!");
							if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 50)
							{
								Shoot();
							}

						}
						else
						{
							castRayRate = 3f;

							searchMode = false;
							lookAtPlayerMode = true;
							getPlayerPos = true;
							findPlayer = false;
							followPlayerMode = false;

							Debug.Log("Didnt hit player. Raycast hit " + hit.transform.name + ". Enemy going back into lookAtPlayer mode");
						}
					}
				}
				#endregion
			}
		}

		float stopDistance = Vector3.Distance(transform.position, player.position);
		rigid.rotation = transform.rotation;
		if (!lookAtPlayerMode)
		{
			if (enemy.curHealth > 0)
			{
				if (stopDistance > stoppingDistance)
				{

					agent.enabled = true;
					agent.updatePosition = true;
					agent.SetDestination(player.position);
					anim.SetBool("Moving", true);
					anim.SetBool("Aim", false);

				}
				if (stopDistance <= stoppingDistance)
				{
					Quaternion originalRot = transform.rotation;
					bodyLook.transform.LookAt(player);
					Quaternion NewRot = transform.rotation;
					transform.rotation = originalRot;
					transform.rotation = Quaternion.Lerp(originalRot, NewRot, lerpSpeed * Time.deltaTime);

					transform.rotation = new Quaternion(transform.rotation.x, bodyLook.transform.rotation.y, transform.rotation.z, transform.rotation.w);
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
	}

	void Shoot()
	{
		shootAnim.Play();

		#region Cast Ray
		RaycastHit hit;

		Vector2 RandomShot = new Vector2(Random.Range(-enemyAccuracy, enemyAccuracy), Random.Range(-enemyAccuracy, enemyAccuracy));

		if (Physics.Raycast(weapon.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position - transform.position + new Vector3(RandomShot.x, 0, RandomShot.y), out hit, 1000))
		{

			if (hit.transform.gameObject.tag == "Player")
			{
				castRayRate = 3f;

				player.GetComponent<Player>().curHealth -= damage;
				player.GetComponent<Player>().wasShot = true;
				player.GetComponent<Player>().healthIsRegening = false;

				Debug.Log("Player has been shot by " + gameObject.name + " which dealt " + damage + "damage!");
			}

			else
			{
				castRayRate = 3f;

				Debug.Log(gameObject.name + " has shot and missed the player");
			}
		}

		#endregion
	}
}
