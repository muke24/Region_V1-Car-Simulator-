using UnityEngine;

public class DestroyCollidersAtStart : MonoBehaviour
{
	void Awake()
	{
		foreach (Collider col in FindObjectsOfType<Collider>())
		{
			if (!col.CompareTag("DontDestroyAtStart"))
			{
				Destroy(col);
			}
		}
	}
}
