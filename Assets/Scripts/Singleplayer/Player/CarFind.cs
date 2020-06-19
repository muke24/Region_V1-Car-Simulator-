#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFind : MonoBehaviour
{
	public Car closestCar;

	// Start is called before the first frame update
	void Awake()
	{
		FindClosestCar();
	}

	// Update is called once per frame
	void Update()
	{
		if (!Car.inCar)
		{
			FindClosestCar();
		}
	}

	void FindClosestCar()
	{
		float distanceToClosestCar = Mathf.Infinity;
		closestCar = null;
		Car[] allCars = FindObjectsOfType<Car>();

		foreach (Car currentCar in allCars)
		{
			float distanceToCar = (currentCar.transform.position - this.transform.position).sqrMagnitude;
			if (distanceToCar < distanceToClosestCar)
			{
				distanceToClosestCar = distanceToCar;
				closestCar = currentCar;
			}
		}

		Debug.DrawLine(this.transform.position, closestCar.transform.position, Color.red);
	}
}
// This code is written by Peter Thompson
#endregion