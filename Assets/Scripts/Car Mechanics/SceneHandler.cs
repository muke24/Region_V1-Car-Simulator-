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
	public void MultiplayerRegisterScene()
	{
		SceneManager.LoadScene("LobbyScene");
	}
	public void QuitGame()
	{
		Application.Quit();
	}
}
