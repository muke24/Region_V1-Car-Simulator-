using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
	public bool inCar;
	public float interactDist = 3f;
	public float intTimer = 0.25f;
	public float intDelay = 0.25f;

	public GameObject carCanv;
	public GameObject fpsCanv;
	public GameObject pauseCanv;

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


	public List<DragObject> dragObjects;
	// Start is called before the first frame update
	void Start()
	{
		inCar = false;
		carCam.gameObject.SetActive(false);
		//player.gameObject.SetActive(true);
		playerEnable();

	}

	// Update is called once per frame
	void Update()
	{
		if (intTimer >= 0)
		{
			intTimer -= Time.deltaTime;
		}

		if (inCar)
		{
			if (Input.GetKeyDown(KeyCode.E) && intTimer <= 0)
			{
				intTimer = intDelay;
				inCar = false;
				player.transform.position = new Vector3(car.transform.position.x - 2, car.transform.position.y, car.transform.position.z);
			}

			playerDisable();
			//player.SetActive(false);
			im.enabled = true;
			foreach (DragObject drag in dragObjects)
			{
				drag.enabled = true;
			}
			carCam.gameObject.SetActive(true);
			//player.SetActive(false);

			carCanv.SetActive(true);
			fpsCanv.SetActive(false);

			Cursor.lockState = CursorLockMode.None;
		}
		if (!inCar)
		{
			carCam.gameObject.SetActive(false);
			playerEnable();
			//player.SetActive(true);
			im.enabled = false;
			foreach (DragObject drag in dragObjects)
			{
				drag.enabled = false;
			}
			if (pauseCanv.activeSelf)
			{
				Cursor.lockState = CursorLockMode.None;
			}
			if (!pauseCanv.activeSelf)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}

			carCanv.SetActive(false);
			fpsCanv.SetActive(true);

			
		}

		if (interactDist >= Vector3.Distance(car.transform.position, player.transform.position) && !inCar)
		{
			intText.gameObject.SetActive(true);

			if (Input.GetKeyDown(KeyCode.E) && intTimer <= 0)
			{
				intTimer = intDelay;
				inCar = true;
			}
		}

		if (interactDist <= Vector3.Distance(car.transform.position, player.transform.position) && !inCar)
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
		/*
		foreach (Behaviour yeet in player.GetComponents<Behaviour>())
		{
			if (yeet.GetType() != typeof(Animator))
			{
				yeet.enabled = true;
			}
		}
		*/
		
		playerCam.SetActive(true);
		playerModel.SetActive(true);
		mouse.enabled = true;
		charC.enabled = true;
		pMove.enabled = true;
		cCol.enabled = true;
		
	}
	void playerDisable()
	{
		/*
		foreach (Behaviour yeet in player.GetComponents<Behaviour>())
		{
			if (yeet.GetType() != typeof(Animator))
			{
				yeet.enabled = false;
			}
		}
		*/
		
		playerCam.SetActive(false);
		playerModel.SetActive(false);
		mouse.enabled = false;
		charC.enabled = false;
		pMove.enabled = false;
		cCol.enabled = false;
		
	}
}
