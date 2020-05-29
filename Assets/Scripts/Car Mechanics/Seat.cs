using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
	[SerializeField]
	private Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	private void FixedUpdate()
	{
		rb.rotation = transform.localRotation;
	}
}
