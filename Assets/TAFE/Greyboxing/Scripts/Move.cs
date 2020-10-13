using UnityEngine;

public class Move : MonoBehaviour
{
	[SerializeField]
	private CharacterController characterController = null;
	private Vector3 moveDirection;
	private readonly float speed = 7f;
	private readonly float jumpSpeed = 6.0f;
	private readonly float gravity = 20f;

	// Start is called before the first frame update
	void OnValidate()
	{
		if (characterController == null)
		{
			characterController = GetComponent<CharacterController>();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (characterController.isGrounded)
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
		}
		//Apply gravity to the controller
		moveDirection.y -= gravity * Time.deltaTime;

		//Making the character move
		characterController.Move(moveDirection * Time.deltaTime);
	}
}
