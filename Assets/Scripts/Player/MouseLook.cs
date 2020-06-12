#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public GameObject paused;

	// Enum holding three values - mouse X and mouse Y, mouse X, mouse Y
	public enum RotationAxes { MouseXAndY = 0 /* MouseXAndY isnt needed but I kept it just in case */, MouseX = 1, MouseY = 2 }

	// Sets default enum to mouseXandY (MouseXandY rotates the object this script is attached to both the X and Y axis, 
	// which having MouseX would only rotate it on the X axis and same goes with the MouseY)
	public RotationAxes axes = RotationAxes.MouseXAndY;

	public float sensitivityX = 100f;
	public float sensitivityY = 100f;
	public float sensitivityXads = 50f;
	public float sensitivityYads = 50f;
	public float minimumX = -360f;	// Minimum X rotation
	public float maximumX = 360f;	// Maximum X rotation
	public float minimumY = -60f;   // Minimum Y rotation
	public float maximumY = 60f;    // Maximum Y rotation
	public GameObject sniper;
	public Animator anim;

	private float rotationX = 0f;
	private float rotationY = 0f;

	Quaternion originalRotation;

	void Start()
	{
		// Make the rigidbody not change rotation so it doesnt stop the player from rotating
		if (GetComponent<Rigidbody>())
		{
			// Freeze rigidbody rotation
			GetComponent<Rigidbody>().freezeRotation = true;
		}
		// Sets the player rotation to the original rotation at the start
		originalRotation = transform.localRotation;
		paused = Pause.pause;
	}

	void Update()
	{
		// IF the game isn't paused
		if (!paused.activeSelf)
		{
			// If MouseXandY is selected in inspector
			if (axes == RotationAxes.MouseXAndY)
			{
				// Read the mouse input axis
				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				// Maximum rotation the camera can rotate
				rotationX = ClampAngle(rotationX, minimumX, maximumX);
				rotationY = ClampAngle(rotationY, minimumY, maximumY);
				Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
				Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
				transform.localRotation = originalRotation * xQuaternion * yQuaternion;
			}
			else if (axes == RotationAxes.MouseX)
			{
				// If set to mouse X then update on mouse X values
				MouseX();
				MouseXads();
			}
			else
			{
				// If set to mouse Y then update on mouse Y values
				MouseY();
				MouseYads();
			}
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		// If the angle exceeds -360 degrees then set it to +360 degrees
		if (angle < -360f)
		{
			angle += 360f;
		}
		// If the angle exceeds +360 degrees then set it to -360 degrees
		if (angle > 360f)
		{
			angle -= 360f;
		}

		return Mathf.Clamp(angle, min, max);
	}

	// Only applies to the mouse X axis when not aiming
	public void MouseX() 
	{
		// If sniper isnt aiming and sniper is active then set the mouse sensitivity to the normal set X sensitivity
		if (!anim.GetBool("Aim") && sniper)
		{
			// X rotation plus the mouse X input times the X sensitivity
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			// Sets the maximum rotation X angle
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;
		}

		// If the sniper is in the reload animation and active then set the mouse sensitivity to the normal set X sensitivity
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("SniperReload") && sniper)
		{
			// X rotation plus the mouse X input times the X sensitivity
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			// Sets the maximum rotation X angle
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;
		}

		// If the sniper isnt active then set the sensitivity to the normal set X sensitivity
		if (!sniper)
		{
			// X rotation plus the mouse X input times the X sensitivity
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			// Sets the maximum rotation X angle
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;
		}
	}

	// Only applies to the mouse Y axis when not aiming
	public void MouseY()
	{
		// If sniper isnt aiming and sniper is active then set the mouse sensitivity to the normal set Y sensitivity
		if (!anim.GetBool("Aim") && sniper)
		{
			// Y rotation plus the mouse Y input times the Y sensitivity
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			// Sets the maximum rotation Y angle
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
			Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
			transform.localRotation = originalRotation * yQuaternion;

		}

		// If the sniper is in the reload animation and active then set the mouse sensitivity to the normal set Y sensitivity
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("SniperReload") && sniper)
		{
			// Y rotation plus the mouse Y input times the Y sensitivity
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			// Sets the maximum rotation Y angle
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
			Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
			transform.localRotation = originalRotation * yQuaternion;
		}

		// If the sniper isnt active then set the sensitivity to the normal set Y sensitivity
		if (!sniper)
		{
			// Y rotation plus the mouse Y input times the Y sensitivity
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			// Sets the maximum rotation Y angle
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
			Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
			transform.localRotation = originalRotation * yQuaternion;
		}
	}

	// Only applies to the mouse X axis when aiming
	public void MouseXads()
	{
		// If sniper is aiming and sniper is active then set the mouse sensitivity to the normal set X aiming sensitivity
		if (anim.GetBool("Aim") && sniper)
		{
			// If the sniper isnt reloading
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
			{
				// X rotation plus the mouse X input times the X aiming sensitivity
				rotationX += Input.GetAxis("Mouse X") * sensitivityXads;
				// Sets the maximum rotation X angle
				rotationX = ClampAngle(rotationX, minimumX, maximumX);
				Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
				transform.localRotation = originalRotation * xQuaternion;
			}
		}
	}

	// Only applies to the mouse Y axis when aiming
	public void MouseYads()
	{
		// If sniper is aiming and sniper is active then set the mouse sensitivity to the normal set Y aiming sensitivity
		if (anim.GetBool("Aim") && sniper)
		{
			// If the sniper isnt reloading
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
			{
				// Y rotation plus the mouse Y input times the Y aiming sensitivity
				rotationY += Input.GetAxis("Mouse Y") * sensitivityYads;
				// Sets the maximum rotation Y angle
				rotationY = ClampAngle(rotationY, minimumY, maximumY);
				Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
				transform.localRotation = originalRotation * yQuaternion;
			}
		}
	}
}
// This code is written by Peter Thompson
#endregion