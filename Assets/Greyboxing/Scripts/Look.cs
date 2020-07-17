using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
	private float x;
	private float y;

	[SerializeField]
	[Range(1, 100)]
	private float sensitivity = 75f;
	[SerializeField]
	private Transform cam = null;
	[SerializeField]
	private Transform player = null;

	private void OnValidate()
	{
		cam = GetComponentInChildren<Camera>().transform;
		player = GetComponent<Transform>();
	}

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update()
	{
		XLook();
		YLook();
	}

	void XLook()
	{
		x += Input.GetAxis("Mouse X") * sensitivity;
		x = ClampAngle(x, -360, 360);
		player.localRotation = Quaternion.Euler(0, x, 0);
	}

	void YLook()
	{
		y -= Input.GetAxis("Mouse Y") * sensitivity;
		y = ClampAngle(y, -90, 90);
		cam.localRotation = Quaternion.Euler(y, 0, 0);
	}

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
}