using UnityEngine;

public class ErrorBox : MonoBehaviour
{
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void Close()
	{
		Destroy(gameObject);
	}
}