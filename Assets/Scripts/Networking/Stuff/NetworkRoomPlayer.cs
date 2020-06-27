using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class NetworkRoomPlayer : NetworkBehaviour
{
	[Header("UI")]
	[SerializeField] private GameObject lobbyUI = null;
	[SerializeField] private Text[] playerNameTexts = new Text[10];
	[SerializeField] private Text[] playerReadyTexts = new Text[10];
	[SerializeField] private Button startGameButton = null;

	[SyncVar(hook = nameof(HandleDisplayNameChanged))]
	public string DisplayName = "Loading...";
	[SyncVar(hook = nameof(HandleReadyStatusChanged))]
	public bool IsReady = false;

	private bool isLeader = false;

	public bool IsLeader
	{
		set
		{
			isLeader = value;
			if (startGameButton != null)
			{
				startGameButton.gameObject.SetActive(value);
			}
		}
	}

	private NetworkManagerLobby room;
	private NetworkManagerLobby Room
	{
		get
		{
			if (room != null)
			{
				return room;
			}
			room = NetworkManager.singleton as NetworkManagerLobby;
			return room;
		}
	}

	public override void OnStartAuthority()
	{
		if (Login.playerUsername != "")
		{
			CmdSetDisplayName(Login.playerUsername);
		}

		if (Login.playerUsername == "")
		{
			CmdSetDisplayName("Guest");
		}

		lobbyUI.SetActive(true);
	}

	public override void OnStartClient()
	{
		Room.RoomPlayers.Add(this);
		UpdateDisplay();
	}

	public override void OnStopClient()
	{
		Room.RoomPlayers.Remove(this);
		UpdateDisplay();
	}

	public void HandleDisplayNameChanged(string oldValue, string newValue)
	{
		UpdateDisplay();
	}

	public void HandleReadyStatusChanged(bool oldValue, bool newValue)
	{
		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		if (!hasAuthority)
		{
			foreach (var player in Room.RoomPlayers)
			{
				if (player.hasAuthority)
				{
					player.UpdateDisplay();
					break;
				}
			}

			return;
		}

		for (int i = 0; i < playerNameTexts.Length; i++)
		{
			playerNameTexts[i].text = "Waiting for Player...";
			playerReadyTexts[i].text = string.Empty;
		}

		for (int i = 0; i < Room.RoomPlayers.Count; i++)
		{
			playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
			playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
		}
	}

	public void HandleReadyToStart(bool readyToStart)
	{
		if (!isLeader)
		{
			return;
		}

		startGameButton.interactable = readyToStart;
	}

	[Command]
	private void CmdSetDisplayName(string displayName)
	{
		DisplayName = displayName;
	}

	[Command]
	public void CmdReadyUp()
	{
		IsReady = !IsReady;

		Room.NotifyPlayersOfReadyState();
	}

	[Command]
	public void CmdStartGame()
	{
		if (connectionToClient != Room.RoomPlayers[0].connectionToClient)
		{
			return;
		}

		Room.StartGame();
	}
}
