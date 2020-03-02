using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public void NextScene()
	{
		SceneManager.LoadScene("InGame");
	}
	public void QuitGame()
	{
		Application.Quit();
	}
}
