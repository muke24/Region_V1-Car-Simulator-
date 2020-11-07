using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTrigger : MonoBehaviour
{
	public bool planeSpawned = false;

	private PlaneController planeController;
	private Collider planeCollider;

	[Header("Debugging (only when plane spawns)")]
	[SerializeField]
	private bool debugColliders = false;

	[SerializeField]
	private Collider[] collidersNotIgnored;

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

				collidersNotIgnored = new Collider[notIgnoredColliders];
				collidersNotIgnored[0] = collider1;
				collidersNotIgnored[1] = collider2;
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

	private void OnTriggerEnter(Collider other)
	{
		if (other == planeCollider)
		{
			planeController.canJump = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other == planeCollider)
		{
			planeController.canJump = false;
			Destroy(this);
		}
	}
}
