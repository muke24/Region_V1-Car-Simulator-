using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
	public Image pauseMenu;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
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

	}
}
