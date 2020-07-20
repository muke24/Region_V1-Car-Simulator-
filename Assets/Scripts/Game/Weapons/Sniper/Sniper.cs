#region This code is written by Peter Thompson
using UnityEngine;
using UnityEngine.UI;

public class Sniper : MonoBehaviour
{
	public static GameObject player;
	public static GameObject thisWeapon;
	private Image[] reticle;
	private Image[] hitMarkerHit;
	private Image[] hitMarkerDead;
	private bool hitEnemy = false;
	private bool deadEnemy = false;
	private float hitMarkerTimer = 0.2f;
	private float deadMarkerTimer = 0.2f;
	public float maxRays = 10f;

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

	//private int ragdollForce = 2;

	[SerializeField]
	private GameObject gunshotDecal = null;

	public static int maxAmmo = 5;
	public static int ammoCount = 5;
	#endregion

	#region Deal Damage
	public Enemy enemy;
	public Collisions collisions;

	[Range(0.1f, 200f)]
	public float damage = 100f;

	[SerializeField] // Makes Unity show the private field in inspector
	[Range(0.1f, 5)]
	private float headShotMultiplier = 2f;
	[SerializeField] // Makes Unity show the private field in inspector
	[Range(0.1f, 5)]
	private float bodyShotMultiplier = 0.99f;
	[SerializeField] // Makes Unity show the private field in inspector
	[Range(0.1f, 5)]
	private float legShotMultiplier = 0.75f;
	[SerializeField] // Makes Unity show the private field in inspector
	[Range(0.1f, 5)]
	private float footShotMultiplier = 0.5f;
	#endregion

	#region Impact
	[SerializeField] // Makes Unity show the private field in inspector
	private ParticleSystem muzzelFlash = null;
	[SerializeField] // Makes Unity show the private field in inspector
	private GameObject impactEffect = null;
	[SerializeField] // Makes Unity show the private field in inspector
	private GameObject waterImpactEffect = null;
	#endregion

