#region This code is written by Peter Thompson
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using MyBox;

public class PlaneController : MonoBehaviour
{
	[Header("Plane Characteristics")]
	[SerializeField]
	private float planeSpeed = 75f;
	[SerializeField]
	private GameObject planeCamera;

	[Header("Gameplay")]
	public bool canJump = false;

	#region MouseLook	
	[Header("Mouse Look")]

	[SerializeField]
	private MouseLookType mouseLookType = MouseLookType.New;
	private enum MouseLookType { New, Old }

	[ConditionalField("mouseLookType", false, MouseLookType.Old)]
	[SerializeField]
	private MouseLookOld mouseLookOld;
	[ConditionalField("mouseLookType", false, MouseLookType.New)]
	[SerializeField]
	private MouseLookNew mouseLookNew;
	#endregion

	private static float ClampAngle(float angle, float min, float max)
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

	private void Awake()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		mouseLookOld.originalRotation = transform.localRotation;

		// Set target direction to the camera's initial orientation.
		mouseLookNew.targetDirection = planeCamera.transform.localRotation.eulerAngles;

		GetMainCamera();
	}

	// Update is called once per frame
	void Update()
	{
		if (mouseLookType == MouseLookType.New)
		{
			NewMouseLook();
		}
		else
		{
			OldMouseLook();
		}

		MovePlane();
	}

	void GetMainCamera()
	{
		Camera.main.transform.SetParent(planeCamera.transform);
		Camera.main.transform.localPosition = new Vector3(0, 0, -45);
		Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);
	}

	void OldMouseLook()
	{
		// Read the mouse input axis
		mouseLookOld.rotationX += Input.GetAxisRaw("Mouse X") * mouseLookOld.sensitivityX;
		mouseLookOld.rotationY += Input.GetAxisRaw("Mouse Y") * mouseLookOld.sensitivityY;
		// Maximum rotation the camera can rotate
		mouseLookOld.rotationX = ClampAngle(mouseLookOld.rotationX, mouseLookOld.minimumX, mouseLookOld.maximumX);
		mouseLookOld.rotationY = ClampAngle(mouseLookOld.rotationY, mouseLookOld.minimumY, mouseLookOld.maximumY);
		Quaternion xQuaternion = Quaternion.AngleAxis(mouseLookOld.rotationX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis(mouseLookOld.rotationY, -Vector3.right);
		planeCamera.transform.localRotation = mouseLookOld.originalRotation * xQuaternion * yQuaternion;
	}

	void NewMouseLook()
	{
		// Allow the script to clamp based on a desired target value.
		Quaternion targetOrientation = Quaternion.Euler(mouseLookNew.targetDirection);

		// Get raw mouse input for a cleaner reading on more sensitive mice.
		Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		// Scale input against the sensitivity setting and multiply that against the smoothing value.
		mouseDelta = Vector2.Scale(mouseDelta, new Vector2(mouseLookNew.sensitivity.x * 100 * mouseLookNew.smoothing.x, mouseLookNew.sensitivity.y * 100 * mouseLookNew.smoothing.y));

		// Interpolate mouse movement over time to apply smoothing delta.
		mouseLookNew._smoothMouse.x = Mathf.Lerp(mouseLookNew._smoothMouse.x, mouseDelta.x, 1f / mouseLookNew.smoothing.x);
		mouseLookNew._smoothMouse.y = Mathf.Lerp(mouseLookNew._smoothMouse.y, mouseDelta.y, 1f / mouseLookNew.smoothing.y);

		// Find the absolute mouse movement value from point zero.
		mouseLookNew._mouseAbsolute += mouseLookNew._smoothMouse;

		// Clamp and apply the local x value first, so as not to be affected by world transforms.
		mouseLookNew._mouseAbsolute.x = ClampAngle(mouseLookNew._mouseAbsolute.x, mouseLookNew.minimumAngle.x, mouseLookNew.maximumAngle.x);

		// Then clamp and apply the global y value.
		mouseLookNew._mouseAbsolute.y = ClampAngle(mouseLookNew._mouseAbsolute.y, mouseLookNew.minimumAngle.y, mouseLookNew.maximumAngle.y);

		Quaternion xQuaternion = Quaternion.AngleAxis(mouseLookNew._mouseAbsolute.x, targetOrientation * Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis(mouseLookNew._mouseAbsolute.y, targetOrientation * -Vector3.right);

		planeCamera.transform.localRotation = targetOrientation * xQuaternion * yQuaternion;
	}


	private void MovePlane()
	{
		transform.Translate(Vector3.forward * planeSpeed * Time.deltaTime);
	}

	public void AllowJump()
	{
		canJump = true;
	}
}

[Serializable]
class MouseLookOld
{
	#region MouseLookOld
	[Header("Variables")]
	public float sensitivityX = 100f;
	public float sensitivityY = 100f;
	public float minimumX = -360f;  // Minimum X rotation
	public float maximumX = 360f;   // Maximum X rotation
	public float minimumY = -60f;   // Minimum Y rotation
	public float maximumY = 60f;    // Maximum Y rotation
	[HideInInspector]
	public float rotationX = 0f;
	[HideInInspector]
	public float rotationY = 0f;
	[HideInInspector]
	public Quaternion originalRotation;
	#endregion
}

[Serializable]
class MouseLookNew
{
	[Header("Variables")]
	public Vector2 minimumAngle = new Vector2(-360, -75);  // Minimum and maximum X rotation
	public Vector2 maximumAngle = new Vector2(360, 75);   // Minimum and maximum Y rotation
	public Vector2 sensitivity = new Vector2(2, 2);
	public Vector2 smoothing = new Vector2(1, 1);
	[HideInInspector]
	public Vector2 targetDirection;
	[HideInInspector]
	public Vector2 _mouseAbsolute;
	[HideInInspector]
	public Vector2 _smoothMouse;
}
// This code is written by Peter Thompson
#endregion