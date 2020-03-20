using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
	public PlayerMovement pMovement;
	public Animator playerAnimation;
	public Animation sniperMove;
	public float vertical;
	public float horizontal;
	public float animPos;


	// Update is called once per frame
	void Update()
	{
		if (!Input.GetButton("Vertical") || !Input.GetButton("Horizontal"))
		{
			playerAnimation.SetBool("Moving", false);
		}
		if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{
			//animPos = sniperMove.

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

		if (Input.GetAxis("Vertical") == 0)
		{
			vertical = 0;
		}
		if (Input.GetAxis("Horizontal") == 0)
		{
			horizontal = 0;
		}

		if (!Input.GetMouseButton(1))
		{
			playerAnimation.SetBool("Aim", false);
		}
		if (Input.GetMouseButton(1))
		{
			playerAnimation.SetBool("Aim", true);
		}
	}
}
