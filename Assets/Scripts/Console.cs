using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
	public static bool _cheat1; // Gives user unlimited ammo
	public static bool _cheat2; // Gives user flying ability
	public static bool _cheat3; // Gives user unlimited health

	public static bool isFlying;
	public bool canFly;
	public float doubleTapTime = 0.3f;
	public bool flying;

	[SerializeField]
	private InputField console;
	[SerializeField]
	private Animator consoleAnim;
	[SerializeField]
	private bool consoleToggle = false;
	[SerializeField]
	private bool consoleCanToggle = true;
	[SerializeField]
	private string cheat1;
	[SerializeField]
	private string cheat2;
	[SerializeField]
	private string cheat3;
	[SerializeField]
	private string disableCheats;

	// Update is called once per frame
	void Update()
	{
		ConsoleToggle();
		SetCheats();
	}

	public void ActivateCommand()
	{
		if (console.text == cheat1)
		{
			_cheat1 = true;
		}
		if (console.text == cheat2)
		{
			_cheat2 = true;
		}
		if (console.text == cheat3)
		{
			_cheat3 = true;
		}

		if (console.text == "!" + cheat1)
		{
			_cheat1 = false;
		}
		if (console.text == "!" + cheat2)
		{
			_cheat2 = false;
		}
		if (console.text == "!" + cheat3)
		{
			_cheat3 = false;
		}

		if (console.text == disableCheats)
		{
			_cheat1 = false;
			_cheat2 = false;
			_cheat3 = false;
		}
	}

	void ConsoleToggle()
	{
		if (!consoleToggle)
		{
			console.readOnly = true;
		}
		
		if (consoleToggle)
		{
			EventSystem.current.SetSelectedGameObject(console.gameObject, null);
			console.OnPointerClick(new PointerEventData(EventSystem.current));

			if (Input.GetKey(KeyCode.BackQuote))
			{
				console.readOnly = true;
			}
			if (!Input.GetKey(KeyCode.BackQuote))
			{
				console.readOnly = false;
			}

			
		}

		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			if (!consoleToggle)
			{
				if (consoleCanToggle)
				{
					consoleAnim.SetBool("consoleToggle", true);
					consoleToggle = true;
					consoleCanToggle = false;
				}
			}
			if (consoleToggle)
			{
				if (consoleCanToggle)
				{
					consoleAnim.SetBool("consoleToggle", false);
					consoleToggle = false;
					consoleCanToggle = false;
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit"))
		{
			if (consoleToggle)
			{
				if (consoleCanToggle)
				{
					consoleAnim.SetBool("consoleToggle", false);
					consoleToggle = false;
					consoleCanToggle = false;
				}
			}
		}

		if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(0) || Input.GetButtonUp("Submit"))
		{
			if (!consoleToggle)
			{
				console.text = "";
				consoleCanToggle = true;
			}
		}

		if (Input.GetKeyUp(KeyCode.BackQuote))
		{
			console.text = "";
			consoleCanToggle = true;
		}
	}

	void SetCheats()
	{
		if (_cheat1) // Max Ammo
		{
			Sniper.ammoCount = Sniper.maxAmmo;
		}

		if (_cheat2) // Can fly
		{
			if (!PlayerMovement.controller.isGrounded && Input.GetButton("Jump") && !Input.GetButton("Crouch"))
			{
				PlayerMovement.moveDirection *= PlayerMovement.speed * 2f;
				//Jumping
				PlayerMovement.moveDirection.y = PlayerMovement.jumpSpeed;
			}
			if (!PlayerMovement.controller.isGrounded && Input.GetButton("Crouch") && !Input.GetButton("Jump"))
			{
				PlayerMovement.moveDirection *= PlayerMovement.speed * 2f;
				//Jumping
				PlayerMovement.moveDirection.y = PlayerMovement.jumpSpeed * -1f;
			}
			if (!PlayerMovement.controller.isGrounded && !Input.GetButton("Jump"))
			{
				if (!PlayerMovement.controller.isGrounded && !Input.GetButton("Crouch"))
				{
					PlayerMovement.moveDirection.y = 0f;
				}
			}
			if (!PlayerMovement.controller.isGrounded && Input.GetButton("Jump") && Input.GetButton("Crouch"))
			{
				PlayerMovement.moveDirection.y = 0f;
			}
		}

		if (_cheat3)
		{

		}
	}
}
