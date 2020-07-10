#region This code is written by Peter Thompson
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
	public float interactDist = 3f;
	public float intTimer = 0.25f;
	public float intDelay = 0.25f;

	public GameObject carCanv;
	public GameObject fpsCanv;
	public GameObject pauseCanv;

	private CarFind carFind;
	public GameObject car;
	private GameObject player;

	private GameObject playerCam;
	private GameObject playerModel;
	private MouseLook mouse;
	private CharacterController charC;
	private PlayerMovement pMove;
	private PlayerNetworkMovement pMoveNet;
	private CapsuleCollider cCol;

	public Text intText;
	public Camera carCam;

	public CarController cc;
	public CarInputManager im;
	public GameObject door1;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerCam = player.transform.Find("Camera").gameObject;
		carFind = player.GetComponent<CarFind>();
		playerModel = player.transform.Find("Model").gameObject;
		mouse = player.GetComponent<MouseLook>();
		charC = player.GetComponent<CharacterController>();
		pMove = player.GetComponent<PlayerMovement>();
		pMoveNet = player.GetComponent<PlayerNetworkMovement>();
		cCol = player.GetComponent<CapsuleCollider>();
		Car.inCar = false;
		carCam.enabled = false;

		if (player != null)
		{
			playerEnable();
		}
	}

	private void Update()
	{
		if (intTimer >= 0)
		{
			intTimer -= Time.deltaTime;
		}

		if (CarFind.inCarTrigger)
		{
			if (Car.inCar)
			{
				if (playerCam.activeSelf)
				{
					playerDisable();
				}

				car.GetComponent<CarInputManager>().enabled = true;
				car.GetComponent<CarController>().enabled = true;

				im.enabled = true;

				carCam.enabled = true;

				carCanv.SetActive(true);
				fpsCanv.SetActive(false);
			}

			if (!Car.inCar)
			{
				if (!playerCam.activeSelf)
				{
					playerEnable();
				}

				if (carFind.closestCar == null)
				{
					car = carFind.closestCar.gameObject;
				}

				door1 = carFind.closestCar.door1;

				car.GetComponent<CarInputManager>().enabled = false;
				car.GetComponent<CarController>().enabled = false;

				carCam.enabled = false;

				im.enabled = false;

				carCanv.SetActive(false);
				fpsCanv.SetActive(true);

				//////

				intText.gameObject.SetActive(true);
				intText.text = "Press " + "E" + " to get into car";
			}
		}

		if (!CarFind.inCarTrigger)
		{
			if (!Car.inCar)
			{
				intText.gameObject.SetActive(false);
			}
		}

		if (Car.inCar)
		{
			if (Input.GetButtonDown("Interact") && intTimer <= 0)
			{
				intTimer = intDelay;
				Car.inCar = false;
				player.transform.position = door1.transform.position;
				player.GetComponent<CarFind>().closestCar.ToggleDrivingPlayer();
			}
		}
		else
		{
			if (CarFind.inCarTrigger)
			{
				if (Input.GetButtonDown("Interact") && intTimer <= 0)
				{
					intTimer = intDelay;
					Car.inCar = true;
					GameObject.FindGameObjectWithTag("Player").GetComponent<CarFind>().closestCar.ToggleDrivingPlayer();
				}
			}			
		}
	}

	void OnDrawGizmos()
	{
		if (CarFind.inCarTrigger)
		{
			// Draw a yellow sphere at the transform's position
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(carFind.closestCar.transform.position, interactDist);
		}
	}

	void playerEnable()
	{
		playerCam.SetActive(true);
		playerModel.SetActive(true);
		mouse.enabled = true;
		charC.enabled = true;
		if (GameMode.singleplayer)
		{
			pMove.enabled = true;
		}
		if (GameMode.multiplayer)
		{
			pMoveNet.enabled = true;
		}
		cCol.enabled = true;
	}
	void playerDisable()
	{
		playerCam.SetActive(false);
		playerModel.SetActive(false);
		mouse.enabled = false;
		charC.enabled = false;
		if (GameMode.singleplayer)
		{
			pMove.enabled = false;
		}
		if (GameMode.multiplayer)
		{
			pMoveNet.enabled = false;
		}
		cCol.enabled = false;
	}
}
// This code is written by Peter Thompson
#endregion