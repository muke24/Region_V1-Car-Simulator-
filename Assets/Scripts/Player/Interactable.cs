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

	public Canvas carCanv;
	public Canvas fpsCanv;

	public GameObject car;
	public GameObject player;
	public Text intText;
	public Camera carCam;

	public PlayerController pc;
	public MouseLook ml;

	public CarController cc;
	public InputManager im;


	public List<DragObject> dragObjects;
	// Start is called before the first frame update
	void Start()
	{
		inCar = false;
		carCam.gameObject.SetActive(false);
		player.gameObject.SetActive(true);
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

			player.SetActive(false);
			im.enabled = true;
			foreach (DragObject drag in dragObjects)
			{
				drag.enabled = true;
			}
			carCam.gameObject.SetActive(true);
			player.SetActive(false);

			carCanv.enabled = true;
			fpsCanv.enabled = false;

			Cursor.lockState = CursorLockMode.Confined;
		}
		if (!inCar)
		{
			player.SetActive(true);
			im.enabled = false;
			foreach (DragObject drag in dragObjects)
			{
				drag.enabled = false;
			}

			carCanv.enabled = false;
			fpsCanv.enabled = true;

			Cursor.lockState = CursorLockMode.Locked;
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

}
