using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectateCamera : MonoBehaviour
{
	public float moveSpeed = 7.5f;
	public Vector3 moveDirection = Vector3.zero;

	public CharacterController controller;

	private void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		moveDirection = new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime, 0, Input.GetAxis("Vertical") * Time.deltaTime);
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= moveSpeed;
		transform.position += moveDirection;
	}
}
