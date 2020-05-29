using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
	#region Sniper
	[SerializeField]
	private GameObject pause = null;
	[SerializeField]
	private float range = 1000f;
	[SerializeField]
	private float boltTimer = 1f;
	[SerializeField]
	private float shootTimer = 1.3f;
	[SerializeField]
	private Camera gunCam = null;
	[SerializeField]
	private PlayerAnimations pA = null;
	//[SerializeField]
	//private bool boltBool;
	[SerializeField]
	private bool shootBool;
	#endregion

	#region Deal Damage
	public Enemy enemy;
	public Collisions collisions;
	public float damage = 100f;

	[SerializeField]
	private float headShotMultiplier = 2f;
	[SerializeField]
	private float bodyShotMultiplier = 1f;
	[SerializeField]
	private float legShotMultiplier = 0.75f;
	[SerializeField]
	private float footShotMultiplier = 0.5f;
	#endregion

	public GameObject gunshotDecal;

	[SerializeField]
	private ParticleSystem muzzelFlash = null;
	[SerializeField]
	private GameObject impactEffect = null;
	[SerializeField]
	private bool reload;

	public static int maxAmmo = 5;
	public static int ammoCount = 5;

	public int imaxAmmo = 5;        // non-static int
	public int iammoCount = 5;      // non-static int

	private void Start()
	{
		//boltBool = true;
	}

	// Update is called once per frame
	void Update()
	{
		imaxAmmo = maxAmmo;
		iammoCount = ammoCount;

		CheckIfCanShoot();

		Reload();

		#region commented out
		//if (!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperBoltAction"))
		//{
		//	if (!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomShoot"))
		//	{
		//		if (!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomBoltAction"))
		//		{
		//			if (Input.GetButtonDown("Fire1"))
		//			{
		//				Shoot();
		//			}
		//		}
		//	}
		//}

		//if (!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperShoot") ||
		//		!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperBoltAction") ||
		//		!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomShoot") ||
		//		!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomBoltAction"))
		//{
		//	if (!pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperShoot") ||
		//		!pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperBoltAction") ||
		//		!pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperZoomShoot") ||
		//		!pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperZoomBoltAction"))
		//	{
		//		if (Input.GetButtonDown("Fire1"))
		//		{
		//			Shoot();
		//		}
		//	}
		//}
		#endregion

		#region Shoot Animation
		if (pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperBoltAction"))
		{
			pA.playerAnimation.SetBool("Shoot", false);
			//pA.playerAnimation.SetBool("Bolt", true);
		}

		if (pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperShoot"))
		{
			pA.playerAnimation.SetBool("Shoot", true);
			//pA.playerAnimation.SetBool("Bolt", false);
		}

		if (pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperBoltAction") && boltTimer > 0 || pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomBoltAction") && boltTimer > 0)
		{
			//pA.playerAnimation.SetBool("Bolt", true);
		}
		if (pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperBoltAction") && boltTimer < 0 && boltTimer > -0.2f || pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomBoltAction") && boltTimer < 0 && boltTimer > -0.2f)
		{
			pA.playerAnimation.SetBool("Bolt", false);
		}

		if (pA.playerAnimation.GetBool("Bolt"))
		{
			boltTimer -= Time.deltaTime;
			//boltBool = true;
		}
		if (!pA.playerAnimation.GetBool("Bolt"))
		{
			//boltBool = false;
		}
		if (boltTimer < 0f && boltTimer > -0.2f)
		{
			pA.playerAnimation.SetBool("Bolt", false);
			boltTimer -= Time.deltaTime;
		}
		if (boltTimer <= -0.2f)
		{
			boltTimer = 1f;
		}
		#endregion

		#region Shoot While Zoomed Animation
		if (pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperZoomBoltAction"))
		{
			pA.playerAnimation.SetBool("Shoot", false);
			//pA.playerAnimation.SetBool("Bolt", true);
		}

		if (pA.playerAnimation.GetNextAnimatorStateInfo(0).IsName("SniperZoomShoot"))
		{
			pA.playerAnimation.SetBool("Shoot", true);
			//pA.playerAnimation.SetBool("Bolt", false);
		}
		#endregion

	}

	void CheckIfCanShoot()
	{
		if (!pause.activeInHierarchy)
		{
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
											if (ammoCount > 0)
											{
												if (!pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
												{
													if (Input.GetButtonDown("Fire1"))
													{
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

		if (shootBool)
		{
			if (shootTimer >= 0 || shootTimer == 1.3f)
			{
				shootTimer -= Time.deltaTime;
			}
			if (shootTimer < 0)
			{
				shootBool = false;
				shootTimer = 1.3f;
			}
		}
	}

	void Shoot()
	{
		shootBool = true;

		ammoCount = ammoCount - 1;

		pA.playerAnimation.SetBool("Shoot", true);
		pA.playerAnimation.SetBool("Bolt", true);

		muzzelFlash.Play();

		RaycastHit hit;

		if (pA.playerAnimation.GetBool("Aim"))
		{
			if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward, out hit, range))
			{
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

				var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				GameObject gunShot = Instantiate(gunshotDecal, hit.point, hitRotation);
				gunShot.transform.SetParent(hit.transform);
				GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(impactGO, 2f);
				Destroy(gunShot, 20f);
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
			//Physics.Raycast(gunCam.transform.position, gunCam.transform.forward, out hit, range)
			Vector2 RandomShot = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
			if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward + new Vector3(RandomShot.x, 0, RandomShot.y), out hit, range))
			{
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

				var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				GameObject gunShot = Instantiate(gunshotDecal, hit.point, hitRotation);
				gunShot.transform.SetParent(hit.transform);
				GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(impactGO, 2f);
				Destroy(gunShot, 20f);

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

	void Reload()
	{
		if (ammoCount == 0)
		{
			reload = true;
		}

		pA.playerAnimation.SetBool("Reload", reload);

		if (pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
		{
			if (pA.playerAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				ammoCount = maxAmmo;
			}
		}

		if (Input.GetButtonDown("Reload"))
		{
			if (ammoCount < maxAmmo)
			{
				reload = true;
				pA.playerAnimation.SetBool("Reload", reload);
			}
		}
	}

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
}
