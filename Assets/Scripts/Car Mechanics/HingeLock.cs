using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeLock : MonoBehaviour
{
	public bool isTouched = false;
	public Rigidbody rb;
	public Quaternion lockRot = new Quaternion(0, 0, 0, 0);
	public Vector3 lockPos = new Vector3(0, 0, 0);
	

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		isTouched = false;
		
	}
	private void Update()
	{
		if (!isTouched)
		{
			rb.gameObject.transform.localRotation = lockRot;
			rb.gameObject.transform.localPosition = lockPos;
		}		
	}

	private void OnCollisionEnter(Collision collision)
	{
		isTouched = true;
	}
}


