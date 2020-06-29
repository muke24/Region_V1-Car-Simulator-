#region This code is written by Peter Thompson
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	public GameObject mainMenuUI;
	public GameObject multiplayerType;
	public GameObject joinOrHostLan;
	public GameObject modeSelect;
	public GameObject typeSelect;
	public GameObject lobby;

	public Button brButton;
	public GameObject brAvailableText;

	public Button mode1v1Button;

	public Button playButton;
	public Button findManuallyButton;

	public InputField ipAdressField;
	public InputField portField;

	public string ipString;
	public string portString = "6969";

	private Animator anim;

	[SerializeField] 
	private NetworkManagerLobby networkManager = null;

	private void Start()
	{
		anim = mainMenuUI.GetComponent<Animator>();
	}

	// Makes the main menu reappear and hides the mode select options, then resets all the selected modes back to their default value of False
	public void BackSelect()
	{
		if (GameMode.singleplayer)
		{
			if (modeSelect.activeInHierarchy)
			{
				modeSelect.SetActive(false);
				mainMenuUI.SetActive(true);
				GameMode.SetAllValuesToFalse();
			}

			if (typeSelect.activeInHierarchy)
			{
				modeSelect.SetActive(true);
				typeSelect.SetActive(false);
			}
		}

		if (GameMode.multiplayer)
		{
			if (multiplayerType.activeInHierarchy)
			{
				multiplayerType.SetActive(false);
				mainMenuUI.SetActive(true);
				GameMode.SetAllValuesToFalse();
			}

			if (modeSelect.activeInHierarchy)
			{
				if (GameMode.online)
				{
					modeSelect.SetActive(false);
					multiplayerType.SetActive(true);
					GameMode.online = false;
					GameMode.lan = false;
				}

				if (GameMode.lan)
				{
					modeSelect.SetActive(false);
					joinOrHostLan.SetActive(true);
					GameMode.hosting = false;
					GameMode.joining = false;
				}
			}

			if (joinOrHostLan.activeInHierarchy)
			{
				joinOrHostLan.SetActive(false);
				multiplayerType.SetActive(true);
				GameMode.online = false;
				GameMode.lan = false;
			}

			if (typeSelect.activeInHierarchy)
			{
				modeSelect.SetActive(true);
				typeSelect.SetActive(false);
			}
		}

		anim.speed = 0f;
		anim.Play("ButtonFadeIn", 0, 1f);
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
			multiplayerType.SetActive(true);
			mainMenuUI.SetActive(false);
		}
	}

	public void LANSelect()
	{
		multiplayerType.SetActive(false);
		joinOrHostLan.SetActive(true);
	}

	public void OnlineSelect()
	{
		GameModeSelect();
	}

	private string GetLocalIPv4()
	{
		return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(first => first.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
	}

	public void HostLAN()
	{
		

		GameMode.hosting = true;
		GameMode.joining = false;

		ipAdressField.interactable = false;
		portField.interactable = false;

		ipString = GetLocalIPv4();
		ipAdressField.text = ipString;
		portField.text = "6969";
	}	

	public void JoinLAN()
	{
		GameMode.hosting = false;
		GameMode.joining = true;

		ipAdressField.interactable = false;
		portField.interactable = false;

		ipString = GetLocalIPv4();
		ipAdressField.text = ipString;
		portField.text = "6969";
	}

	public void HostLocalHost()
	{
		GameMode.hosting = true;
		GameMode.joining = false;

		ipAdressField.interactable = false;
		portField.interactable = false;

		ipString = "localhost";
		ipAdressField.text = ipString;
		portField.text = "6969";

		GameModeSelect();
	}

	public void JoinLocalHost()
	{
		GameMode.hosting = false;
		GameMode.joining = true;

		ipAdressField.interactable = false;
		portField.interactable = false;

		ipString = "localhost";
		ipAdressField.text = ipString;
		portField.text = "6969";
	}

	public void JoinCustomLAN()
	{
		GameMode.hosting = false;
		GameMode.joining = true;
	}

	// Hides the main menu and makes the mode select options appear
	public void GameModeSelect()
	{
		// If multiplayer mode is true then make the BR button active and disable the BR not available text
		if (GameMode.multiplayer == true)
		{
			if (GameMode.lan)
			{
				if (GameMode.hosting)
				{
					joinOrHostLan.SetActive(false);
					modeSelect.SetActive(true);

					brButton.gameObject.SetActive(true);
					brAvailableText.SetActive(false);

					brButton.interactable = false;					
					mode1v1Button.interactable = false;
				}

				if (GameMode.joining)
				{

				}
			}

			if (GameMode.online)
			{
				multiplayerType.SetActive(false);
				modeSelect.SetActive(true);

				brButton.gameObject.SetActive(true);
				brAvailableText.SetActive(false);
			}			
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

	// Hides the mode select options and shows the type select options. It then toggles on all of the toggles in case they have been turned off previously
	public void GameTypeSelect()
	{
		#region Set Settings
		modeSelect.SetActive(false);
		typeSelect.SetActive(true);

		foreach (Toggle toggle in typeSelect.transform.Find("Panel").GetComponentsInChildren<Toggle>())
		{
			toggle.isOn = true;
		}
		#endregion

		if (GameMode.singleplayer)
		{
			playButton.transform.Find("Text").GetComponent<Text>().text = "Play";
			findManuallyButton.gameObject.SetActive(false);
		}

		if (GameMode.multiplayer)
		{
			if (GameMode.lan)
			{
				if (GameMode.hosting)
				{
					playButton.transform.Find("Text").GetComponent<Text>().text = "Host Game";
					findManuallyButton.gameObject.SetActive(false);
				}

				if (GameMode.joining)
				{

				}
			}

			if (GameMode.online)
			{
				playButton.transform.Find("Text").GetComponent<Text>().text = "Find Game";
				findManuallyButton.gameObject.SetActive(true);
			}			
		}

		if (!GameMode.multiplayer && !GameMode.singleplayer)
		{
			playButton.transform.Find("Text").GetComponent<Text>().text = "Play";
			findManuallyButton.gameObject.SetActive(false);
		}
	}

	public void HostGame()
	{
		typeSelect.SetActive(false);
		networkManager.StartHost();
	}
}
// This code is written by Peter Thompson
#endregion