using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
	public RawImage compassScrollTexture;
	public Transform playerPositionInWorld;
	public CurrentCar currentCar;

	private void Start()
	{
		playerPositionInWorld = GameObject.FindGameObjectWithTag("Player").transform;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (playerPositionInWorld == null)
		{
			playerPositionInWorld = GameObject.FindGameObjectWithTag("Player").transform;
		}

		compassScrollTexture.uvRect = new Rect(playerPositionInWorld.localEulerAngles.y / 360f, 0, 1, 1);

		if (Car.inCar)
		{
			playerPositionInWorld = currentCar.currentCar.transform;
		}
		else
		{
			playerPositionInWorld = GameObject.FindGameObjectWithTag("Player").transform;
		}
	}
}
