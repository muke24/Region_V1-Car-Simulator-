using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public GameObject paused;

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }

	public RotationAxes axes = RotationAxes.MouseXAndY;

	public float sensitivityX = 100f;
	public float sensitivityY = 100f;
	public float sensitivityXads = 50f;
	public float sensitivityYads = 50f;
	public float minimumX = -360f;
	public float maximumX = 360f;
	public float minimumY = -60f;
	public float maximumY = 60f;
	public GameObject sniper;
	public Animator anim;

	private float rotationX = 0f;
	private float rotationY = 0f;

	Quaternion originalRotation;

	void Start()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
		originalRotation = transform.localRotation;
	}

	void Update()
	{
		if (!paused.activeSelf)
		{
			if (axes == RotationAxes.MouseXAndY)
			{
				// Read the mouse input axis
				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationX = ClampAngle(rotationX, minimumX, maximumX);
				rotationY = ClampAngle(rotationY, minimumY, maximumY);
				Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
				Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
				transform.localRotation = originalRotation * xQuaternion * yQuaternion;
			}
			else if (axes == RotationAxes.MouseX)
			{
				MouseX();
				MouseXads();
			}
			else
			{
				MouseY();
				MouseYads();
			}
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}


		if (angle > 360f)
		{
			angle -= 360f;
		}

		return Mathf.Clamp(angle, min, max);
	}
	public void MouseX()
	{
		if (!anim.GetBool("Aim") && sniper)
		{
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;
		}
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
		{
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;
		}
		if (!sniper)
		{
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;
		}
	}
	public void MouseY()
	{
		if (!anim.GetBool("Aim") && sniper)
		{

			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
			Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
			transform.localRotation = originalRotation * yQuaternion;

		}
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
			Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
			transform.localRotation = originalRotation * yQuaternion;
		}
		if (!sniper)
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
			Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
			transform.localRotation = originalRotation * yQuaternion;
		}
	}
	public void MouseXads()
	{
		if (anim.GetBool("Aim") && sniper)
		{
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
			{
				rotationX += Input.GetAxis("Mouse X") * sensitivityXads;
				rotationX = ClampAngle(rotationX, minimumX, maximumX);
				Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
				transform.localRotation = originalRotation * xQuaternion;
			}
		}
	}
	public void MouseYads()
	{
		if (anim.GetBool("Aim") && sniper)
		{
			if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityYads;
				rotationY = ClampAngle(rotationY, minimumY, maximumY);
				Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
				transform.localRotation = originalRotation * yQuaternion;
			}
		}
	}
}