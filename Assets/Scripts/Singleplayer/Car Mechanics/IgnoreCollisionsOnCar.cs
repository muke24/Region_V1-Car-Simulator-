using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionsOnCar : MonoBehaviour
{
	private Collider[] carColliders;

	// Start is called before the first frame update
	void Start()
	{
		carColliders = GetComponentsInChildren<Collider>();
		
		for (int i = 0; i < carColliders.Length; i++)
		{
			foreach (Collider collider in carColliders)
			{
				if (i == 29)
				{
					Physics.IgnoreCollision(collider, carColliders[i++ - 1]);
				}
				if (i >= 0 && i < 29)
				{
					Physics.IgnoreCollision(collider, carColliders[i++]);
				}				

				if (i == 0)
				{
					Physics.IgnoreCollision(collider, carColliders[1 - i--]);
				}
				if (i <= 28 && i > 0)
				{
					Physics.IgnoreCollision(collider, carColliders[i--]);
				}				
			}
		}
	}
}
