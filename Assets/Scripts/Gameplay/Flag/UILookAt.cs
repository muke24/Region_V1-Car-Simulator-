using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILookAt : MonoBehaviour
{
	public GameObject playerCam;
	public GameObject carCam;

	public GameObject flag;
	public float scaleMultiplier = 2;

	private void Start()
	{
		playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").gameObject;
		carCam = GameObject.FindGameObjectWithTag("CarCam");
	}

	// Update is called once per frame
	void Update()
	{
		if (!Car.inCar)
		{
			GetComponent<Canvas>().transform.localScale = new Vector3(Vector3.Distance(playerCam.transform.position, GetComponent<Canvas>().transform.position) * scaleMultiplier, Vector3.Distance(playerCam.transform.position, GetComponent<Canvas>().transform.position) * scaleMultiplier, Vector3.Distance(playerCam.transform.position, GetComponent<Canvas>().transform.position) * scaleMultiplier);
			GetComponent<Canvas>().worldCamera = playerCam.GetComponent<Camera>();
			transform.position = new Vector3(flag.transform.position.x, flag.transform.position.y + 4f, flag.transform.position.z);

			Vector3 v = playerCam.transform.position - transform.position;
			v.x = v.z = 0.0f;
			transform.LookAt(playerCam.transform.position - v);
			transform.Rotate(0, 180, 0);
		}

		if (Car.inCar)
		{
			GetComponent<Canvas>().transform.localScale = new Vector3(Vector3.Distance(carCam.transform.position, GetComponent<Canvas>().transform.position) * scaleMultiplier, Vector3.Distance(carCam.transform.position, GetComponent<Canvas>().transform.position) * scaleMultiplier, Vector3.Distance(carCam.transform.position, GetComponent<Canvas>().transform.position) * scaleMultiplier);
			GetComponent<Canvas>().worldCamera = carCam.GetComponent<Camera>();
			transform.position = new Vector3(flag.transform.position.x, flag.transform.position.y + 4f, flag.transform.position.z);

			Vector3 v = carCam.transform.position - transform.position;
			v.x = v.z = 0.0f;
			transform.LookAt(carCam.transform.position - v);
			transform.Rotate(0, 180, 0);
		}

		if (!flag.activeSelf)
		{
			GetComponentInChildren<Image>().enabled = false;
		}
	}
}
