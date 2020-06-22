#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
	public static GameObject player;
	public static GameObject thisWeapon;

	#region Sniper
	[SerializeField] // Makes Unity show the private field in inspector
	private GameObject pause = null;
	[SerializeField] // Makes Unity show the private field in inspector
	private Camera gunCam = null;
	[SerializeField] // Makes Unity show the private field in inspector
	private PlayerAnimations pA = null;

	[SerializeField] // Makes Unity show the private field in inspector
	private float range = 1000f;
	[SerializeField] // Makes Unity show the private field in inspector
	private float boltTimer = 1f;
	[SerializeField] // Makes Unity show the private field in inspector
	private float shootTimer = 1.4f;

	[SerializeField] // Makes Unity show the private field in inspector
	private bool shootBool;
	[SerializeField] // Makes Unity show the private field in inspector
	private bool reload;

	public int ragdollForce = 2;

	public GameObject gunshotDecal;

	public static int maxAmmo = 5;
	public static int ammoCount = 5;

	public int imaxAmmo = 5;   // non-static int to show in inspector
	public int iammoCount = 5; // non-static int to show in inspector
	#endregion

	#region Deal Damage
	public Enemy enemy;
	public Collisions collisions;

	public float damage = 100f;

	[SerializeField] // Makes Unity show the private field in inspector
	private float headShotMultiplier = 2f;
	[SerializeField] // Makes Unity show the private field in inspector
	private float bodyShotMultiplier = 0.99f;
	[SerializeField] // Makes Unity show the private field in inspector
	private float legShotMultiplier = 0.75f;
	[SerializeField] // Makes Unity show the private field in inspector
	private float footShotMultiplier = 0.5f;
	#endregion

	#region Impact
	[SerializeField] // Makes Unity show the private field in inspector
	private ParticleSystem muzzelFlash = null;
	[SerializeField] // Makes Unity show the private field in inspector
	private GameObject impactEffect = null;
	#endregion

	private void Start()
	{
		// Sets the players gameobject to the player in the scene at the start
		player = transform.root.gameObject;
		// Sets the pause script to the pause script in the scene at the start
		pause = Pause.pause;
	}

	// Update is called once per frame
	void Update()
	{
		// Sets the inspector max ammo int to the static int to see what the max ammo is in the inspector
		imaxAmmo = maxAmmo;
		// Sets the inspector ammo count int to the static int to see what the ammo count is in the inspector
		iammoCount = ammoCount;

		// If is not paused
		if (!pause.activeInHierarchy)
		{
			// Checks if sniper is able to shoot
			CheckIfCanShoot();
		}

		// Always check reload void
		Reload();
		// Always check ShootAnimation void 
		ShootAnimation();
		// Always check ShootWhileZoomedAnimation void 
		ShootWhileZoomedAnimation();
	}

	void CheckIfCanShoot()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			/* Checks all the animation states to see if the sniper isnt in the shoot or bolt animation already,
			* or isnt transitioning to the shoot or bolt animation, isnt in the reload animation and has an ammo count greater than 0 */
			if (!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperShoot"))
			{
				if (!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperBoltAction"))
				{
					if (!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomShoot"))
					{
						if (!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomBoltAction"))
						{
							if (!pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperShoot"))
							{
								if (!pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperBoltAction"))
								{
									if (!pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperZoomShoot"))
									{
										if (!pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperZoomBoltAction"))
										{
											if (!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
											{
												if (!pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperReload"))
												{
													// If shoot bool is false then reload time pass passed
													if (!shootBool)
													{
														if (ammoCount > 0)
														{
															// If passed all the if statements the sniper will shoot
															Shoot();
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// When shootBool returns true then the sniper cannot shoot, and shootTimer acts as a reload time
		if (shootBool)
		{
			// If shootTimer is greater than 0 then it will decrease its value by time
			if (shootTimer > 0)
			{
				shootTimer -= Time.deltaTime;
			}
			// If shootTimer is less than or equal to 0 then it will set the shootBool to false
			if (shootTimer <= 0)
			{
				shootBool = false;
				shootTimer = 1.4f;
			}
		}
	}

	void Shoot()
	{
		// When void Shoot is called set shootBool to true. ShootBool then sets a timer (shootTimer) for when the sniper can shoot again
		shootBool = true;

		// Takes away ammo when sniper shoots;
		ammoCount = ammoCount - 1;

		// Sets the Player Animaton bools to let the animations change state to shoot and then bolt
		pA.playerAnimation.SetBool("Shoot", true);
		pA.playerAnimation.SetBool("Bolt", true);

		// Play the muzzle flash particle system
		muzzelFlash.Play();

		// Raycast hit info variable
		RaycastHit hit;

		// If aiming
		if (pA.playerAnimation.GetBool("Aim"))
		{
			if (GameMode.multiplayer)
			{
				ClientSend.PlayerShoot(gunCam.transform.forward);
			}			

			// Shoot a ray directly forward from the gun camera 
			if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward, out hit, range))
			{
				if (hit.transform.gameObject.tag == "Player")
				{
					if (Physics.Raycast(hit.transform.position, gunCam.transform.forward, out hit, range))
					{
						Debug.Log("Raycast has hit player, shooting another ray from that hit point.");
					}
				}

				// Tells us the collider that was hit in the debug
				Debug.Log("Gunshot hit " + hit.collider.name);

				// If an enemy was hit then get its specific enemy script and collision script. The collision script holds all of the enemy's hitboxes
				if (hit.transform.root.gameObject.tag == "Enemy")
				{
					enemy = hit.transform.root.gameObject.GetComponent<Enemy>();
					collisions = hit.transform.root.gameObject.GetComponent<Collisions>();
				}

				// If an enemy wasn't hit then set the current enemy and collisions script to null
				else
				{
					enemy = null;
					collisions = null;
				}				

				if (hit.transform.gameObject.tag != "Player")
				{
					var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
					GameObject gunShot = Instantiate(gunshotDecal, hit.point, hitRotation);
					gunShot.transform.SetParent(hit.transform);
					GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
					Destroy(impactGO, 2f);
					Destroy(gunShot, 20f);
				}

				#region Hit collider check aimed
				if (hit.transform.root.tag == "Enemy")
				{
					if (enemy.gameObject.activeSelf)
					{
						if (hit.collider == collisions.head)
						{
							HeadShotHit();
						}
						if (hit.collider == collisions.body)
						{
							BodyShotHit();
						}
						if (hit.collider == collisions.leftUpperArm)
						{
							BodyShotHit();
						}
						if (hit.collider == collisions.rightUpperArm)
						{
							BodyShotHit();
						}
						if (hit.collider == collisions.leftLowerArm)
						{
							BodyShotHit();
						}
						if (hit.collider == collisions.rightLowerArm)
						{
							BodyShotHit();
						}
						if (hit.collider == collisions.leftUpperLeg)
						{
							LegShotHit();
						}
						if (hit.collider == collisions.rightUpperLeg)
						{
							LegShotHit();
						}
						if (hit.collider == collisions.leftLowerLeg)
						{
							FootShotHit();
						}
						if (hit.collider == collisions.rightLowerLeg)
						{
							FootShotHit();
						}
					}
				}
				#endregion
			}
		}


		if (!pA.playerAnimation.GetBool("Aim"))
		{
			Vector2 RandomShot = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));

			if (GameMode.multiplayer)
			{
				ClientSend.PlayerShoot(gunCam.transform.forward + new Vector3(RandomShot.x, 0, RandomShot.y));
			}
			
			if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward + new Vector3(RandomShot.x, 0, RandomShot.y), out hit, range))
			{
				if (hit.transform.gameObject.tag == "Player")
				{
					if (Physics.Raycast(hit.transform.position, gunCam.transform.forward, out hit, range))
					{
						Debug.Log("Raycast has hit player, shooting another ray from that hit point.");
					}
				}

				Debug.Log("Gunshot hit " + hit.collider.name);

				if (hit.transform.root.gameObject.tag == "Enemy")
				{
					enemy = hit.transform.root.gameObject.GetComponent<Enemy>();
					collisions = hit.transform.root.gameObject.GetComponent<Collisions>();
				}
				else
				{
					enemy = null;
					collisions = null;
				}
				if (hit.transform.gameObject.tag != "Player")
				{
					var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
					GameObject gunShot = Instantiate(gunshotDecal, hit.point, hitRotation);
					gunShot.transform.SetParent(hit.transform);
					GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
					Destroy(impactGO, 2f);
					Destroy(gunShot, 20f);
				}

				#region Hit collider check
				if (hit.transform.root.tag == "Enemy")
				{
					if (enemy.gameObject.activeSelf)
					{

						if (hit.collider == collisions.head)
						{
							HeadShotHit();
						}
						if (hit.collider == collisions.body)
						{
							BodyShotHit();
						}
						if (hit.collider == collisions.leftUpperArm)
						{
							BodyShotHit();
						}
						if (hit.collider == collisions.rightUpperArm)
						{
							BodyShotHit();
						}
						if (hit.collider == collisions.leftLowerArm)
						{
							BodyShotHit();
						}
						if (hit.collider == collisions.rightLowerArm)
						{
							BodyShotHit();
						}
						if (hit.collider == collisions.leftUpperLeg)
						{
							LegShotHit();
						}
						if (hit.collider == collisions.rightUpperLeg)
						{
							LegShotHit();
						}
						if (hit.collider == collisions.leftLowerLeg)
						{
							FootShotHit();
						}
						if (hit.collider == collisions.rightLowerLeg)
						{
							FootShotHit();
						}
					}
				}
				#endregion
			}
		}
	}

	void ShootAnimation()
	{
		#region Shoot Animation

		// If the next animation is the sniper bolt animation then set the shoot bool to false
		if (pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperBoltAction"))
		{
			pA.playerAnimation.SetBool("Shoot", false);
		}

		// If the next animation is the shoot animation then set the shoot bool to false
		if (pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperShoot"))
		{
			pA.playerAnimation.SetBool("Shoot", true);
		}

		//// If the current animation playing is the bolt animation and the bolt timer is greater than 0, or the bolt while aiming animation is playing and the bolt timer is greater than 0 then
		//if (pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperBoltAction") && boltTimer > 0 || pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomBoltAction") && boltTimer > 0)
		//{
		//	//pA.playerAnimation.SetBool("Bolt", true);
		//}

		// If the current animation playing is the bolt animation and the bolt timer is less than 0 and greater than -0,2, or the bolt while aiming animation is playing and the bolt timer is less than 0 and greater than -0.2 then set the bolt bool to false
		if (pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperBoltAction") && boltTimer < 0 && boltTimer > -0.2f || pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomBoltAction") && boltTimer < 0 && boltTimer > -0.2f)
		{
			pA.playerAnimation.SetBool("Bolt", false);
		}

		// If the bolt bool is true then minus the bolt timer by the time that passes
		if (pA.playerAnimation.GetBool("Bolt"))
		{
			boltTimer -= Time.deltaTime;
		}

		//if (!pA.playerAnimation.GetBool("Bolt"))
		//{
		//	//boltBool = false;
		//}

		// If the bolt timer is less than 0 and greater than -0.2 than set the bolt bool to false and minus the bolt timer by the time that passes
		if (boltTimer < 0f && boltTimer > -0.2f)
		{
			pA.playerAnimation.SetBool("Bolt", false);
			boltTimer -= Time.deltaTime;
		}

		// If the bolt timer is less than -0.2 than set the bolt timer to 1
		if (boltTimer <= -0.2f)
		{
			boltTimer = 1f;
		}
		#endregion
	}

	void ShootWhileZoomedAnimation()
	{
		#region Shoot While Zoomed Animation

		// If the next animation is the sniper bolt animation then set the shoot bool to false
		if (pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperZoomBoltAction"))
		{
			pA.playerAnimation.SetBool("Shoot", false);
		}

		// If the next animation is the shoot animation then set the shoot bool to true
		if (pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperZoomShoot"))
		{
			pA.playerAnimation.SetBool("Shoot", true);
		}

		#endregion
	}

	void Reload()
	{
		// If the sniper ammo count is 0 then set the reload bool to true
		if (ammoCount == 0)
		{
			reload = true;
		}

		// Sets the reload bool in the sniper animation to true which plays the reload animation
		pA.playerAnimation.SetBool("Reload", reload);

		// If sniper reload animation is finished then set the sniper ammo to the maximum ammo count
		if (pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
		{
			if (pA.playerAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				ammoCount = maxAmmo;
			}
		}

		// If game isnt paused and reload button is pressed then reload the sniper
		if (!pause.activeInHierarchy)
		{
			if (Input.GetButtonDown("Reload"))
			{
				if (ammoCount < maxAmmo)
				{
					reload = true;
					pA.playerAnimation.SetBool("Reload", reload);
				}
			}
		}
	}

	#region Hitboxes
	void HeadShotHit()
	{
		enemy.curHealth -= damage * headShotMultiplier;
	}

	void BodyShotHit()
	{
		enemy.curHealth -= damage * bodyShotMultiplier;
	}

	void LegShotHit()
	{
		enemy.curHealth -= damage * legShotMultiplier;
	}

	void FootShotHit()
	{
		enemy.curHealth -= damage * footShotMultiplier;
	}
	#endregion
}
// This code is written by Peter Thompson
#endregion