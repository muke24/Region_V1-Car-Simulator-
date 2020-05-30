#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputManager : MonoBehaviour
{
	public float throttle;
	public float steer;
	public bool brake;
	public bool lightToggle;

	public Rigidbody rb;

	public CarFind findCar;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if (Car.inCar)
		{
			throttle = Input.GetAxis("Vertical");
			steer = Input.GetAxis("Horizontal");
			brake = Input.GetKey(KeyCode.Space);
			lightToggle = Input.GetKeyDown(KeyCode.L);
		}

		if (!Car.inCar)
		{
			throttle = 0f;
			steer = 0f;
			brake = true;
		}

		#region BrakeFix
		//if (transform.InverseTransformVector(rb.velocity).z > 1f)
		//
		//	brake = false;
		//	if (Input.GetKey(KeyCode.S))
		//	{
		//		brake = true;
		//	}
		//	if (Input.GetKey(KeyCode.DownArrow))
		//	{
		//		brake = true;
		//	}
		//
		//
		//
		//if (transform.InverseTransformVector(rb.velocity).z < -1f)
		//
		//	brake = false;
		//	if (Input.GetKey(KeyCode.W))
		//	{
		//		brake = true;
		//	}
		//	if (Input.GetKey(KeyCode.UpArrow))
		//	{
		//		brake = true;
		//	}
		//
		#endregion

	}
}
// This code is written by Peter Thompson
#endregion