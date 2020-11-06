﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;
using Mirror;

namespace New
{
	public enum Status { idle, walking, crouching, sprinting, sliding, climbingLadder, wallRunning, vaulting, grabbedLedge, climbingLedge, surfaceSwimming, underwaterSwimming, skyDiving, gliding }
	public class StatusEvent : UnityEvent<Status, Func<IKData>> { }
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(PlayerMovement))]
	public class PlayerController : NetworkBehaviour
	{
		public Status status;
		public LayerMask collisionLayer; //Default
		public float crouchHeight = 1f;
		public PlayerInfo info;
		[SerializeField]
		private float sprintTime = 6f;
		[SerializeField]
		private float sprintReserve = 4f;
		[SerializeField]
		private float sprintMinimum = 2f;
		[SerializeField]
		private Vector3 cameraOffset;
		[SerializeField]
		private Transform cameraParent;

		new CameraMovement camera;
		PlayerMovement movement;
		PlayerInput playerInput;
		AnimateLean animateLean;
		AnimateCameraLevel animateCamLevel;

		bool canInteract;
		bool forceSprintReserve = false;

		float crouchCamAdjust;
		float stamina;

		public StatusEvent onStatusChange;
		List<MovementType> movements;
		WallrunMovement wallrun;
		SurfaceSwimmingMovement swimming;

		public void ChangeStatus(Status s)
		{
			if (status == s) return;
			status = s;
			if (onStatusChange != null)
				onStatusChange.Invoke(status, null);
		}

		public void ChangeStatus(Status s, Func<IKData> call)
		{
			if (status == s) return;
			status = s;
			if (onStatusChange != null)
				onStatusChange.Invoke(status, call);
		}

		public void AddToStatusChange(UnityAction<Status, Func<IKData>> action)
		{
			if (onStatusChange == null)
				onStatusChange = new StatusEvent();

			onStatusChange.AddListener(action);
		}

		public void AddMovementType(MovementType move)
		{
			if (movements == null) movements = new List<MovementType>();
			move.SetPlayerComponents(movement, playerInput);

			if ((move as WallrunMovement) != null) // If this move type is a Wallrunning
				wallrun = (move as WallrunMovement);
			else if ((move as SurfaceSwimmingMovement) != null) // If this move type is a Surface Swimming
				swimming = (move as SurfaceSwimmingMovement);

			movements.Add(move);
		}

		public SurfaceSwimmingMovement GetSwimmingMovement()
		{
			return swimming;
		}

		private void Start()
		{
			if (isLocalPlayer)
			{
				Camera.main.transform.SetParent(cameraParent);
				Camera.main.gameObject.AddComponent<CameraMovement>();
				Camera.main.GetComponent<CameraMovement>().characterBody = gameObject;
				Camera.main.transform.localPosition = cameraOffset;
				Camera.main.transform.rotation = Quaternion.identity;

				playerInput = GetComponent<PlayerInput>();

				movement = GetComponent<PlayerMovement>();
				movement.AddToReset(() => { status = Status.walking; });

				camera = GetComponentInChildren<CameraMovement>();

				if (GetComponentInChildren<AnimateLean>())
					animateLean = GetComponentInChildren<AnimateLean>();
				if (GetComponentInChildren<AnimateCameraLevel>())
					animateCamLevel = GetComponentInChildren<AnimateCameraLevel>();

				info = new PlayerInfo(movement.controller.radius, movement.controller.height);
				crouchCamAdjust = (crouchHeight - info.height) / 2f;
				stamina = sprintTime;
			}
			else
			{
				Destroy(GetComponent<PlayerMovement>());
				Destroy(GetComponent<InterpolatedTransformUpdater>());
				Destroy(GetComponent<PlayerInput>());
				Destroy(this);
			}			
		}

		void Update()
		{
			// Updates
			UpdateInteraction();
			UpdateMovingStatus();

			// Checks
			CheckCrouching();
			foreach (MovementType moveType in movements)
			{
				if (moveType.enabled)
					moveType.Check(canInteract);
			}

			// Misc
			UpdateLean();
			UpdateCamLevel();
		}

		void UpdateInteraction()
		{
			if ((int)status >= 5)
				canInteract = false;
			else if (!canInteract)
			{
				if (movement.grounded || movement.moveDirection.y < 0)
					canInteract = true;
			}
		}

		void UpdateMovingStatus()
		{
			if (status == Status.sprinting && stamina > 0)
				stamina -= Time.deltaTime;
			else if (stamina < sprintTime)
				stamina += Time.deltaTime;

			if ((int)status <= 1 || IsSprinting())
			{
				if (playerInput.Input.magnitude > 0.02f)
					ChangeStatus((ShouldSprint()) ? Status.sprinting : Status.walking);
				else
					ChangeStatus(Status.idle);
			}
		}

		public bool ShouldSprint()
		{
			bool sprint = false;
			sprint = (playerInput.Run && playerInput.Input.y > 0);
			if (status != Status.sliding)
			{
				if (!IsSprinting()) //If we want to sprint
				{
					if (forceSprintReserve && stamina < sprintReserve)
						return false;
					else if (!forceSprintReserve && stamina < sprintMinimum)
						return false;
				}
				if (stamina <= 0)
				{
					forceSprintReserve = true;
					return false;
				}
			}
			if (sprint)
				forceSprintReserve = false;
			return sprint;
		}

		void UpdateLean()
		{
			if (animateLean == null) return;
			Vector2 lean = Vector2.zero;
			if (status == Status.wallRunning)
				lean.x = GetWallrunDir();
			if (status == Status.sliding)
				lean.y = -1;
			else if (status == Status.climbingLedge || status == Status.vaulting)
				lean.y = 1;
			animateLean.SetLean(lean);
		}

		void UpdateCamLevel()
		{
			if (animateCamLevel == null) return;

			float level = 0f;
			if (status == Status.crouching || status == Status.sliding || status == Status.vaulting || status == Status.climbingLedge || status == Status.underwaterSwimming)
				level = crouchCamAdjust;
			animateCamLevel.UpdateLevel(level);
		}

		int GetWallrunDir()
		{
			int wallDir = 0;
			if (wallrun != null)
				wallDir = wallrun.getWallDir();
			return wallDir;
		}
		/*********************************************************************/


		/******************************** MOVE *******************************/
		void FixedUpdate()
		{
			foreach (MovementType moveType in movements)
			{
				if (status == moveType.changeTo)
				{
					moveType.Movement();
					return;
				}
			}

			DefaultMovement();
		}

		void DefaultMovement()
		{
			if (IsSprinting() && IsCrouching())
				Uncrouch();

			movement.Move(playerInput.Input, IsSprinting(), IsCrouching());
			if (movement.grounded && playerInput.Jump())
			{
				if (status == Status.crouching)
				{
					if (!Uncrouch())
						return;
				}

				movement.Jump(Vector3.up, 1f);
				playerInput.ResetJump();
			}
		}

		
		public bool IsSprinting()
		{
			return (status == Status.sprinting && movement.grounded);
		}

		public bool IsWalking()
		{
			if (status == Status.walking || status == Status.crouching)
				return (movement.controller.velocity.magnitude > 0f && movement.grounded);
			else
				return false;
		}

		public bool IsCrouching()
		{
			return (status == Status.crouching);
		}

		void CheckCrouching()
		{
			if (!movement.grounded || (int)status > 2) return;

			if (playerInput.Run)
			{
				Uncrouch();
				return;
			}

			if (playerInput.Crouch)
			{
				if (status != Status.crouching)
					Crouch(true);
				else
					Uncrouch();
			}
		}

		public void Crouch(bool setStatus)
		{
			movement.controller.height = crouchHeight;
			if (setStatus) ChangeStatus(Status.crouching);
		}

		public bool Uncrouch()
		{
			Vector3 bottom = transform.position - (Vector3.up * ((crouchHeight / 2) - info.radius));
			bool isBlocked = Physics.SphereCast(bottom, info.radius, Vector3.up, out var hit, info.height - info.radius, collisionLayer);
			if (isBlocked) return false; // If we have something above us, do nothing and return
			movement.controller.height = info.height;
			ChangeStatus(Status.walking);
			return true;
		}

		public bool HasObjectInfront(float dis, LayerMask layer)
		{
			Vector3 top = transform.position + (transform.forward * 0.25f);
			Vector3 bottom = top - (transform.up * info.halfheight);

			return (Physics.CapsuleCastAll(top, bottom, 0.25f, transform.forward, dis, layer).Length >= 1);
		}

		public bool HasWallToSide(int dir, LayerMask layer)
		{
			// Check for ladder in front of player
			Vector3 top = transform.position + (transform.right * 0.25f * dir);
			Vector3 bottom = top - (transform.up * info.radius);
			top += (transform.up * info.radius);

			return (Physics.CapsuleCastAll(top, bottom, 0.25f, transform.right * dir, 0.05f, layer).Length >= 1);
		}
	}

	public class PlayerInfo
	{
		public float rayDistance;
		public float radius;
		public float height;
		public float halfradius;
		public float halfheight;

		public PlayerInfo(float r, float h)
		{
			radius = r; height = h;
			halfradius = r / 2f; halfheight = h / 2f;
			rayDistance = halfheight + radius + .175f;
		}
	}
	public class IKData
	{
		public Vector3 handPos;
		public Vector3 handEulerAngles;

		public Vector3 armElbowPos;
		public Vector3 armLocalPos;

		public IKData()
		{
			handPos = Vector3.zero;
			handEulerAngles = Vector3.zero;
			armElbowPos = Vector3.zero;
			armLocalPos = Vector3.zero;
		}

		public TransformData HandData()
		{
			TransformData data = new TransformData();
			data.position = handPos;
			data.eulerAngles = handEulerAngles;
			return data;
		}
	}
}
