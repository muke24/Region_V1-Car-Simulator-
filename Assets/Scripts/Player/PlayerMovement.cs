using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6.0F;
	public float runSpeed = 10.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	public CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;

	void Update()
	{
		controller = GetComponent<CharacterController>();
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
				moveDirection.y = jumpSpeed;

		}

		if (controller.isGrounded && Input.GetKey("left shift"))
		{
			//Feed moveDirection with input.
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection *= runSpeed;
			//Jumping
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;

		}

		if (!controller.isGrounded)
		{
			//Feed moveDirection with input.
			moveDirection.x = Input.GetAxis("Horizontal") / 1.2f;
			moveDirection.z = Input.GetAxis("Vertical") / 1.2f;
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection.x *= speed;
			moveDirection.z *= speed;

		}

		if (!controller.isGrounded && Input.GetKey("left shift"))
		{
			//Feed moveDirection with input.
			moveDirection.x = Input.GetAxis("Horizontal") / 1.2f;
			moveDirection.z = Input.GetAxis("Vertical") / 1.2f;
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection.x *= runSpeed;
			moveDirection.z *= runSpeed;

		}
		//Applying gravity to the controller
		moveDirection.y -= gravity * Time.deltaTime;
		//Making the character move
		controller.Move(moveDirection * Time.deltaTime);
	}
}