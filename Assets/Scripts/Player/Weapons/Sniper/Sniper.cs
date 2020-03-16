using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
	public Animator animator;
	public GameObject scopedCam;

	float HorizontalSpeed = 2.0f;
	float VerticalSpeed = 2.0f;

	float scopedHorizontalSpeed = 1.0f;
	float scopedVerticalSpeed = 1.0f;

	private bool isScoped = false;

	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			// Plays the animation to either bring up or bring down the scope after click, and checks which state the scope is in (scoped in or scoped out)
			isScoped = !isScoped;
			animator.SetBool("Scoped", isScoped);
			scopedCam.SetActive(isScoped);
		}
		if (Input.GetMouseButtonUp(1))
		{
			// Plays the animation to either bring up or bring down the scope after click, and checks which state the scope is in (scoped in or scoped out)
			isScoped = !isScoped;
			animator.SetBool("Scoped", isScoped);
			scopedCam.SetActive(isScoped);
		}

		if (isScoped == true)
		{

			float h = scopedHorizontalSpeed * Input.GetAxis("Mouse X");
			float v = scopedVerticalSpeed * Input.GetAxis("Mouse Y");


		}

		if (isScoped == false)
		{

			float h = HorizontalSpeed * Input.GetAxis("Mouse X");
			float v = VerticalSpeed * Input.GetAxis("Mouse Y");


		}
	}
}