#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionProbePosition : MonoBehaviour
{
	[SerializeField]
	private Interact interact;

	public Transform seat1Pos;
	public ReflectionProbe reflectionProbe;
	public GameObject carCam;
	public GameObject playerCam;

	// Start is called before the first frame update
	void Start()
	{
		// Retrieves the interact script
		interact = GetComponent<Interact>();
		// Retrieves the reflection probe
		reflectionProbe = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").transform.Find("Reflection Probe").GetComponent<ReflectionProbe>();
		// Retrieves the player's camera
		playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").gameObject;
	}

	// Update is called once per frame
	void Update()
	{
		if (reflectionProbe == null)
		{
			reflectionProbe = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").transform.Find("Reflection Probe").GetComponent<ReflectionProbe>();
			// Retrieves the player's camera
			playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").gameObject;
		}

		// Sets the reflection probe to the car's camera position
		if (interact.inCar)
		{
			reflectionProbe.transform.parent = carCam.transform;
			reflectionProbe.transform.localPosition = Vector3.zero;
		}
		// Sets the reflection probe to the player's camera position
		if (!interact.inCar)
		{
			reflectionProbe.transform.parent = playerCam.transform;
			reflectionProbe.transform.localPosition = Vector3.zero;
		}
	}
}
// This code is written by Peter Thompson
#endregion