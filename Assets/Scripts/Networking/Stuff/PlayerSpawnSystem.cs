using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnSystem : NetworkBehaviour
{
	[SerializeField] private GameObject playerPrefab = null;

	private static List<Transform> spawnPoints = new List<Transform>();

	private int nextIndex = 0;

	public static void AddSpawnPoint(Transform spawnTransform)
	{
		spawnPoints.Add(spawnTransform);

		spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
	}

	public static void RemoveSpawnPoint(Transform spawnTransform)
	{
		spawnPoints.Remove(spawnTransform);
	}

	public override void OnStartServer()
	{
		NetworkManagerLobby.onServerReadied += SpawnPlayer;
	}

	[ServerCallback]
	private void OnDestroy()
	{
		NetworkManagerLobby.onServerReadied -= SpawnPlayer;
	}

	[Server]
	public void SpawnPlayer(NetworkConnection conn)
	{
		Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

		if (spawnPoint == null)
		{
			Debug.LogError("Missing spawn point for player" + nextIndex);
			return;
		}

		GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
		NetworkServer.Spawn(playerInstance, conn);

		nextIndex++;
	}
}
