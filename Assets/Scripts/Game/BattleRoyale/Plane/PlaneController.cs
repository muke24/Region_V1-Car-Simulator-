using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System;

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
	/* Sets default enum to mouseXandY (MouseXandY rotates the object this script is attached to both the X and Y axis, 
	   which having MouseX would only rotate it on the X axis and same goes with the MouseY) */
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 100f;
	public float sensitivityY = 100f;
	public float minimumX = -360f;  // Minimum X rotation
	public float maximumX = 360f;   // Maximum X rotation
	public float minimumY = -60f;   // Minimum Y rotation
	public float maximumY = 60f;    // Maximum Y rotation
	private float rotationX = 0f;
	private float rotationY = 0f;
	Quaternion originalRotation;
	// Enum holding three values - mouse X and mouse Y, mouse X, mouse Y
	public enum RotationAxes
	{
		MouseXAndY = 0
	}
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
		originalRotation = transform.localRotation;
	}

	// Update is called once per frame
	void Update()
	{
		MouseLook();

		MovePlane();
	}

	void MouseLook()
	{
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
			planeCamera.transform.localRotation = originalRotation * xQuaternion * yQuaternion;
		}
	}

	void MovePlane()
	{
		transform.Translate(Vector3.forward * planeSpeed * Time.deltaTime);
	}

	public void ChangeToPlayerCam()
	{

	}
}
