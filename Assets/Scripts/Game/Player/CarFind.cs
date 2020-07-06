#region This code is written by Peter Thompson
using UnityEngine;

public class CarFind : MonoBehaviour
{
	public static bool inCarTrigger = false;

	public Car closestCar;

	private Car[] allCars;

	void Awake()
	{
		allCars = FindObjectsOfType<Car>();
	}

	private void Start()
	{
		FindClosestCar();
	}

	void FixedUpdate()
	{
		if (inCarTrigger)
		{
			if (!Car.inCar)
			{
				FindClosestCar();
			}
		}
	}

	void FindClosestCar()
	{
		float distanceToClosestCar = Mathf.Infinity;

		foreach (Car currentCar in allCars)
		{
			float distanceToCar = (currentCar.transform.position - transform.position).sqrMagnitude;
			if (distanceToCar < distanceToClosestCar)
			{
				distanceToClosestCar = distanceToCar;
				closestCar = currentCar;
			}
		}

		Debug.DrawLine(transform.position, closestCar.transform.position, Color.red);
	}
}
// This code is written by Peter Thompson
#endregion