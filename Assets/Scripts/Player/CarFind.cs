using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFind : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		FindClosestCar();
	}

	void FindClosestCar()
	{
		float distanceToClosestCar = Mathf.Infinity;
		Car closestCar = null;
		Car[] allCars = GameObject.FindObjectsOfType<Car>();

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
