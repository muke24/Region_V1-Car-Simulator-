using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
	[Header("Gun Animations")]
	public Animator animator;
	public GameObject scopedCam;


	[Header("Shoot")]

	public float damage = 100f;
	public float range = 1000f;
	public float fireRate = 1f;
	public float bulletCount = 5f;

	[Space(10)]
	
	public ParticleSystem muzzelFlash;
	public GameObject raycastAim;
	public GameObject impactEffect;

	public AudioSource gunShot;

	private float nextTimeToFire = 0f;

	[Header("Sensitivity")]
	float HorizontalSpeed = 2.0f;
	float VerticalSpeed = 2.0f;

	float scopedHorizontalSpeed = 1.0f;
	float scopedVerticalSpeed = 1.0f;

	private bool isScoped = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
