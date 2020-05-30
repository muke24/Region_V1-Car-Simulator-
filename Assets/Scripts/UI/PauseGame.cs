#region This code is written by Peter Thompson
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

	public float settingChangeTimer = 0.10f;
	public bool timerchanger = false;
	public int carSetting = 0;

	public GameObject suspensionSet;	// carSetting 0
	public GameObject physicsSet;		// carSetting 1
	public GameObject engineSet;		// carSetting 2
	public GameObject strengthSet;		// carSetting 3

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			// Toggle pause menu on/off
			PauseGameScreenToggle();
		}

		if (settingChangeTimer <= 0)
		{
			timerchanger = false;
			settingChangeTimer = 0.10f;
		}

		if (timerchanger)
		{
			settingChangeTimer -= Time.deltaTime;
		}

		#region In game car settings changer (planning to remove)
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
			strengthSet.SetActive(true);	// carSetting 3

			suspensionSet.SetActive(false); // carSetting 0
			physicsSet.SetActive(false);    // carSetting 1
			engineSet.SetActive(false);     // carSetting 2
		}
		#endregion
	}

	public void PauseGameScreenToggle()
	{
		if (pauseMenu.gameObject.activeSelf)
		{
			// Toggles off all sub-pause screens (eg. weapon skins)
			pause.SetActive(true);
			pauseMenu.gameObject.SetActive(false);
			settings.SetActive(false);
			custCar.SetActive(false);
			skins.SetActive(false);
		}
		else
		{
			// Toggles on pause if all sub-pause screens are toggled off
			pauseMenu.gameObject.SetActive(true);
		}
	}

	// Void is used when UI button is clicked in pause menu
	public void CustomiseCar()
	{
		// Toggles off main pause screen and toggles on customise car screen
		pause.SetActive(false);
		custCar.SetActive(true);
	}

	// Void is used when UI button is clicked in pause menu
	public void Settings()
	{
		// Toggles off main pause screen and toggles on customise car screen
		pause.SetActive(false);
		settings.SetActive(true);
	}

	// Void is used when UI button is clicked in pause menu
	public void PauseGameScreen()
	{
		pause.SetActive(true);
		settings.SetActive(false);
		custCar.SetActive(false);
		skins.SetActive(false);
		gunViewerCam.SetActive(false);
		gunImg.gameObject.SetActive(false);
	}

	// Void is used when UI button is clicked in car setting menu
	public void CarSettingButtonLeft()
	{
		if (!timerchanger)
		{
			if (carSetting == 0)
			{
				timerchanger = true;
				carSetting = 3;
			}
		}
		if (!timerchanger)
		{
			if (carSetting == 3)
			{
				timerchanger = true;
				carSetting = 2;
			}
		}
		if (!timerchanger)
		{
			if (carSetting == 2)
			{
				timerchanger = true;
				carSetting = 1;
			}
		}
		if (!timerchanger)
		{
			if (carSetting == 1)
			{
				timerchanger = true;
				carSetting = 0;
			}
		}
		

	}

	// Void is used when UI button is clicked in car setting menu
	public void CarSettingButtonRight()
	{
		if (!timerchanger)
		{
			if (carSetting == 0)
			{
				timerchanger = true;
				carSetting = 1;
			}
		}
		if (!timerchanger)
		{
			if (carSetting == 1)
			{
				timerchanger = true;
				carSetting = 2;
			}
		}
		if (!timerchanger)
		{
			if (carSetting == 2)
			{
				timerchanger = true;
				carSetting = 3;
			}
		}
		if (!timerchanger)
		{
			if (carSetting == 3)
			{
				timerchanger = true;
				carSetting = 0;
			}
		}		
	}

	// Void is used when UI button is clicked in pause menu
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
// This code is written by Peter Thompson
#endregion