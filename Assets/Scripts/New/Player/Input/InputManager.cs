using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEngine.InputSystem.InputAction;

namespace New
{
	public class InputManager : MonoBehaviour
	{
		[SerializeField]
		private int totalControls = 9;

		private Inputs inputs;
		private static Action<CallbackContext>[] actionMethods = new Action<CallbackContext>[9];

		#region Variables
		private static Vector2[] axis2ds;
		private static bool[] buttonDown;
		private static bool[] buttonHold;
		private static bool[] buttonUp;
		private static bool[] buttonNull;
		private static CallbackContext[] callbackContexts;
		#endregion

		private void OnEnable()
		{
			inputs.Enable();
		}

		private void OnDisable()
		{
			inputs.Disable();
		}

		void Awake()
		{
			axis2ds = new Vector2[totalControls];
			buttonDown = new bool[totalControls];
			buttonHold = new bool[totalControls];
			buttonUp = new bool[totalControls];
			buttonNull = new bool[totalControls];
			callbackContexts = new CallbackContext[totalControls];
			SetDefaults();

			inputs = new Inputs();
			Inputs.PlayerActions actions = inputs.Player;
			BindAllActions(actions);
			GetMethods();
		}

		void SetDefaults()
		{
			for (int i = 0; i < totalControls; i++)
			{
				axis2ds[i] = Vector2.zero;
				buttonDown[i] = false;
				buttonHold[i] = false;
				buttonUp[i] = false;
				//buttonNull[i] = true;
			}
		}

		void BindAction(InputAction action, Action<CallbackContext> method)
		{
			//action.cal
			action.started += method;
			action.performed += method;
			action.canceled += method;
		}

		void BindAllActions(Inputs.PlayerActions actions)
		{
			BindAction(actions.WASD, Move);
			BindAction(actions.Jump, Jump);
			BindAction(actions.Sprint, Sprint);
			BindAction(actions.Crouch, Crouch);
			BindAction(actions.Interact, Interact);
			BindAction(actions.Reload, Reload);
			BindAction(actions.Aim, Aim);
			BindAction(actions.Shoot, Shoot);
			BindAction(actions.Scroll, Scroll);
		}

		void GetMethods()
		{
			actionMethods[0] = Move;
			actionMethods[1] = Jump;
			actionMethods[2] = Sprint;
			actionMethods[3] = Crouch;
			actionMethods[4] = Interact;
			actionMethods[5] = Reload;
			actionMethods[6] = Aim;
			actionMethods[7] = Shoot;
			actionMethods[8] = Scroll;
		}

		public static Vector2 GetAxis2D(string AxisName)
		{
			for (int i = 0; i < actionMethods.Length; i++)
			{
				if (AxisName == actionMethods[i].Method.Name)
				{
					return axis2ds[i];
				}
			}
			Debug.LogError("2D axis name is incorrect. Please check if you have misspelled the 2D axis name.");
			return Vector2.zero;
		}

		protected static class OnButton
		{
			//public static bool Null(string ButtonName)
			//{
			//	for (int i = 0; i < actionMethods.Length; i++)
			//	{
			//		if (ButtonName == actionMethods[i].Method.Name)
			//		{
			//			if (!buttonDown[i] && !buttonHold[i] && !buttonUp[i])
			//			{
			//				return true;
			//			}
			//			else
			//			{
			//				return false;
			//			}
			//		}
			//	}
			//	return false;
			//	//Debug.LogError("Button down name is incorrect. Please check if you have misspelled the button down name.");
			//	//return false;
			//}

			public static bool Down(string ButtonName)
			{
				for (int i = 0; i < actionMethods.Length; i++)
				{
					if (ButtonName == actionMethods[i].Method.Name)
					{
						return buttonDown[i];
					}
				}
				Debug.LogError("Button name is incorrect. Please check if you have misspelled the button name.");
				return false;
			}

			public static bool Hold(string ButtonName)
			{
				for (int i = 0; i < actionMethods.Length; i++)
				{
					if (ButtonName == actionMethods[i].Method.Name)
					{
						return buttonHold[i];
					}
				}
				Debug.LogError("Button name is incorrect. Please check if you have misspelled the button name.");
				return false;
			}

			//public static bool Up(string ButtonName)
			//{
			//	for (int i = 0; i < actionMethods.Length; i++)
			//	{
			//		if (ButtonName == actionMethods[i].Method.Name)
			//		{
			//			if (buttonUp[i])
			//			{
			//				buttonUp[i] = false;
			//				Up(ButtonName);
			//				return true;
			//			}
			//			else
			//			{
			//				return false;
			//			}
			//		}
			//		return false;
			//	}
			//	Debug.LogError("Button up name is incorrect. Please check if you have misspelled the button up name.");
			//	return false;
			//}
		}

		void Move(CallbackContext obj)
		{
			axis2ds[0] = obj.ReadValue<Vector2>();
		}

		private void Jump(CallbackContext obj)
		{
			ButtonControl button = obj.control as ButtonControl;
			//buttonDown[1] = button.wasPressedThisFrame;
			//buttonHold[1] = button.isPressed;
			//buttonUp[1] = button.wasReleasedThisFrame;

			if (button.wasPressedThisFrame)
			{
				buttonDown[1] = true;
			}
			else
			{
				buttonDown[1] = false;
			}

			if (button.isPressed)
			{
				buttonHold[1] = true;
			}
			else
			{
				buttonHold[1] = true;
			}

			if (button.wasReleasedThisFrame)
			{

			}
			else
			{

			}

			#region OldCode
			//if (button.wasPressedThisFrame)
			//{
			//	buttonDown[1] = true;
			//}
			//if (obj.started)
			//{
			//	buttonDown[1] = true;
			//}
			//else
			//{
			//	buttonDown[1] = false;
			//}
			//if (obj.performed)
			//{
			//	buttonHold[1] = true;
			//}
			//else
			//{
			//	buttonHold[1] = false;
			//}
			#endregion
		}

		private void Sprint(CallbackContext obj)
		{

		}

		private void Crouch(CallbackContext obj)
		{

		}

		private void Interact(CallbackContext obj)
		{

		}

		private void Reload(CallbackContext obj)
		{

		}

		private void Aim(CallbackContext obj)
		{

		}

		private void Shoot(CallbackContext obj)
		{

		}

		private void Scroll(CallbackContext obj)
		{

		}
	}
}
