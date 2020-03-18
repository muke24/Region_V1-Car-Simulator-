using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
	public Animator playerAnimation;

	public bool getAxisVertical;

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

		if (!Input.GetButton("Vertical"))
		{
			//playerAnimation.Play("Idle", 1);
			playerAnimation.SetBool("Moving", false);
		}
		if (Input.GetButton("Vertical"))
		{
			//playerAnimation.Play("Moving", 1, Input.GetAxis("Vertical"));
			playerAnimation.SetBool("Moving", true);
			playerAnimation.SetFloat("MoveSpeed", Input.GetAxis("Vertical"));
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
