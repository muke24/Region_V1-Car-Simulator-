using System;
using UnityEngine;
using MyBox;

public class PlaneTrigger : MonoBehaviour
{
	public static bool planeSpawned = false;

	private PlaneController planeController;
	private Collider planeCollider;

	[Header("Debugging (only when plane spawns)")]
	[SerializeField]
	private bool debugColliders = false;

	[ConditionalField("debugColliders")]
	[SerializeField] 
	private Debugging debugging;

	private void OnTriggerEnter(Collider other)
	{
		if (other == planeCollider)
		{
			planeController.GetComponent<Animator>().SetInteger("DoorState", 1);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other == planeCollider)
		{
			planeController.GetComponent<Animator>().SetInteger("DoorState", 2);
			Destroy(this);
		}
	}

	private void Awake()
	{
		DetectAndIgnoreTriggers();
	}

	public void DetectAndIgnoreTriggers()
	{
		if (planeSpawned)
		{
			planeController = FindObjectOfType<PlaneController>();
			planeCollider = planeController.gameObject.GetComponent<Collider>();
			Collider planeTrigger = GetComponent<Collider>();
			int notIgnoredColliders = 0;
			Collider[] allColliders = FindObjectsOfType<Collider>();

			if (!debugColliders)
			{
				foreach (Collider collider in FindObjectsOfType<Collider>())
				{
					if (collider != planeCollider && collider != planeTrigger)
					{
						Physics.IgnoreCollision(planeTrigger, collider);
						Physics.IgnoreCollision(planeCollider, collider);
					}
				}
			}

			#region Debug Colliders
			if (debugColliders)
			{
				Collider collider1 = null;
				Collider collider2 = null;

				for (int i = 0; i < allColliders.Length; i++)
				{
					if (allColliders[i] != planeCollider && allColliders[i] != planeTrigger)
					{
						Physics.IgnoreCollision(planeTrigger, allColliders[i]);
						Physics.IgnoreCollision(planeCollider, allColliders[i]);
					}
					else
					{
						notIgnoredColliders++;
						if (collider1 == null)
						{
							collider1 = allColliders[i];
						}
						else
						{
							collider2 = allColliders[i];
						}
					}
				}

				debugging.collidersNotIgnored = new Collider[notIgnoredColliders];
				debugging.collidersNotIgnored[0] = collider1;
				debugging.collidersNotIgnored[1] = collider2;
			}
			#endregion
		}

		else
		{
			Collider planeTrigger = GetComponent<Collider>();

			foreach (Collider collider in FindObjectsOfType<Collider>())
			{
				if (collider != planeTrigger)
				{
					Physics.IgnoreCollision(planeTrigger, collider);
				}
			}
		}
	}
}

[Serializable]
class Debugging
{
	public Collider[] collidersNotIgnored;
}