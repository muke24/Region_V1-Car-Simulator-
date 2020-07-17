#region This code is written by Peter Thompson
using UnityEngine;

public class Look : MonoBehaviour
{
	public static bool isPaused = false;

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

	/// <summary>
	/// Rotates the player on the X axis
	/// </summary>
	void XLook()
	{
		if (!isPaused)
		{
			x += Input.GetAxis("Mouse X") * sensitivity * 300f * Time.deltaTime;
			x = ClampAngle(x, -360, 360);
			player.localRotation = Quaternion.Euler(0, x, 0);
		}
	}

	/// <summary>
	/// Rotates the players camera on the Y axis
	/// </summary>
	void YLook()
	{
		if (!isPaused)
		{
			y -= Input.GetAxis("Mouse Y") * sensitivity * 300f * Time.deltaTime;
			y = ClampAngle(y, -90, 90);
			cam.localRotation = Quaternion.Euler(y, 0, 0);
		}		
	}

	/// <summary>
	/// Limits the angle
	/// </summary>
	/// <param name="angle">Float angle to limit</param>
	/// <param name="min">Minimum angle</param>
	/// <param name="max">Maximum angle</param>
	/// <returns></returns>
	private static float ClampAngle(float angle, float min, float max)
	{
		//* If the angle exceeds -360 degrees then set it to +360 degrees
		if (angle < -360f)
		{
			angle += 360f;
		}
		//* If the angle exceeds +360 degrees then set it to -360 degrees
		if (angle > 360f)
		{
			angle -= 360f;
		}

		return Mathf.Clamp(angle, min, max);
	}
}
//* This code is written by Peter Thompson
#endregion