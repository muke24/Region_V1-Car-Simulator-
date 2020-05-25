using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementNew : MonoBehaviour
{
	public float speed = 6.0F;
	public float runSpeed = 10.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	public CharacterController controller;
	public string yeet;

	private Vector3 moveDirection = Vector3.zero;
	private Controls controls = null;

	private void Awake()
	{
		controls = new Controls();
	}

	private void OnEnable()
	{
		controls.Player.Enable();
	}

	private void OnDisable()
	{
		controls.Player.Disable();
	}

	private void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		//Applying gravity to the controller
		moveDirection.y -= gravity * Time.deltaTime;
		//Making the character move
		controller.Move(moveDirection * Time.deltaTime);
		yeet = controls.Player.Jump.controls.Last().ToString();
	}

	private void Move()
	{
		var movementInput = controls.Player.Movement.ReadValue<Vector2>();
		var movement = new Vector3()
		{
			x = movementInput.x,
			z = movementInput.y
		}.normalized;

		// is the controller on the ground?
		if (controller.isGrounded)
		{
			//Feed moveDirection with input.
			moveDirection = new Vector3(movement.x, 0, movement.z);
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection *= speed;
			//transform.Translate(movement * speed * Time.deltaTime);
		}

		if (!controller.isGrounded)
		{
			//Feed moveDirection with input.
			moveDirection = new Vector3(movement.x / 1.2f, 0, movement.z / 1.2f);
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection *= speed;
			//transform.Translate(movement * speed * Time.deltaTime);
		}
	}

	public void Sprint()
	{
		var movementInput = controls.Player.Movement.ReadValue<Vector2>();
		var movement = new Vector3()
		{
			x = movementInput.x,
			z = movementInput.y
		}.normalized;

		if (controller.isGrounded && Keyboard.current.leftShiftKey.isPressed)
		{
			//Feed moveDirection with input.
			moveDirection = new Vector3(movement.x, 0, movement.z);
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection *= runSpeed * 10;
			//transform.Translate(movement * runSpeed * Time.deltaTime);
		}

		if (!controller.isGrounded && Keyboard.current.leftShiftKey.isPressed)
		{
			//Feed moveDirection with input.
			moveDirection = new Vector3(movement.x / 1.2f, 0, movement.z / 1.2f);
			moveDirection = transform.TransformDirection(moveDirection);
			//Multiply it by speed.
			moveDirection *= runSpeed * 10;
			//transform.Translate(movement * runSpeed * Time.deltaTime);
		}
	}

	public void Jump()
	{
		if (controller.isGrounded)
		{
			moveDirection.y = jumpSpeed;
		}
	}
}