#region This code is written by Peter Thompson
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public GameObject mainMenuUI;
	public GameObject multiplayerTypeUI;
	public GameObject modeSelect;
	public GameObject typeSelect;

	public Button brButton;
	public GameObject brAvailableText;

	public Button playButton;
	public Button findManuallyButton;

	private Animator anim;

	private void Start()
	{
		anim = mainMenuUI.GetComponent<Animator>();
	}

	public void MultiplayerTypeSelect()
	{
		// If multiplayer mode is not true then make then skip to GameModeSelect()
		if (GameMode.multiplayer == false)
		{
			GameModeSelect();
			return;
		}

		// If multiplayer mode is true then make the main menu ui inactive and make the multiplayerType UI active
		if (GameMode.multiplayer == true)
		{
			multiplayerTypeUI.SetActive(true);
			mainMenuUI.SetActive(false);
		}		
	}

	// Hides the main menu and makes the mode select options appear
	public void GameModeSelect()
	{
		// If multiplayer mode is true then make the BR button active and disable the BR not available text
		if (GameMode.multiplayer == true)
		{
			multiplayerTypeUI.SetActive(false);
			modeSelect.SetActive(true);

			brButton.gameObject.SetActive(true);
			brAvailableText.SetActive(false);
		}

		// If multiplayer mode is not true then make the BR button inactive and enable the BR not available text
		if (GameMode.multiplayer == false)
		{
			mainMenuUI.SetActive(false);
			modeSelect.SetActive(true);

			brButton.gameObject.SetActive(false);
			brAvailableText.SetActive(true);
		}
	}

	// Makes the main menu reappear and hides the mode select options, then resets all the selected modes back to their default value of False
	public void BackToMainMenu()
	{
		if (GameMode.singleplayer)
		{
			modeSelect.SetActive(false);
			mainMenuUI.SetActive(true);
			GameMode.SetAllValuesToFalse();
		}
		if (GameMode.multiplayer)
		{
			if (multiplayerTypeUI.activeInHierarchy)
			{
				multiplayerTypeUI.SetActive(false);
				mainMenuUI.SetActive(true);
				GameMode.SetAllValuesToFalse();
			}
			if (modeSelect.activeInHierarchy)
			{
				modeSelect.SetActive(false);
				multiplayerTypeUI.SetActive(true);
				GameMode.online = false;
				GameMode.lan = false;
			}
		}		

		anim.speed = 0f;
		anim.Play("ButtonFadeIn", 0, 1f);

		
	}

	// Hides the mode select options and shows the type select options. It then toggles on all of the toggles in case they have been turned off previously
	public void GameTypeSelect()
	{
		modeSelect.SetActive(false);
		typeSelect.SetActive(true);

		foreach (Toggle toggle in typeSelect.transform.Find("Panel").GetComponentsInChildren<Toggle>())
		{
			toggle.isOn = true;
		}

		if (GameMode.singleplayer)
		{
			playButton.transform.Find("Text").GetComponent<Text>().text = "Play";
			findManuallyButton.gameObject.SetActive(false);
		}

		if (GameMode.multiplayer)
		{
			playButton.transform.Find("Text").GetComponent<Text>().text = "Find Game";
			findManuallyButton.gameObject.SetActive(true);
		}

		if (!GameMode.multiplayer && !GameMode.singleplayer)
		{
			playButton.transform.Find("Text").GetComponent<Text>().text = "Play";
			findManuallyButton.gameObject.SetActive(false);
		}
	}

	// Hides the type select options and shows the mode select options
	public void BackToGameModeSelect()
	{
		modeSelect.SetActive(true);
		typeSelect.SetActive(false);
	}
}
// This code is written by Peter Thompson
#endregion