using UnityEngine;
using Mirror;
using System.Collections;
using System;

public class PlaneController : MonoBehaviour
{
	public bool jumping = false;
	[SerializeField]
	private float speed = 50f;
	private bool doorOpened = false;
	private Vector3 camRotation;
	private float mouseSensitivity = 10000f;
	[SerializeField]
	private GameObject planeCamera;

	private void Awake()
	{
		foreach (Collider collider1 in FindObjectsOfType<Collider>())
		{
			if (collider1.tag != "PlaneTrigger" || collider1.tag != "Plane")
			{
				foreach (Collider collider2 in GetComponentsInChildren<Collider>())
				{
					Physics.IgnoreCollision(collider1, collider2);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "PlaneTrigger")
		{

		}
	}

	// Update is called once per frame
	void Update()
	{
		camRotation.x = -Input.GetAxis("Mouse Y") * Time.deltaTime;
		camRotation.y = Input.GetAxis("Mouse X") * Time.deltaTime;

		planeCamera.transform.Rotate(camRotation.x * mouseSensitivity, camRotation.y * mouseSensitivity, 0);

		//var c = Camera.main.transform;
		//c.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);
		//c.Rotate(-Input.GetAxis("Mouse Y") * mouseSensitivity, 0, 0);
		//c.Rotate(0, 0, -Input.GetAxis("QandE") * 90 * Time.deltaTime);

		MovePlane();

		#region Old
		//transform.rotation = Quaternion.AngleAxis(camRotation.y * Time.deltaTime * sensitivity, Vector3.right);
		//transform.rotation = Quaternion.AngleAxis(camRotation.x * Time.deltaTime * sensitivity, Vector3.up) * transform.rotation;

		//Quaternion xQuaternion = Quaternion.AngleAxis(camRotation.x, Vector3.up);
		//Quaternion yQuaternion = Quaternion.AngleAxis(camRotation.y, Vector3.right);
		//transform.rotation = transform.rotation * xQuaternion * yQuaternion;
		#endregion


	}

	void MovePlane()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	public void ChangeToPlayerCam()
	{

	}
}
