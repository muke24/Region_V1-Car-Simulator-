using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraManager : MonoBehaviour
{
	public MouseLook mouseLook;

	public CarFind carFind;

	public CurrentCar currentCar;
	public Rigidbody rb = null;
	public GameObject pause;
	public float distance = 5f;
	public float height = 2f;
	public float dampening = 12.5f;
	public float h2 = 0f;
	public float d2 = 0f;
	public float l = 0f;
	public float objDistance = 0f;
	public float maxDistance = 8f;

	public float fpsCarTimer = 0.1f;
	public static int camMode = 0;

	#region MouseLook

	public float minimumX = -360F;
	public float maximumX = 360F;

	private float rotationX = 0F;

	Quaternion originalRotation;

	#endregion

	[SerializeField]
	private GameObject car = null;
	[SerializeField]
	private AudioListener aL = null;

	private void Awake()
	{
		
	}

	void Start()
	{
		
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}


		if (angle > 360f)
		{
			angle -= 360f;
		}

		return Mathf.Clamp(angle, min, max);
	}

	private void Update()
	{
		

		if (Car.inCar)
		{
			car = currentCar.currentCar;
			aL.enabled = true;
			//originalRotation = rb.transform.rotation;
			rb = currentCar.currentCar.GetComponent<Rigidbody>();

			if (rb == null)
			{
				Debug.LogWarning("No Rigidbody is assigned");
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				camMode = (camMode + 1) % 2;
			}

			if (camMode == 0)
			{
				if (fpsCarTimer <= 0f)
				{
					fpsCarTimer = 0.1f;
				}
			}
			if (camMode == 1)
			{
				transform.position = car.transform.position + car.transform.TransformDirection(new Vector3(l, h2, d2));

				if (!pause.activeSelf)
				{
					Cursor.lockState = CursorLockMode.Locked;
				}
				if (pause.activeSelf)
				{
					Cursor.lockState = CursorLockMode.None;
				}

				if (!Input.GetButton("Aim"))
				{

					transform.rotation = car.transform.rotation;
					Camera.main.fieldOfView = 80f;
				}

				if (Input.GetButton("Aim"))
				{
					rotationX += Input.GetAxis("Mouse X") * 7;
					rotationX = ClampAngle(rotationX, minimumX, maximumX);
					Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
					transform.localRotation = originalRotation * xQuaternion;
				}
			}			
		}

		if (!Car.inCar)
		{
			car = null;
			aL.enabled = false;
		}
	}

	void FixedUpdate()
	{
		if (Car.inCar)
		{
			if (Input.GetKey(KeyCode.I))
			{
				distance = -5f;
				dampening = 40f;
			}
			if (!Input.GetKey(KeyCode.I))
			{
				distance = 4.5f;
				dampening = 12.5f;
			}

			if (camMode == 0)
			{
				objDistance = Vector3.Distance(car.transform.position, transform.position);
				transform.LookAt(car.transform);
				GetComponent<Camera>().fieldOfView = 60f;
				transform.position = Vector3.Lerp(transform.position, car.transform.position + car.transform.TransformDirection(new Vector3(0f, height, -distance)), dampening * Time.deltaTime);

				if (objDistance >= maxDistance)
				{
					dampening = 13f;
					//transform.position = 5.5f;
				}
				else
				{
					dampening = 12.5f;
				}
			}

			if (camMode == 1)
			{
				transform.position = car.transform.position + car.transform.TransformDirection(new Vector3(l, h2, d2));

				GetComponent<Camera>().fieldOfView = 80f;

				if (!pause.activeSelf)
				{
					Cursor.lockState = CursorLockMode.Locked;
				}
				if (pause.activeSelf)
				{
					Cursor.lockState = CursorLockMode.None;
				}

				if (Input.GetButtonDown("Aim") || Input.GetButtonUp("Aim"))
				{					
					originalRotation = rb.transform.rotation;
					rotationX = 0;
				}

				if (Input.GetButton("Aim"))
				{
					rotationX += Input.GetAxis("Mouse X") * mouseLook.sensitivityX;
					rotationX = ClampAngle(rotationX, minimumX, maximumX);

					Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
					transform.localRotation = originalRotation * xQuaternion;
					originalRotation = rb.transform.rotation;
				}
			}
		}
	}
}
