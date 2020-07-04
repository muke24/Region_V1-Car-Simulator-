#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
	public static bool inCar = false;

	public GameObject inCarPlayer;

	public GameObject door1;
	public GameObject door2 = null;
	public GameObject door3 = null;
	public GameObject door4 = null;

	public CurrentCar _currentCar;

	public bool floating = false;
	private Rigidbody rigid;

	public float depthBeforeSubmerged = 1f;
	public float displacementAmount = 2f;
	public float dragForce = 1f;

	private void Awake()
	{
		_currentCar = GameObject.FindGameObjectWithTag("Manager").GetComponent<CurrentCar>();

		inCarPlayer.SetActive(false);

		rigid = GetComponent<Rigidbody>();
	}

	public void ToggleDrivingPlayer()
	{
		if (inCar)
		{
			_currentCar.currentCar.transform.Find("Seats").Find("Seat1").Find("DrivingPlayer").gameObject.SetActive(true);
		}
		if (!inCar)
		{
			_currentCar.currentCar.transform.Find("Seats").Find("Seat1").Find("DrivingPlayer").gameObject.SetActive(false);
		}
	}

	private void FixedUpdate()
	{
		if (floating)
		{
			// Makes the car float up and down when its reached the water submerge depth
			if (transform.position.y > 0)
			{
				float displacementMultiplier = Mathf.Clamp01((transform.position.y) / depthBeforeSubmerged) * displacementAmount;

				rigid.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);

			}
			if (transform.position.y <= 0)
			{
				float displacementMultiplier = Mathf.Clamp01((-transform.position.y) / depthBeforeSubmerged) * displacementAmount;

				rigid.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);

			}
		}
	}

	public void Float()
	{
		floating = true;
	}

	public void StopFloat()
	{
		floating = false;
	}
}
// This code is written by Peter Thompson
#endregion