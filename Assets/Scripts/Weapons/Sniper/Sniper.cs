using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
	[SerializeField]
	private GameObject pause;
	[SerializeField]
	private float damage = 100f;
	[SerializeField]
	private float range = 1000f;
	[SerializeField]
	private float boltTimer = 1f;
	[SerializeField]
	private float shootTimer = 1.3f;
	[SerializeField]
	private Camera gunCam;
	[SerializeField]
	private PlayerAnimations pA;
	[SerializeField]
	private bool boltBool;
	[SerializeField]
	private bool shootBool;
	[SerializeField]
	private GameObject gunshotDecal;
	[SerializeField]
	private ParticleSystem muzzelFlash;
	[SerializeField]
	private GameObject impactEffect;
	[SerializeField]
	private bool reload;

	public int maxAmmo = 5;
	public int ammoCount = 5;
	//public AudioSource gunShot;

	[Header("Sniper Sensitivity")]
	float HorizontalSpeed = 2.0f;
	float VerticalSpeed = 2.0f;

	float scopedHorizontalSpeed = 1.0f;
	float scopedVerticalSpeed = 1.0f;

	// Update is called once per frame
	void Update()
	{
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
			boltBool = true;
		}
		if (!pA.playerAnimation.GetBool("Bolt"))
		{
			boltBool = false;
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

		if (PlayerAnimations.scoped)
		{
			if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward, out hit, range))
			{
				Debug.Log("Gunshot hit " + hit.transform.name);

				var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				Instantiate(gunshotDecal, hit.point, hitRotation);
				GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(impactGO, 2f);
			}
		}
		else
		{
			//Physics.Raycast(gunCam.transform.position, gunCam.transform.forward, out hit, range)
			Vector2 RandomShot = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
			if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward + new Vector3(RandomShot.x, 0, RandomShot.y), out hit, range))
			{
				Debug.Log("Gunshot hit " + hit.transform.name);

				var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				GameObject gunShot = Instantiate(gunshotDecal, hit.point, hitRotation);
				gunShot.transform.SetParent(hit.transform);
				GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(impactGO, 2f);
				Destroy(gunShot, 20f);
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
				ammoCount = 5;
			}
		}

		if (Input.GetButtonDown("Reload"))
		{
			if (ammoCount < 5)
			{
				reload = true;
				pA.playerAnimation.SetBool("Reload", reload);
			}
		}
	}
}