	private void Awake()
	{
		// Sets the players gameobject to the player in the scene at the start
		player = transform.root.gameObject;
		// Sets the pause script to the pause script in the scene at the start
		pause = Pause.pause;

		reticle = new Image[GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Canvas").Find("FPSCanv").Find("Reticle").GetComponentsInChildren<Image>().Length];
		hitMarkerHit = new Image[GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Canvas").Find("FPSCanv").Find("HitMarkerHit").GetComponentsInChildren<Image>().Length];
		hitMarkerDead = new Image[GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Canvas").Find("FPSCanv").Find("HitMarkerDead").GetComponentsInChildren<Image>().Length];

		for (int i = 0; i < reticle.Length; i++)
		{
			reticle[i] = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Canvas").Find("FPSCanv").Find("Reticle").GetComponentsInChildren<Image>()[i];
		}
		for (int i = 0; i < hitMarkerHit.Length; i++)
		{
			hitMarkerHit[i] = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Canvas").Find("FPSCanv").Find("HitMarkerHit").GetComponentsInChildren<Image>()[i];
		}
		for (int i = 0; i < hitMarkerDead.Length; i++)
		{
			hitMarkerDead[i] = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Canvas").Find("FPSCanv").Find("HitMarkerDead").GetComponentsInChildren<Image>()[i];
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (hitEnemy)
		{
			HitMarkerHit();
		}

		if (deadEnemy)
		{
			HitMarkerDead();
		}

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

		SetReticleTransparency();
	}

	/// <summary>
	/// Checks whether the player can shoot the sniper
	/// </summary>
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

	/// <summary>
	/// Fires the sniper
	/// </summary>
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

		int raycastCount = 0;
		float wallbangMultiplier = 1f;

		// If aiming
		if (pA.playerAnimation.GetBool("Aim"))
		{
			// Shoot a ray directly forward from the gun camera 
			if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward, out hit, range))
			{
				RaycastShot(hit, raycastCount, wallbangMultiplier);
			}
			return;
		}

		if (!pA.playerAnimation.GetBool("Aim"))
		{
			Vector2 RandomShot = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));

			if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward + new Vector3(RandomShot.x, 0, RandomShot.y), out hit, range))
			{
				RaycastShot(hit, raycastCount, wallbangMultiplier);
			}
			return;
		}
	}

	/// <summary>
	/// Shoots ray
	/// </summary>
	/// <param name="hit">Ray data</param>
	/// <param name="raycastCount">Total amount of rays that has been casted</param>
	/// <param name="wallbangMultiplier">Damage multiplier of the bullet</param>
	/// <param name="distance">Total distance travelled</param>
	void RaycastShot(RaycastHit hit, int raycastCount, float wallbangMultiplier)
	{
		raycastCount++;
		float distance = Vector3.Distance(gunCam.transform.position, hit.point);

		if (raycastCount < maxRays)
		{
			if (!hit.transform.gameObject.CompareTag("Player") || hit.transform.name == "Environment")
			{
				if (!hit.transform.gameObject.CompareTag("Water"))
				{
					if (!hit.transform.root.gameObject.CompareTag("Enemy"))
					{
						if (!hit.transform.root.gameObject.CompareTag("TeamPlayer"))
						{
							var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
							GameObject gunShot = Instantiate(gunshotDecal, hit.point, hitRotation);
							gunShot.transform.SetParent(hit.transform);
							GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
							Destroy(impactGO, 2f);
							Destroy(gunShot, 20f);
						}
					}
				}
			}

			// Tells us the collider that was hit in the debug
			Debug.Log("Gunshot hit " + hit.collider.name);

			if (hit.transform.GetComponent<WallBang>() != null && wallbangMultiplier > 0.5f || hit.transform.CompareTag("Wall-Bangable") && wallbangMultiplier > 0.5f)
			{
				Wallbang(hit, raycastCount, wallbangMultiplier, distance);
			}

			if (hit.transform.gameObject.CompareTag("Water"))
			{
				RaycastHit hit2;
				//var heading = hit.point - gunCam.transform.forward;
				//var hitdistance = heading.magnitude;
				//var direction = heading / hitdistance;

				if (Physics.Raycast(gunCam.transform.position, hit.point, out hit2, range))
				{
					GameObject impactGO = Instantiate(waterImpactEffect, hit2.point, Quaternion.LookRotation(hit.normal));
					Destroy(impactGO, 2f);
					RaycastShot(hit2, raycastCount, wallbangMultiplier);
				}

				return;
			}

			if (hit.transform.gameObject.CompareTag("Player"))
			{
				if (Physics.Raycast(hit.point, gunCam.transform.forward, out hit, range - distance))
				{
					Debug.Log("Raycast has hit player, shooting another ray from that hit point.");

					RaycastShot(hit, raycastCount, wallbangMultiplier);
				}
				return;
			}

			#region Enemy

			if (hit.transform.root.gameObject.CompareTag("Enemy") || hit.transform.gameObject.CompareTag("Enemy"))
			{
				enemy = hit.transform.root.gameObject.GetComponent<Enemy>();
				collisions = hit.transform.root.gameObject.GetComponent<Collisions>();

				if (enemy.curHealth > 0)
				{
					if (enemy.gameObject.activeSelf)
					{
						if (hit.collider == collisions.head)
						{
							HeadShotHit(wallbangMultiplier);
						}
						if (hit.collider == collisions.body)
						{
							BodyShotHit(wallbangMultiplier);
						}
						if (hit.collider == collisions.leftUpperArm)
						{
							BodyShotHit(wallbangMultiplier);
						}
						if (hit.collider == collisions.rightUpperArm)
						{
							BodyShotHit(wallbangMultiplier);
						}
						if (hit.collider == collisions.leftLowerArm)
						{
							BodyShotHit(wallbangMultiplier);
						}
						if (hit.collider == collisions.rightLowerArm)
						{
							BodyShotHit(wallbangMultiplier);
						}
						if (hit.collider == collisions.leftUpperLeg)
						{
							LegShotHit(wallbangMultiplier);
						}
						if (hit.collider == collisions.rightUpperLeg)
						{
							LegShotHit(wallbangMultiplier);
						}
						if (hit.collider == collisions.leftLowerLeg)
						{
							FootShotHit(wallbangMultiplier);
						}
						if (hit.collider == collisions.rightLowerLeg)
						{
							FootShotHit(wallbangMultiplier);
						}
						if (enemy.curHealth > 0)
						{
							hitEnemy = true;

						}
						if (enemy.curHealth <= 0 && !enemy.isDead)
						{
							deadEnemy = true;
							enemy.isDead = true;
						}
					}
				}

				if (Physics.Raycast(hit.transform.position, gunCam.transform.forward, out hit, range - distance))
				{
					Debug.Log("Raycast has hit an enemy, shooting another ray from that hit point.");

					if (raycastCount > maxRays)
					{
						return;
					}
					else
					{
						RaycastShot(hit, raycastCount, wallbangMultiplier);
					}
				}
				return;
			}

			else
			{
				enemy = null;
				collisions = null;
			}

			#endregion

			if (hit.transform.root.gameObject.CompareTag("TeamPlayer") || hit.transform.gameObject.CompareTag("TeamPlayer"))
			{
				if (Physics.Raycast(hit.transform.position, gunCam.transform.forward, out hit, range - distance))
				{
					Debug.Log("Raycast has hit a team player, shooting another ray from that hit point.");

					RaycastShot(hit, raycastCount, wallbangMultiplier);
				}
				return;
			}
		}
	}

	/// <summary>
	/// Wallbang sniper shot
	/// </summary>
	/// <param name="hit">Ray data</param>
	/// <param name="raycastCount">Total amount of rays that has been casted</param>
	/// <param name="wallbangMultiplier">Damage multiplier of the bullet</param>
	/// <param name="distance">Total distance travelled</param>
	void Wallbang(RaycastHit hit, int raycastCount, float wallbangMultiplier, float distance)
	{
		wallbangMultiplier *= hit.transform.GetComponent<WallBang>().damageCutOffMultiplier;
		print("Wallbang! Shot damage reduced to " + (damage * wallbangMultiplier).ToString());
		foreach (Collider col in hit.transform.GetComponentsInChildren<Collider>())
		{
			col.enabled = false;
		}
		RaycastHit hit2;
		if (Physics.Raycast(hit.point, gunCam.transform.forward, out hit2, range - distance))
		{
			RaycastShot(hit2, raycastCount, wallbangMultiplier);
			foreach (Collider col in hit.transform.GetComponentsInChildren<Collider>())
			{
				col.enabled = true;
			}
		}
	}

	/// <summary>
	/// Shooting animation
	/// </summary>
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

	/// <summary>
	/// Shoot if aiming animation
	/// </summary>
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

	/// <summary>
	/// Reload sniper animation
	/// </summary>
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

	/// <summary>
	/// Reticle transparency on aim
	/// </summary>
	void SetReticleTransparency()
	{
		// Local variables
		Color reticleColour = reticle[0].color;
		float reticleAlpha = reticleColour.a;

		// If aiming reduce the alpha colour of the reticle
		if (pA.playerAnimation.GetBool("Aim") && !pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperReload") && !pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("TakeOutSniper") && !pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("TakeAwayWeapon"))
		{
			if (reticleAlpha >= 0 && reticleAlpha <= 1)
			{
				reticleColour = new Color(reticleColour.r, reticleColour.g, reticleColour.b, reticleAlpha -= Time.deltaTime * 4);
			}
			if (reticleAlpha <= 0)
			{
				reticleAlpha = 0;
				reticleColour = new Color(reticleColour.r, reticleColour.g, reticleColour.b, reticleAlpha);
			}
			if (reticleAlpha >= 1)
			{
				reticleAlpha = 1;
				reticleColour = new Color(reticleColour.r, reticleColour.g, reticleColour.b, reticleAlpha);
			}
		}

		// If not aiming increase the alpha colour of the reticle
		if (!pA.playerAnimation.GetBool("Aim"))
		{
			if (reticleAlpha >= 0 && reticleAlpha <= 1)
			{
				reticleColour = new Color(reticleColour.r, reticleColour.g, reticleColour.b, reticleAlpha += Time.deltaTime * 4);
			}
			if (reticleAlpha <= 0)
			{
				reticleAlpha = 0;
				reticleColour = new Color(reticleColour.r, reticleColour.g, reticleColour.b, reticleAlpha);
			}
			if (reticleAlpha >= 1)
			{
				reticleAlpha = 1;
				reticleColour = new Color(reticleColour.r, reticleColour.g, reticleColour.b, reticleAlpha);
			}
		}

		// If changing weapon set reticle alpha to 1
		if (pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("TakeAwayWeapon"))
		{
			reticleAlpha = 1;
			reticleColour = new Color(reticleColour.r, reticleColour.g, reticleColour.b, reticleAlpha);
		}

		// Apply the reticle alpha
		foreach (Image item in reticle)
		{
			item.color = reticleColour;
			item.GetComponent<Outline>().effectColor = new Color(0, 0, 0, reticleColour.a);
		}
	}

	/// <summary>
	/// Show yellow hitmarker if bullet hit enemy
	/// </summary>
	void HitMarkerHit()
	{
		Color hitMarkerColour = hitMarkerHit[0].color;

		if (hitMarkerTimer >= 0)
		{
			hitMarkerTimer -= Time.deltaTime;

			if (hitMarkerHit[0].color.a != 1)
			{
				foreach (Image hitmarker in hitMarkerHit)
				{
					hitmarker.color = new Color(hitMarkerColour.r, hitMarkerColour.g, hitMarkerColour.b, 1);
					hitmarker.GetComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
				}
			}
		}

		if (hitMarkerTimer <= 0)
		{
			hitMarkerTimer = 0.2f;

			foreach (Image hitmarker in hitMarkerHit)
			{
				hitmarker.color = new Color(hitMarkerColour.r, hitMarkerColour.g, hitMarkerColour.b, 0);
				hitmarker.GetComponent<Outline>().effectColor = new Color(0, 0, 0, 0);
			}

			hitEnemy = false;
		}
	}

	/// <summary>
	/// Show red hitmarker if bullet killed enemy
	/// </summary>
	void HitMarkerDead()
	{
		Color hitMarkerColour = hitMarkerDead[0].color;

		if (deadMarkerTimer >= 0)
		{
			deadMarkerTimer -= Time.deltaTime;

			if (hitMarkerDead[0].color.a != 1)
			{
				foreach (Image hitmarker in hitMarkerDead)
				{
					hitmarker.color = new Color(hitMarkerColour.r, hitMarkerColour.g, hitMarkerColour.b, 1);
					hitmarker.GetComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
				}
			}
		}

		if (deadMarkerTimer <= 0)
		{
			deadMarkerTimer = 0.2f;

			foreach (Image hitmarker in hitMarkerDead)
			{
				hitmarker.color = new Color(hitMarkerColour.r, hitMarkerColour.g, hitMarkerColour.b, 0);
				hitmarker.GetComponent<Outline>().effectColor = new Color(0, 0, 0, 0);
			}

			deadEnemy = false;
		}
	}

	#region Hitboxes

	/// <summary>
	/// Sniper shot has hit enemy in head
	/// </summary>
	/// <param name="wallbangMultiplier">Damage of the bullet if there was a wallbang</param>
	void HeadShotHit(float wallbangMultiplier)
	{
		enemy.curHealth -= damage * wallbangMultiplier * headShotMultiplier;
	}

	/// <summary>
	/// Sniper shot has hit enemy in the body
	/// </summary>
	/// <param name="wallbangMultiplier">Damage of the bullet if there was a wallbang</param>
	void BodyShotHit(float wallbangMultiplier)
	{
		enemy.curHealth -= damage * wallbangMultiplier * bodyShotMultiplier;
	}

	/// <summary>
	/// Sniper shot has hit enemy in the leg
	/// </summary>
	/// <param name="wallbangMultiplier">Damage of the bullet if there was a wallbang</param>
	void LegShotHit(float wallbangMultiplier)
	{
		enemy.curHealth -= damage * wallbangMultiplier * legShotMultiplier;
	}

	/// <summary>
	/// Sniper shot has hit enemy in the foot
	/// </summary>
	/// <param name="wallbangMultiplier">Damage of the bullet if there was a wallbang</param>
	void FootShotHit(float wallbangMultiplier)
	{
		enemy.curHealth -= damage * wallbangMultiplier * footShotMultiplier;
	}
	#endregion
}
// This code is written by Peter Thompson
#endregion