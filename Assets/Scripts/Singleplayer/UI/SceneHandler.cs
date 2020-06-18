using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
	public void MainMenuScene()
	{
		SceneManager.LoadScene("MainMenu");
	}
	public void PlayScene()
	{
		SceneManager.LoadScene("InGame");
	}
	public void QuitGame()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
