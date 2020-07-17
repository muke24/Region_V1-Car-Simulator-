using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
	public GameObject pause;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (pause.activeInHierarchy)
			{
				pause.SetActive(false);
				Look.isPaused = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				pause.SetActive(true);
				Look.isPaused = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}

	public void BackToMainMenu()
	{
		SceneManager.LoadScene(0);
	}
}
