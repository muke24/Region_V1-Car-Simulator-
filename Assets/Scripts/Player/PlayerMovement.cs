#region This code is written by Peter Thompson
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public static float speed = 4.5f;
	public static float runSpeed = 7.0f;
	public static float jumpSpeed = 6.0f;
	public static float gravity = 20.0f;
	public static CharacterController controller;

	public bool isWalking;
	public bool isRunning;
	public bool isCrouching;
	public bool isAirBorn;
	public bool isAiming;

	public float walkMultiplier = 1.3f;
	public float runMultiplier = 1.4f;
	public float crouchMultiplier = 1f;
	public float airBornMultiplier = 1f;

	public float aimMoveSpeed = 4f;

	public PlayerAnimations pA;

	public static Vector3 moveDirection = Vector3.zero;

	private void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		Movements();
		AnimatorMultipliers();
	}

	void Movements()
	{		
		// is the controller on the ground?
		if (controller.isGrounded)
		{
			//Feed moveDirection with input.
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection *= speed;
			//Jumping
			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}

			isWalking = true;
			isRunning = false;
			isCrouching = false;
			isAirBorn = false;
		}

		if (controller.isGrounded && Input.GetButton("Sprint") && !Input.GetButton("Aim"))
		{
			//Feed moveDirection with input.
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection *= runSpeed;
			//Jumping
			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
			isWalking = false;
			isRunning = true;
			isCrouching = false;
			isAirBorn = false;
		}

		if (!controller.isGrounded)
		{
			//Feed moveDirection with input.
			moveDirection.x = Input.GetAxis("Horizontal") / 1.15f;
			moveDirection.z = Input.GetAxis("Vertical") / 1.15f;
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection.x *= speed;
			moveDirection.z *= speed;

			isWalking = true;
			isRunning = false;
			isCrouching = false;
			isAirBorn = true;
		}

		if (!controller.isGrounded && Input.GetButton("Sprint"))
		{
			//Feed moveDirection with input.
			moveDirection.x = Input.GetAxis("Horizontal") / 1.1f;
			moveDirection.z = Input.GetAxis("Vertical") / 1.1f;
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection.x *= runSpeed;
			moveDirection.z *= runSpeed;

			isWalking = false;
			isRunning = true;
			isCrouching = false;
			isAirBorn = true;
		}

		if (controller.isGrounded && Input.GetButton("Crouch"))
		{
			//Feed moveDirection with input.
			moveDirection = new Vector3(Input.GetAxis("Horizontal") / 2f, 0, Input.GetAxis("Vertical") / 2f);
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection *= speed;
			//Jumping
			if (Input.GetButton("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}

			isWalking = false;
			isRunning = false;
			isCrouching = true;
			isAirBorn = false;
		}

		if (!controller.isGrounded && Input.GetButton("Crouch"))
		{
			//Feed moveDirection with input.
			moveDirection.x = Input.GetAxis("Horizontal") / 2.1f;
			moveDirection.z = Input.GetAxis("Vertical") / 2.1f;

			//moveDirection = new Vector3(Input.GetAxis("Horizontal") / 2.25f, 0, Input.GetAxis("Vertical") / 2.25f);
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection.x *= speed;
			moveDirection.z *= speed;
			//moveDirection *= speed;

			isWalking = false;
			isRunning = false;
			isCrouching = true;
			isAirBorn = true;
		}

		//Making the character move
		controller.Move(moveDirection * Time.deltaTime);
		if (!Console._cheat2)
		{
			Gravity();
		}		
	}

	void Gravity()
	{
		//Apply gravity to the controller
		moveDirection.y -= gravity * Time.deltaTime;		
	}

	void AnimatorMultipliers()
	{
		if (pA.playerAnimation.GetCurrentAnimatorStateInfo(0).IsTag("MultiplierChangable"))
		{
			if (isWalking)
			{
				pA.playerAnimation.speed = walkMultiplier;
			}
			if (isRunning)
			{
				pA.playerAnimation.speed = runMultiplier;
			}
			if (isCrouching)
			{
				pA.playerAnimation.speed = crouchMultiplier;
			}
			if (isAirBorn)
			{
				pA.playerAnimation.speed = airBornMultiplier;
			}
		}
	}
}
// This code is written by Peter Thompson
#endregion