using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
	public PlayerMovement pMovement;
	public Animator playerAnimation;
	public float vertical;
	public float horizontal;
	public float animPos;

	private void Start()
	{
		//playerAnimation.StartPlayback();
	}

	// Update is called once per frame
	void Update()
	{
		if (!Input.GetMouseButton(1))
		{
			playerAnimation.SetBool("Aim", false);
		}
		if (Input.GetMouseButton(1))
		{
			playerAnimation.SetBool("Aim", true);
		}

		if (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
		{
			playerAnimation.SetBool("Moving", false);
			playerAnimation.SetBool("MovingLeft", false);
			playerAnimation.SetBool("MovingRight", false);
		}
		if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{


			playerAnimation.SetBool("Moving", true);

			if (Input.GetAxis("Vertical") > 0)
			{
				vertical = Input.GetAxis("Vertical");
			}
			if (Input.GetAxis("Vertical") < 0)
			{
				vertical = -Input.GetAxis("Vertical");
			}
			if (Input.GetAxis("Vertical") == 0)
			{
				vertical = 0;
			}

			if (Input.GetAxis("Horizontal") > 0)
			{
				horizontal = Input.GetAxis("Horizontal");
			}
			if (Input.GetAxis("Horizontal") < 0)
			{
				horizontal = -Input.GetAxis("Horizontal");
			}
			if (Input.GetAxis("Horizontal") == 0)
			{
				horizontal = 0;
			}

			if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") == 0)
			{
				playerAnimation.SetFloat("MoveSpeed", vertical);
			}
			if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") == 0)
			{
				playerAnimation.SetFloat("MoveSpeed", horizontal);
			}

			if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
			{
				playerAnimation.SetFloat("MoveSpeed", (vertical + horizontal) / 2);
			}
		}

		if (Input.GetAxis("Horizontal") != 0)
		{
			if (Input.GetAxis("Horizontal") > 0)
			{
				playerAnimation.SetBool("MovingRight", true);
				playerAnimation.SetBool("MovingLeft", false);
			}
			if (Input.GetAxis("Horizontal") < 0)
			{
				playerAnimation.SetBool("MovingLeft", true);
				playerAnimation.SetBool("MovingRight", false);
			}
		}

		if (Input.GetButton("Jump") && pMovement.controller.isGrounded)
		{
			playerAnimation.SetBool("Jump", true);
		}

		if (Input.GetAxis("Vertical") == 0)
		{
			vertical = 0;
		}
		if (Input.GetAxis("Horizontal") == 0)
		{
			horizontal = 0;
		}

		
	}
}
