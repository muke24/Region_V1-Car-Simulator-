using UnityEngine;
using UnityEngine.AI;

public class BreakObject : MonoBehaviour
{
	private Rigidbody[] rigids;
	private bool destroyingParent = false;
	private float timer = 15f;
	private float alpha = 1;
	private Material[] mats;

	private void Awake()
	{
		if (name.Contains("BreakableFence"))
		{
			GetComponent<FixedJoint>().connectedBody = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Rigidbody>();
		}

		rigids = new Rigidbody[GetComponentsInChildren<Rigidbody>().Length];
		rigids = GetComponentsInChildren<Rigidbody>();
		Renderer[] renderers = new Renderer[GetComponentsInChildren<Renderer>().Length];

		int count = 0;
		for (int i = 0; i < renderers.Length; i++)
		{
			count += renderers[i].materials.Length;
		}

		mats = new Material[count];

		for (int i = 0; i < renderers.Length; i++)
		{
			for (int e = 0; e < count; e++)
			{
				mats[e] = renderers[i].materials[e];
			}
		}
	}

	private void Update()
	{
		if (destroyingParent)
		{
			if (timer > 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				foreach (Material mat in mats)
				{
					Color color = mat.color;
					color.a -= Time.deltaTime;
					mat.color = color;
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		foreach (Rigidbody rigid in rigids)
		{
			if (rigid != null)
			{
				if (rigid.isKinematic && other.transform.CompareTag("Car"))
				{
					rigid.isKinematic = false;

					if (!destroyingParent)
					{
						Destroy(rigid.gameObject, 20f);
						Destroy(GetComponent<NavMeshObstacle>(), 1f);
						destroyingParent = true;
					}
				}
			}
		}
	}
}
