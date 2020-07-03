using UnityEngine;

public class DestroyCollidersAtStart : MonoBehaviour
{
	void Awake()
	{
		foreach (Collider col in FindObjectsOfType<Collider>())
		{
			if (!col.transform.CompareTag("DontDestroyAtStart"))
			{
				Destroy(col);
			}
		}
		foreach (Rigidbody rigid in FindObjectsOfType<Rigidbody>())
		{
			if (!rigid.transform.CompareTag("DontDestroyAtStart"))
			{
				Destroy(rigid);
			}
		}
	}
}
