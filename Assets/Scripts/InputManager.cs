using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public float throttle;
	public float steer;
	public bool brake;
	public bool lightToggle;

	public Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		throttle = Input.GetAxis("Vertical");
		steer = Input.GetAxis("Horizontal");

		brake = Input.GetKey(KeyCode.Space);

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
		
		lightToggle = Input.GetKeyDown(KeyCode.L);
	}
}
