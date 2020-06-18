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

		GameMode.SetAllValuesToFalse();
	}

	public void GameTypeSelect()
	{
		modeSelect.SetActive(false);
		typeSelect.SetActive(true);
	}
}
// This code is written by Peter Thompson
#endregion