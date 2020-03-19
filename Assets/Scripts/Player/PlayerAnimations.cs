using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
	public Animator playerAnimation;
	public float vertical;
	public float horizontal;

	// Update is called once per frame
	void Update()
	{
		/*
		if (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.S))
		{
			getAxisVertical = false;
		}
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
		{
			getAxisVertical = true;
		}
		*/

		if (!Input.GetButton("Vertical") || !Input.GetButton("Horizontal"))
		{
			//playerAnimation.Play("Idle", 1);
			playerAnimation.SetBool("Moving", false);
		}
		if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{
			if (Input.GetAxis("Vertical") > 0)
			{
				vertical = Input.GetAxis("Vertical");
			}
			if (Input.GetAxis("Vertical") < 0)
			{
				vertical = -Input.GetAxis("Vertical");
			}

			if (Input.GetAxis("Horizontal") > 0)
			{
				horizontal = Input.GetAxis("Horizontal");
			}
			if (Input.GetAxis("Horizontal") < 0)
			{
				horizontal = -Input.GetAxis("Horizontal");
			}
			
			if (Input.GetButton("Vertical") && !Input.GetButton("Horizontal"))
			{
				playerAnimation.SetFloat("MoveSpeed", vertical);
			}
			if (Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
			{
				playerAnimation.SetFloat("MoveSpeed", horizontal);
			}

			if (Input.GetButton("Vertical") && Input.GetButton("Horizontal"))
			{
				playerAnimation.SetFloat("MoveSpeed", (vertical / 2) + (horizontal / 2));
			}
			
			playerAnimation.SetBool("Moving", true);
			



			/*
			if (Input.GetButton("Vertical"))
			{
				playerAnimation.SetBool("Moving", true);
				playerAnimation.SetFloat("MoveSpeed", Input.GetAxis("Vertical"));
			}
			if (Input.GetButton("Horizontal"))
			{
				playerAnimation.SetBool("Moving", true);
				playerAnimation.SetFloat("MoveSpeed", Input.GetAxis("Horizontal"));
			}
			*/
			//playerAnimation.Play("Moving", 1, Input.GetAxis("Vertical"));

		}

		if (!Input.GetMouseButton(1))
		{
			playerAnimation.SetBool("Aim", false);
			//playerAnimation.Play("SniperIdle", 0);
		}
		if (Input.GetMouseButton(1))
		{
			playerAnimation.SetBool("Aim", true);
			//playerAnimation.Play("SniperZoom", 0);
		}

	}
}
