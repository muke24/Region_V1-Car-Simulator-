using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace New
{
	public class PlayerInput : MonoBehaviour
	{
		private Inputs inputs;
		private Vector2 i;
		private Vector2 iRaw;
		private Vector2 previous;
		private Vector2 _down;
		private float scrollValue = 0f;
		private int jumping = 0;
		private int jumpTimer;
		private bool jump = false;
		private bool sprinting = false;
		private bool _crouch = false;
		private bool _crouching = false;
		private bool interacting = false;
		private bool reloading = false;
		private bool aimDown = false;
		private bool isAiming = false;
		private bool shoot = false;

		public Vector2 Input
		{
			get
			{
				inputs.Player.WASD.performed += ctx =>
				{
					Vector2 value = ctx.ReadValue<Vector2>();

					i = value;
					i *= (i.x != 0.0f && i.y != 0.0f) ? .7071f : 1.0f;
				};

				inputs.Player.WASD.canceled += ctx =>
				{
					i = ctx.ReadValue<Vector2>();
				};

				return i;
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
				inputs.Player.WASD.performed += ctx =>
				{
					var value = ctx.ReadValue<Vector2>();   // Read value from control.
					var control = ctx.control;
					var button = control as ButtonControl;

					if (value.x > 0)
						iRaw.x = 1;

					if (value.x < 0)
						iRaw.x = -1;

					if (value.y > 0)
						iRaw.y = 1;

					if (value.y < 0)
						iRaw.y = -1;

					iRaw *= (i.x != 0.0f && i.y != 0.0f) ? .7071f : 1.0f;

					_down = Vector2.zero;
					if (Raw.x != previous.x)
					{
						previous.x = Raw.x;
						if (previous.x != 0)
							_down.x = previous.x;
					}
					if (Raw.y != previous.y)
					{
						previous.y = Raw.y;
						if (previous.y != 0)
							_down.y = previous.y;
					}
				};

				inputs.Player.WASD.canceled += ctx =>
				{
					iRaw = ctx.ReadValue<Vector2>();
				};

				return iRaw;
			}
		}

		public float Elevate
		{
			get
			{
				inputs.Player.Jump.performed += ctx =>
				{
					jumping = 1;
				};
				inputs.Player.Jump.canceled += ctx =>
				{
					jumping = 0;
				};
				return jumping;
			}
		}

		public bool Run
		{
			get
			{
				inputs.Player.Sprint.started += ctx =>
				{
					sprinting = true;
				};
				inputs.Player.Sprint.canceled += ctx =>
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
				if (inputs.Player.Crouch.triggered)
				{
					_crouch = true;
				}
				else
				{
					_crouch = false;
				}
				return _crouch;
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
			inputs.Player.Jump.performed += ctx =>
			{
				if (jumpTimer > 0)
				{
					jump = true;
				}
			};
			inputs.Player.Jump.canceled += ctx =>
			{
				jump = false;
			};

			return jump;
		}

		public void ResetJump()
		{
			jumpTimer = -1;
		}
	}
}

