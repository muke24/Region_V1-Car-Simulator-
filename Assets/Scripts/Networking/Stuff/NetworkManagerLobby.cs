using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 2;
    [Scene][SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayer roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayer gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;

    public event Action onClientConnected;
    public event Action onClientDisconnected;
    public static event Action<NetworkConnection> onServerReadied;

    public List<NetworkRoomPlayer> RoomPlayers { get; } = new List<NetworkRoomPlayer>();
    public List<NetworkGamePlayer> GamePlayers { get; } = new List<NetworkGamePlayer>();

    public override void OnStartServer()
    {
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList<GameObject>();
    }

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach(var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        onClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        onClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if(numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        //only if we want people to join only in the lobby
        if(SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayer roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }


    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if(conn.identity != null)
        {
            NetworkRoomPlayer player = conn.identity.GetComponent<NetworkRoomPlayer>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if(numPlayers < minPlayers)
        {
            return false;
        }

        foreach(var player in RoomPlayers)
        {
            if(!player.IsReady)
            {
                return false;
            }
        }

        return true;
    }


    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        onServerReadied?.Invoke(conn);
    }

    public void StartGame()
    {
        if(SceneManager.GetActiveScene().path == menuScene)
        {
            if(!IsReadyToStart())
            {
                return;
            }

            ServerChangeScene("Game_Map_01");
        }
    }

    
    public override void ServerChangeScene(string newSceneName)
    {
        //from menu to game
        if(SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Game_Map"))
        {
            for(int i = RoomPlayers.Count -1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                NetworkGamePlayer gamePlayerInstance = Instantiate(gamePlayerPrefab);
                gamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject, true);
            }
        }


        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Game_Map"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }
}
