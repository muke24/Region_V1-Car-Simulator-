using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointLock : MonoBehaviour
{
	public JointLock jl;

	public float closedRotXlow;
	public float closedRotXhigh;
	public float closedRotYlow = -0.55f;
	public float closedRotYhigh = -0.45f;
	public float closedRotZlow;
	public float closedRotZhigh;

	public int partNumber;

	public bool lockState = true;
	public Rigidbody rb;
	public Quaternion lockRot = new Quaternion(0, 0, 0, 0);
	public Vector3 lockPos = new Vector3(0, 0, 0);
	

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		jl = GetComponent<JointLock>();
		lockState = true;		
	}
	private void LateUpdate()
	{
		if (lockState)
		{
			rb.gameObject.transform.localRotation = lockRot;
			rb.gameObject.transform.localPosition = lockPos;
		}
		if (transform.localPosition.x > 1f)
		{
			jl.enabled = false;
		}
		if (transform.localPosition.y > 1f)
		{
			jl.enabled = false;
		}
		if (transform.localPosition.z > 1f)
		{
			jl.enabled = false;
		}
		if (transform.localPosition.x < -1f)
		{
			jl.enabled = false;
		}
		if (transform.localPosition.y < -1f)
		{
			jl.enabled = false;
		}
		if (transform.localPosition.z < -1f)
		{
			jl.enabled = false;
		}
	}

	private void FixedUpdate()
	{
		if (partNumber == 1)
		{
			if (rb.transform.localRotation.y >= closedRotYlow && rb.transform.localRotation.y <= closedRotYhigh)
			{
				lockState = true;
			}
		}
		
	}

	private void OnCollisionEnter(Collision collision)
	{
		lockState = false;
	}
}


