#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointLock : MonoBehaviour
{
	CurrentCar _currentCar;

	public float hingeAngle = 0f;

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

	public static bool changeCarLayer = false;

	void Start()
	{
		_currentCar = GameObject.FindGameObjectWithTag("Manager").GetComponent<CurrentCar>();

		Physics.IgnoreLayerCollision(9, 9);
		rb = GetComponent<Rigidbody>();
		lockState = true;
	}

	private void Update()
	{
		if (Car.inCar && changeCarLayer)
		{
			Transform[] allChildren = _currentCar.currentCar.GetComponentsInChildren<Transform>();
			foreach (Transform child in allChildren)
			{
				child.gameObject.layer = 17;
			}
			Physics.IgnoreLayerCollision(17, 17);
			changeCarLayer = false;
		}

		if (!Car.inCar && changeCarLayer)
		{
			Transform[] allChildren = _currentCar.currentCar.GetComponentsInChildren<Transform>();
			foreach (Transform child in allChildren)
			{
				child.gameObject.layer = 9;
			}
			Physics.IgnoreLayerCollision(9, 9);
			changeCarLayer = false;
		}
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
			enabled = false;
		}
		if (transform.localPosition.y > 1f)
		{
			enabled = false;
		}
		if (transform.localPosition.z > 1f)
		{
			enabled = false;
		}
		if (transform.localPosition.x < -1f)
		{
			enabled = false;
		}
		if (transform.localPosition.y < -1f)
		{
			enabled = false;
		}
		if (transform.localPosition.z < -1f)
		{
			enabled = false;
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

			if (enabled)
			{
				if (GetComponent<HingeJoint>())
				{
					hingeAngle = GetComponent<HingeJoint>().angle;
				}
			}

			if (!lockState)
			{
				if (GetComponent<HingeJoint>())
				{
					if (GetComponent<HingeJoint>().angle < 0.5f)
					{
						lockState = true;
					}
				}				
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.root.tag == "Car")
		{
			lockState = true;
		}
		else
		{
			lockState = false;
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.transform.root.tag == "Car")
		{
			lockState = true;
		}
		else
		{
			lockState = false;
		}
	}
}
// This code is written by Peter Thompson
#endregion