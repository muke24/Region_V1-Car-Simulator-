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
	public GameObject skins;
	public GameObject gunViewerCam;
	public RawImage gunImg;
	public int carSetting = 0;

	public float settingChangeTimer = 0.10f;
	public bool timechanger = false;

	public GameObject suspensionSet;	// carSetting 0
	public GameObject physicsSet;		// carSetting 1
	public GameObject engineSet;		// carSetting 2
	public GameObject strengthSet;		// carSetting 3


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGameScreenToggle();
		}

		if (settingChangeTimer <= 0)
		{
			timechanger = false;
			settingChangeTimer = 0.10f;
		}
		if (timechanger)
		{
			settingChangeTimer -= Time.deltaTime;
		}

		if (carSetting == 0)
		{
			suspensionSet.SetActive(true);  // carSetting 0

			physicsSet.SetActive(false);    // carSetting 1
			engineSet.SetActive(false);     // carSetting 2
			strengthSet.SetActive(false);   // carSetting 3
		}

		if (carSetting == 1)
		{
			physicsSet.SetActive(true);     // carSetting 1

			suspensionSet.SetActive(false); // carSetting 0
			engineSet.SetActive(false);     // carSetting 2
			strengthSet.SetActive(false);   // carSetting 3
		}
		
		if (carSetting == 2)
		{
			engineSet.SetActive(true);      // carSetting 2

			suspensionSet.SetActive(false); // carSetting 0
			physicsSet.SetActive(false);    // carSetting 1
			strengthSet.SetActive(false);   // carSetting 3
		}

		if (carSetting == 3)
		{
			strengthSet.SetActive(true);   // carSetting 3

			suspensionSet.SetActive(false); // carSetting 0
			physicsSet.SetActive(false);    // carSetting 1
			engineSet.SetActive(false);     // carSetting 2
		}

	}

	public void PauseGameScreenToggle()
	{
		if (pauseMenu.gameObject.activeSelf == true)
		{
			pauseMenu.gameObject.SetActive(false);
			pause.gameObject.SetActive(true);
			settings.gameObject.SetActive(false);
			custCar.gameObject.SetActive(false);
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
		skins.SetActive(false);
		gunViewerCam.SetActive(false);
		gunImg.gameObject.SetActive(false);
	}

	public void CarSettingButtonLeft()
	{
		if (!timechanger)
		{
			if (carSetting == 0)
			{
				timechanger = true;
				carSetting = 3;
			}
		}
		if (!timechanger)
		{
			if (carSetting == 3)
			{
				timechanger = true;
				carSetting = 2;
			}
		}
		if (!timechanger)
		{
			if (carSetting == 2)
			{
				timechanger = true;
				carSetting = 1;
			}
		}
		if (!timechanger)
		{
			if (carSetting == 1)
			{
				timechanger = true;
				carSetting = 0;
			}
		}
		

	}
	public void CarSettingButtonRight()
	{
		if (!timechanger)
		{
			if (carSetting == 0)
			{
				timechanger = true;
				carSetting = 1;
			}
		}
		if (!timechanger)
		{
			if (carSetting == 1)
			{
				timechanger = true;
				carSetting = 2;
			}
		}
		if (!timechanger)
		{
			if (carSetting == 2)
			{
				timechanger = true;
				carSetting = 3;
			}
		}
		if (!timechanger)
		{
			if (carSetting == 3)
			{
				timechanger = true;
				carSetting = 0;
			}
		}
		
	}

	public void Skins()
	{
		if (!skins.activeSelf)
		{
			skins.SetActive(true);
			pause.SetActive(false);
			gunViewerCam.SetActive(true);
			gunImg.gameObject.SetActive(true);
		}
	}
}
