using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSpawnPoints : MonoBehaviour
{
	public GameObject[] spawnPoints;

	public GameObject teamPlayer;
	public GameObject player;

	public int length = 0;

	// Start is called before the first frame update
	void Awake()
	{
		if (!GameMode.mode2Players && !GameMode.mode10Players)
		{
			AutoSpawn();
			Debug.Log("AutoSpawning Player and team");
		}

		if (GameMode.mode2Players)
		{
			if (GameMode.captureTheFlag || GameMode.teamDeathMatch)
			{
				spawnPoints = new GameObject[1];
				for (int i = 0; i < 1; i++)
				{
					spawnPoints[i] = transform.GetChild(i).gameObject;
				}

				length = spawnPoints.Length;

				SpawnTeam1v1();
			}
		}

		if (GameMode.mode10Players)
		{
			if (GameMode.captureTheFlag || GameMode.teamDeathMatch)
			{
				spawnPoints = new GameObject[transform.childCount];
				for (int i = 0; i < transform.childCount; i++)
				{
					spawnPoints[i] = transform.GetChild(i).gameObject;
				}

				length = spawnPoints.Length;

				SpawnTeam5v5();
			}
		}
	}

	void AutoSpawn()
	{
		spawnPoints = new GameObject[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			spawnPoints[i] = transform.GetChild(i).gameObject;
		}

		length = spawnPoints.Length;

		SpawnTeam5v5();
	}

	void SpawnTeam1v1()
	{
		Instantiate<GameObject>(player, spawnPoints[0].transform.position, spawnPoints[0].transform.rotation, null);
	}

	void SpawnTeam5v5()
	{
		Instantiate<GameObject>(teamPlayer, spawnPoints[0].transform.position, spawnPoints[0].transform.rotation, null);
		Instantiate<GameObject>(teamPlayer, spawnPoints[1].transform.position, spawnPoints[1].transform.rotation, null);
		Instantiate<GameObject>(player, spawnPoints[2].transform.position, spawnPoints[2].transform.rotation, null);
		Instantiate<GameObject>(teamPlayer, spawnPoints[3].transform.position, spawnPoints[3].transform.rotation, null);
		Instantiate<GameObject>(teamPlayer, spawnPoints[4].transform.position, spawnPoints[4].transform.rotation, null);
	}	
}
