using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
	[SerializeField]
	private GameObject pause;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (pause.activeInHierarchy)
			{
				pause.SetActive(false);
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				pause.SetActive(true);
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}

	public void BackToMainMenu()
	{
		SceneManager.LoadScene(0);
	}
}
