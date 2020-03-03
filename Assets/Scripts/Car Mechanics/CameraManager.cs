using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public GameObject focus;
	public float distance = 5f;
	public float height = 2f;
	public float dampening = 1f;
	public float h2 = 0f;
	public float d2 = 0f;
	public float l = 0f;

	private int camMode = 0;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			camMode = (camMode + 1) % 2;
		}

		if (camMode == 1)
		{
			transform.position = focus.transform.position + focus.transform.TransformDirection(new Vector3(l, h2, d2));
			transform.rotation = focus.transform.rotation;
			Camera.main.fieldOfView = 80f;
		}
	}

	void FixedUpdate()
	{
		if (camMode == 0)
		{
			transform.position = Vector3.Lerp(transform.position, focus.transform.position + focus.transform.TransformDirection(new Vector3(0f, height, -distance)), dampening * Time.deltaTime);
			transform.LookAt(focus.transform);
			Camera.main.fieldOfView = 60f;
		}
	}
}
