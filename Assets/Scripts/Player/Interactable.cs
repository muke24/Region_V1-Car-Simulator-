using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
	public bool inCar;
	public float interactDist = 3f;

	public GameObject car;
	public GameObject player;
	public Text intText;
	public Camera carCam;

	public PlayerMovement pm;
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
		

		if (interactDist >= Vector3.Distance(car.transform.position, transform.position) && !inCar)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				inCar = true;
				player.SetActive(false);
				carCam.gameObject.SetActive(true);
			}
			intText.gameObject.SetActive(true);
		}

		if (interactDist <= Vector3.Distance(car.transform.position, transform.position) && !inCar)
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

	public void InCar()
	{
		if (inCar)
		{
			pm.enabled = false;
			ml.enabled = false;
			im.enabled = true;
			foreach (DragObject drag in dragObjects)
			{
				drag.enabled = true;
			}
			
		}
		if (!inCar)
		{
			pm.enabled = true;
			ml.enabled = true;
			im.enabled = false;
			foreach (DragObject drag in dragObjects)
			{
				drag.enabled = false;
			}
		}
	}
}
