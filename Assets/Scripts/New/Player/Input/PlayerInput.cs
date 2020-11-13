using Mirror;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace New
{
	public class PlayerInput : InputManager
	{
		private Inputs inputs;
		private Vector2 previous;
		private Vector2 _down;
		private float scrollValue = 0f;
		private int jumping = 0;
		private int jumpTimer;
		private bool jump = false;
		private bool sprinting = false;
		private bool _crouching = false; //////
		private bool interacting = false;
		private bool reloading = false;
		private bool aimDown = false;
		private bool isAiming = false;
		private bool shoot = false;
		private Action<InputAction.CallbackContext> callback;

		public Vector2 Input
		{
			get
			{
				//Vector2 WASD = inputs.Player.WASD.ReadValue<Vector2>();

				Vector2 WASD = GetAxis2D("Move");
				WASD *= (WASD.x != 0.0f && WASD.y != 0.0f) ? .7071f : 1.0f;

				return WASD;
			}
		}

		public Vector2 Down
		{
			get { return _down; }
		}

		public Vector2 Raw
		{
			get
			{
				//Vector2 WASD = inputs.Player.WASD.ReadValue<Vector2>();
				Vector2 WASD = GetAxis2D("Move");

				WASD *= (WASD.x != 0.0f && WASD.y != 0.0f) ? .7071f : 1.0f;

				_down = Vector2.zero;
				if (WASD.x != previous.x)
				{
					previous.x = WASD.x;
					if (previous.x != 0)
						_down.x = previous.x;
				}
				if (WASD.y != previous.y)
				{
					previous.y = WASD.y;
					if (previous.y != 0)
						_down.y = previous.y;
				}

				return WASD;
			}
		}

		public float Elevate
		{
			get
			{
				if (OnButton.Hold("Jump"))
				{
					return 1;
				}
				else
				{
					return 0;
				}

				//bool elev = false;
				//inputs.Player.Jump.performed += callback =>
				//{
				//	elev = true;
				//};
				//if (elev)
				//{
				//	return 1;
				//}
				//else
				//{
				//	return 0;
				//}
			}
		}

		public bool Run
		{
			get
			{
				InputAction Sprint = inputs.Player.Sprint;
				Sprint.started += ctx =>
				{
					sprinting = true;
				};
				Sprint.canceled += ctx =>
				{
					sprinting = false;
				};
				return sprinting;
			}
		}

		public bool Crouch
		{
			get
			{
				//InputAction Crouch = inputs.Player.Crouch;
				//if (inputs.Player.Crouch.triggered)
				//{
				//	_crouch = true;
				//}
				//else
				//{
				//	_crouch = false;
				//}
				return inputs.Player.Crouch.triggered;
			}
		}

		public bool Crouching
		{
			get
			{
				inputs.Player.Crouch.started += ctx =>
				{
					_crouching = true;
				};
				inputs.Player.Crouch.canceled += ctx =>
				{
					_crouching = false;
				};

				return _crouching;
			}
		}

		public bool InteractKey
		{
			get
			{
				inputs.Player.Interact.started += ctx =>
				{
					interacting = true;
					Debug.Log(interacting);
				};
				inputs.Player.Interact.canceled += ctx =>
				{
					interacting = false;
					Debug.Log(interacting);
				};

				return interacting;
			}
		}

		public bool Interact
		{
			get
			{
				return InteractKey;
			}
		}

		public bool Reload
		{
			get
			{
				inputs.Player.Reload.started += ctx =>
				{
					reloading = true;
				};
				inputs.Player.Reload.canceled += ctx =>
				{
					reloading = false;
				};
				return reloading;
			}
		}

		public bool Aim
		{
			get
			{
				inputs.Player.Aim.started += ctx =>
				{
					aimDown = true;
				};
				inputs.Player.Aim.canceled += ctx =>
				{
					aimDown = false;
				};

				return aimDown;
			}
		}

		public bool Aiming
		{
			get
			{
				inputs.Player.Aim.performed += ctx =>
				{
					isAiming = true;
				};
				inputs.Player.Aim.canceled += ctx =>
				{
					isAiming = false;
				};
				return isAiming;
			}
		}

		public bool Shooting
		{
			get
			{
				inputs.Player.Shoot.performed += ctx =>
				{
					shoot = true;
				};
				inputs.Player.Shoot.canceled += ctx =>
				{
					shoot = false;
				};
				return shoot;
			}
		}

		public float MouseScroll
		{
			get
			{
				inputs.Player.Scroll.performed += ctx =>
				{
					if (ctx.ReadValue<float>() != 0f)
					{
						if (scrollValue > 0f)
						{
							scrollValue = 1f;
						}
						else
						{
							scrollValue = -1f;
						}
					}
				};

				inputs.Player.Scroll.canceled += ctx =>
				{
					scrollValue = 0f;
				};

				return scrollValue;
			}
		}

		public void OnEnable()
		{
			inputs.Enable();
		}

		public void OnDisable()
		{
			inputs.Disable();
		}

		private void Awake()
		{
			inputs = new Inputs();
		}

		void Start()
		{
			jumpTimer = -1;
		}

		public void FixedUpdate()
		{
			if (!jump)
			{
				jumpTimer++;
			}
		}

		public bool Jump()
		{
			if (OnButton.Hold("Jump"))
			{
				jump = true;
				return true;
			}
			else
			{
				jump = false;
				return false;
			}

			//if (inputs.Player.Jump.triggered)
			//{
			//	jump = true;
			//	return true;
			//}
			//else
			//{
			//	jump = false;
			//	return false;
			//}
		}

		public void ResetJump()
		{
			jumpTimer = -1;
		}
	}
}
