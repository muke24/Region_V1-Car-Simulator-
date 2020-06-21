using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
	private Image loadingBar;

	private Text gameTypeText;
	private Text gameModeText;

	private void Awake()
	{
		loadingBar = transform.Find("Bar").transform.Find("MaskLoadingBar").transform.Find("LoadingBar").GetComponent<Image>();
		gameTypeText = transform.Find("GameTypeText").GetComponent<Text>();
		gameModeText = transform.Find("GameModeText").GetComponent<Text>();
		
		#region Game Type Text
		if (GameMode.freeForAll)
		{
			gameTypeText.text = "Free For All";
		}
		if (GameMode.teamDeathMatch)
		{
			gameTypeText.text = "Team Death Match";
		}
		if (GameMode.captureTheFlag)
		{
			gameTypeText.text = "Capture The Flag";
		}
		if (GameMode.battleRoyale)
		{
			gameTypeText.text = "Battle Royale";
		}

		if (!GameMode.freeForAll && !GameMode.teamDeathMatch && !GameMode.captureTheFlag && !GameMode.battleRoyale)
		{
			Debug.Log("No game mode or game type has been selected. Loading automatic Singleplayer 5v5 settings");

			gameTypeText.text = "Development Auto";
			gameModeText.text = "Auto 5v5";
		}
		#endregion

		#region Game Mode Text
		if (GameMode.mode2Players)
		{
			gameModeText.text = "1v1";
		}
		if (GameMode.mode10Players)
		{
			gameModeText.text = "5v5";
		}
		if (GameMode.mode100Players)
		{
			gameModeText.text = "Battle Royale";
		}
		#endregion

		//Cursor.visible = false;
		//Cursor.lockState = CursorLockMode.Locked;
	}

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(LoadAsyncOperation());

		loadingBar.fillAmount = 0.05f;

		loadingBar.fillAmount = 0.125f;
	}

	IEnumerator LoadAsyncOperation()
	{
		AsyncOperation gameLevel = SceneManager.LoadSceneAsync("InGame");

		while (gameLevel.progress < 1f)
		{
			loadingBar.fillAmount = gameLevel.progress;

			yield return new WaitForEndOfFrame();
		}
	}
}
