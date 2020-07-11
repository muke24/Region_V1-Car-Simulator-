#region This code is written by Peter Thompson
using UnityEngine;

public class IgnoreCollisionsOnCar : MonoBehaviour
{
	public Collider[] carColliders;

	// Start is called before the first frame update
	void Awake()
	{
		FindAndIgnoreColliders();
	}

	public void FindAndIgnoreColliders()
	{
		carColliders = GetComponentsInChildren<Collider>();

		foreach (var item in carColliders)
		{
			for (int i = 0; i < carColliders.Length; i++)
			{
				if (item != carColliders[i])
				{
					Physics.IgnoreCollision(item, carColliders[i], true);
				}
			}
		}

		//for (int i = 0; i < carColliders.Length; i++)
		//{
		//	try
		//	{
		//		foreach (Collider collider in carColliders)
		//		{
		//			if (i == carColliders.Length - 1)
		//			{
		//				Physics.IgnoreCollision(collider, carColliders[i++ - 1]);
		//			}
		//			if (i >= 0 && i < (carColliders.Length - 1))
		//			{
		//				Physics.IgnoreCollision(collider, carColliders[i++]);
		//			}

		//			if (i == 0)
		//			{
		//				Physics.IgnoreCollision(collider, carColliders[1 - i--]);
		//			}
		//			if (i <= carColliders.Length - 1 && i > 0)
		//			{
		//				Physics.IgnoreCollision(collider, carColliders[i--]);
		//			}
		//		}
		//	}
		//	catch (System.Exception)
		//	{

		//		throw;
		//	}
		//}
	}
}
// This code is written by Peter Thompson
#endregion