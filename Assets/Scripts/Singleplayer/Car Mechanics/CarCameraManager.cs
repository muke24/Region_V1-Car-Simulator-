#region This code is written by Peter Thompson
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

	public static int camMode = 0;

	#region MouseLook

	public float minimumX = -360f;
	public float maximumX = 360f;

	private float rotationX = 0f;
	private Quaternion originalRotation;

	#endregion

	[SerializeField]
	private GameObject car = null;
	[SerializeField]
	private AudioListener aL = null;

	private void Start()
	{
		mouseLook = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>();
		carFind = GameObject.FindGameObjectWithTag("Player").GetComponent<CarFind>();
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		// Clamps the angle. So if the car camera is in carMode 1 (meaning you are driving the car in first person mode), 
		// then the car cam cannot go past the maximum clamp angle
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
		// If you are in a car;
		if (Car.inCar)
		{
			// Sets the current car that you are driving to the last closest car that you were in before you got in the car.
			car = carFind.closestCar.gameObject;
			// Enables the carCam Audio Listener
			aL.enabled = true;
			//originalRotation = rb.transform.rotation;
			rb = currentCar.currentCar.GetComponent<Rigidbody>();

			if (Input.GetKeyDown(KeyCode.C))
			{
				// Changes the car cameras mode to the opposite mode it is in, so if the car cam mode is in fps mode it 
				// will change to third person mode and vice versa
				camMode = (camMode + 1) % 2;
			}

			// If cam is in fps mode
			if (camMode == 1)
			{
				// Sets the carCam to inside the car
				transform.position = car.transform.position + car.transform.TransformDirection(new Vector3(l, h2, d2));

				// If not paused
				if (!pause.activeSelf)
				{
					// Lock cursor
					if (Cursor.lockState != CursorLockMode.Locked)
					{
						Cursor.lockState = CursorLockMode.Locked;
						Cursor.visible = false;
					}					
				}

				// If paused
				if (pause.activeSelf)
				{
					if (Cursor.lockState != CursorLockMode.None)
					{
						// Unlock cursor
						Cursor.lockState = CursorLockMode.None;
						Cursor.visible = true;
					}					
				}
			}
		}

		// If you are not in a car;
		if (!Car.inCar)
		{
			// Sets the current car to null so the carCam doesn't try to set its position to multiple cars position if the player gets into another car
			car = null;
			// Disables the carCam Audio Listener to ensure you aren't hearing sound from both the player's fps camera Audio Listener and CarCam Audio Listener
			aL.enabled = false;
		}
	}

	// FixedUpdate each fixed update time rate occurs. Fixed update is set to 30 frames but can be changed in the editor
	void FixedUpdate()
	{
		// If you are in a car
		if (Car.inCar)
		{
			// If camMode is 
			if (camMode == 0)
			{
				// Sets the objDistance float to the distance inbetween the camera and the car
				objDistance = Vector3.Distance(car.transform.position, transform.position);
				// Set the carCamera rotation to look at the car
				transform.LookAt(car.transform);
				// If in third person then reduce the car cameras field of view to 60 degrees
				GetComponent<Camera>().fieldOfView = 60f;
				// Makes the camera give the gradually change its position due to the car speed. 
				// eg. If the car suddenly accelerates the camera will be further out
				transform.position = Vector3.Lerp(transform.position, car.transform.position + car.transform.TransformDirection(new Vector3(0f, height, -distance)), dampening * Time.deltaTime);

				// Changes the dampening to a higher amount to reduce the effect of the lerp when the distance from the car is over the max distance float
				if (objDistance >= maxDistance)
				{
					dampening = 13f;
				}
				// Normal dampening float when below max distance for the car and the camera
				else
				{
					dampening = 12.5f;
				}
			}

			// If not paused
			if (!pause.activeSelf)
			{
				// "I" key pressed
				if (Input.GetKey(KeyCode.I))
				{
					// If you have the key "I" pressed, the car camera will view behind the car
					distance = -5f;
					dampening = 40f;
				}
				// "I" key not pressed
				if (!Input.GetKey(KeyCode.I))
				{
					// If you don't have the key "I" pressed, the car camera will view forwards of the car
					distance = 4.5f;
					dampening = 12.5f;
				}

				// If in fps car mode
				if (camMode == 1)
				{
					// Sets the cameras fov to 80 degress when in first person mode
					GetComponent<Camera>().fieldOfView = 80f;

					if (!pause.activeSelf)
					{
						// Lock cursor
						Cursor.lockState = CursorLockMode.Locked;
						Cursor.visible = false;
					}
					if (pause.activeSelf)
					{
						// Unlock cursor
						Cursor.lockState = CursorLockMode.None;
						Cursor.visible = true;
					}

					if (!Input.GetButton("Aim"))
					{
						/* Lock the camera rotation to the car rigidbody rotation (Originally had it in Update and had it locked the the cars rotation, 
						 * but tried a few stuff out and found that this way gives the camera a nice look when turning around corners, 
						 * as it pretty much doesnt fully lock to the cars rotation, but it keeps the camera a little behind time and has a nice look to it) */					
						Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
						transform.localRotation = originalRotation * xQuaternion;
						originalRotation = rb.transform.rotation;
					}

					if (Input.GetButtonDown("Aim") || Input.GetButtonUp("Aim"))
					{
						// Sets the fps free look rotation to the front of the car when the right click mouse button has been just pressed down or released up
						originalRotation = rb.transform.rotation;
						rotationX = 0;
					}

					if (Input.GetButton("Aim"))
					{
						// Allows rotating the car cam to look around when in fps mode at the current selected sensitivity set in settings
						rotationX += Input.GetAxis("Mouse X") * mouseLook.sensitivityX;
						// Sets the maximum rotating angle
						rotationX = ClampAngle(rotationX, minimumX, maximumX);
						// Sets the rotation
						Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
						transform.localRotation = originalRotation * xQuaternion;
						originalRotation = rb.transform.rotation;
					}
				}
			}
		}
	}
}
// This code is written by Peter Thompson
#endregion