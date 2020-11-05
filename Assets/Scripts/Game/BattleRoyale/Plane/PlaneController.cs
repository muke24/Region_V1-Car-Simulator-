using UnityEngine;

public class PlaneController : MonoBehaviour
{
	public bool jumping = false;
	private bool doorOpened = false;
	private Vector3 camRotation;
	private float mouseSensitivity = 10000f;	

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
		camRotation.x = Input.GetAxis("Mouse Y");
		camRotation.y = Input.GetAxis("Mouse X");
		camRotation.z = 0;

		transform.Rotate(camRotation * mouseSensitivity * Time.deltaTime, Space.Self);
		

		//transform.rotation = Quaternion.AngleAxis(camRotation.y * Time.deltaTime * sensitivity, Vector3.right);
		//transform.rotation = Quaternion.AngleAxis(camRotation.x * Time.deltaTime * sensitivity, Vector3.up) * transform.rotation;

		//Quaternion xQuaternion = Quaternion.AngleAxis(camRotation.x, Vector3.up);
		//Quaternion yQuaternion = Quaternion.AngleAxis(camRotation.y, Vector3.right);
		//transform.rotation = transform.rotation * xQuaternion * yQuaternion;
	}

	public void ChangeToPlayerCam()
	{

	}
}
