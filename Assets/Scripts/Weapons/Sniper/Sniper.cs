using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
	[Header("Shoot")]

	public float damage = 100f;
	public float range = 1000f;
	public float fireRate = 1f;
	public float bulletCount = 5f;

	[Space(10)]

	public bool scoped;

	public Camera gunCam;

	public GameObject gunshotDecal;

	public ParticleSystem muzzelFlash;
	public GameObject impactEffect;
	public AudioSource gunShot;

	private float nextTimeToFire = 0f;

	[Header("Sensitivity")]
	float HorizontalSpeed = 2.0f;
	float VerticalSpeed = 2.0f;

	float scopedHorizontalSpeed = 1.0f;
	float scopedVerticalSpeed = 1.0f;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Shoot();
		}
	}

	void Shoot()
	{
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
			Vector2 RandomShot = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
			if (Physics.Raycast(gunCam.transform.position, gunCam.transform.forward + new Vector3(RandomShot.x, RandomShot.y, 0), out hit, range))
			{
				Debug.Log("Gunshot hit " + hit.transform.name);

				var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				Instantiate(gunshotDecal, hit.point, hitRotation);
				GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
				Destroy(impactGO, 2f);

			}
		}



	}
}
