using UnityEngine;

public class PlaneCamMovement : MonoBehaviour
{
	public bool jumping = false;

	private Vector3 camRotation;

	private float sensitivity = 10000f;

	// Update is called once per frame
	void Update()
	{
		camRotation.x = Input.GetAxis("Mouse Y");
		camRotation.y = Input.GetAxis("Mouse X");
		camRotation.z = 0;

		transform.Rotate(camRotation * sensitivity * Time.deltaTime, Space.Self);
		

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
