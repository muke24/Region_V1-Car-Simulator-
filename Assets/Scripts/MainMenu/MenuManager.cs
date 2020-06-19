#region This code is written by Peter Thompson
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public GameObject mainMenuUI;
	public GameObject modeSelect;
	public GameObject typeSelect;

	public Button brButton;
	public GameObject brAvailableText;

	public Button playButton;

	private Animator anim;

	private void Start()
	{
		anim = mainMenuUI.GetComponent<Animator>();
	}

	// Hides the main menu and makes the mode select options appear
	public void GameModeSelect()
	{
		mainMenuUI.SetActive(false);
		modeSelect.SetActive(true);		

		// If multiplayer mode is true then make the BR button interactable and disable the BR not available text
		if (GameMode.multiplayer == true)
		{
			brButton.interactable = true;
			brAvailableText.SetActive(false);
		}

		// If multiplayer mode is not true then make the BR button not interactable and enable the BR not available text
		if (GameMode.multiplayer == false)
		{
			brButton.interactable = false;
			brAvailableText.SetActive(true);
		}
	}

	// Makes the main menu reappear and hides the mode select options, then resets all the selected modes back to their default value of False
	public void BackToMainMenu()
	{
		mainMenuUI.SetActive(true);
		modeSelect.SetActive(false);

		anim.speed = 0f;
		anim.Play("ButtonFadeIn", 0, 1f);

		GameMode.SetAllValuesToFalse();
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
		}

		if (GameMode.multiplayer)
		{
			playButton.transform.Find("Text").GetComponent<Text>().text = "Find Game";
		}

		if (!GameMode.multiplayer && !GameMode.singleplayer)
		{
			playButton.transform.Find("Text").GetComponent<Text>().text = "Play";
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