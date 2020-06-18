#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
	public bool inCar;
	public float interactDist = 3f;
	public float intTimer = 0.25f;
	public float intDelay = 0.25f;

	public GameObject carCanv;
	public GameObject fpsCanv;
	public GameObject pauseCanv;

	public CarFind carFind;
	public GameObject car;
	public GameObject player;

	public GameObject playerCam;
	public GameObject playerModel;
	public MouseLook mouse;
	public CharacterController charC;
	public PlayerMovement pMove;
	public CapsuleCollider cCol;

	public Text intText;
	public Camera carCam;

	public CarController cc;
	public CarInputManager im;

	public GameObject door1;

	// Start is called before the first frame update
	void Awake()
	{
				
	}

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").gameObject;
		carFind = GameObject.FindGameObjectWithTag("Player").GetComponent<CarFind>();
		playerModel = GameObject.FindGameObjectWithTag("Player").transform.Find("Model").gameObject;
		mouse = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>();
		charC = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
		pMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		cCol = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
		Car.inCar = false;
		carCam.enabled = false;

		if (player != null)
		{
			playerEnable();
		}		
	}

	// Update is called once per frame
	void Update()
	{
		inCar = Car.inCar;

		if (intTimer >= 0)
		{
			intTimer -= Time.deltaTime;
		}

		if (Car.inCar)
		{
			car.GetComponent<CarInputManager>().enabled = true;
			car.GetComponent<CarController>().enabled = true;

			if (Input.GetButtonDown("Interact") && intTimer <= 0)
			{
				intTimer = intDelay;
				Car.inCar = false;
				player.transform.position = door1.transform.position;
				//player.transform.rotation = car.transform.rotation;
				//new Vector3(car.transform.localPosition.x - 2, car.transform.localPosition.y, car.transform.localPosition.z);
			}
			if (player != null)
			{
				playerDisable();
			}			

			im.enabled = true;
			
			carCam.enabled = true;

			carCanv.SetActive(true);
			fpsCanv.SetActive(false);

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if (!Car.inCar)
		{
			car = carFind.closestCar.gameObject;
			door1 = carFind.closestCar.door1;

			car.GetComponent<CarInputManager>().enabled = false;
			car.GetComponent<CarController>().enabled = false;

			carCam.enabled = false;

			if (player != null)
			{
				playerEnable();
			}			
						
			im.enabled = false;
			
			if (pauseCanv.activeSelf)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			if (!pauseCanv.activeSelf)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}

			carCanv.SetActive(false);
			fpsCanv.SetActive(true);
		}

		if (interactDist >= Vector3.Distance(car.transform.position, player.transform.position) && !Car.inCar)
		{
			intText.gameObject.SetActive(true);
			intText.text = "Press " + "E" + " to get into car";

			if (Input.GetButtonDown("Interact") && intTimer <= 0)
			{
				intTimer = intDelay;
				Car.inCar = true;
			}
		}

		if (interactDist <= Vector3.Distance(car.transform.position, player.transform.position) && !Car.inCar)
		{
			intText.gameObject.SetActive(false);
		}

	}
	void OnDrawGizmos()
	{
		// Draw a yellow sphere at the transform's position
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(car.transform.position, interactDist);
	}

	void playerEnable()
	{
		playerCam.SetActive(true);
		playerModel.SetActive(true);
		mouse.enabled = true;
		charC.enabled = true;
		pMove.enabled = true;
		cCol.enabled = true;
	}
	void playerDisable()
	{
		playerCam.SetActive(false);
		playerModel.SetActive(false);
		mouse.enabled = false;
		charC.enabled = false;
		pMove.enabled = false;
		cCol.enabled = false;
	}
}
// This code is written by Peter Thompson
#endregion