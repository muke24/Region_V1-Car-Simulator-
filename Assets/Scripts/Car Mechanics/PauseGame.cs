using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
	public Image pauseMenu;
	public GameObject pause;
	public GameObject settings;
	public GameObject custCar;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGameScreenToggle();
		}
	}

	public void PauseGameScreenToggle()
	{
		if (pauseMenu.gameObject.activeSelf == true)
		{
			pauseMenu.gameObject.SetActive(false);
		}
		else
		{
			pauseMenu.gameObject.SetActive(true);
		}
	}

	public void CustomiseCar()
	{
		pause.SetActive(false);
		custCar.SetActive(true);
	}

	public void Settings()
	{
		pause.SetActive(false);
		settings.SetActive(true);
	}

	public void PauseGameScreen()
	{
		pause.SetActive(true);
		settings.SetActive(false);
		custCar.SetActive(false);
	}

}
