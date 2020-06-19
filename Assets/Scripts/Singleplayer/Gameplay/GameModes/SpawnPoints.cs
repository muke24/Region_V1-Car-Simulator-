using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
	public float team;

	public GameObject spawn1;
	public GameObject spawn2;

	public GameObject base1;
	public GameObject base2;

	public GameObject[] spawnPoints;
	public GameObject[] enemySpawnPoints;

	public GameObject enemyPlayer;
	public GameObject teamPlayer;
	public GameObject player;

	public int length = 0;

	// Start is called before the first frame update
	void Awake()
	{
		if (!GameMode.singleplayer && !GameMode.multiplayer)
		{
			Debug.LogWarning("No game mode or game type has been selected. Automatically setting the game mode to singleplayer - Capture The Flag");
			GameMode.singleplayer = true;
			GameMode.captureTheFlag = true;
			GameMode.mode10Players = true;
		}

		if (GameMode.singleplayer)
		{
			team = Random.Range(1, 3);

			if (base1.transform.Find("FlagCapture").TryGetComponent(out TeamBase teamBase1))
			{
				Destroy(teamBase1);
			}

			if (base1.transform.Find("FlagCapture").TryGetComponent(out EnemyBase enemyBase1))
			{
				Destroy(enemyBase1);
			}

			if (base2.transform.Find("FlagCapture").TryGetComponent(out TeamBase teamBase2))
			{
				Destroy(teamBase2);
			}

			if (base2.transform.Find("FlagCapture").TryGetComponent(out EnemyBase enemyBase2))
			{
				Destroy(enemyBase2);
			}

			if (GameMode.mode2Players)
			{
				if (GameMode.captureTheFlag || GameMode.teamDeathMatch)
				{
					if (team == 1)
					{
						base1.transform.Find("FlagCapture").gameObject.AddComponent<TeamBase>();
						base2.transform.Find("FlagCapture").gameObject.AddComponent<EnemyBase>();
					}

					if (team == 2)
					{
						base1.transform.Find("FlagCapture").gameObject.AddComponent<EnemyBase>();
						base2.transform.Find("FlagCapture").gameObject.AddComponent<TeamBase>();
					}

					spawnPoints = new GameObject[1];
					for (int i = 0; i < 1; i++)
					{
						if (team == 1)
						{
							spawnPoints[i] = spawn1.transform.GetChild(i).gameObject;
							enemySpawnPoints[i] = spawn2.transform.GetChild(i).gameObject;
						}

						if (team == 2)
						{
							spawnPoints[i] = spawn2.transform.GetChild(i).gameObject;
							enemySpawnPoints[i] = spawn1.transform.GetChild(i).gameObject;
						}
					}

					length = spawnPoints.Length;

					SpawnTeam1v1();
					SpawnEnemies1v1();
				}
			}

			if (GameMode.mode10Players)
			{
				if (GameMode.captureTheFlag || GameMode.teamDeathMatch)
				{
					if (team == 1)
					{
						base1.transform.Find("FlagCapture").gameObject.AddComponent<TeamBase>();
						base2.transform.Find("FlagCapture").gameObject.AddComponent<EnemyBase>();

						spawnPoints = new GameObject[spawn1.transform.childCount];
						for (int i = 0; i < spawn1.transform.childCount; i++)
						{
							spawnPoints[i] = spawn1.transform.GetChild(i).gameObject;
						}

						enemySpawnPoints = new GameObject[spawn2.transform.childCount];
						for (int i = 0; i < spawn2.transform.childCount; i++)
						{
							enemySpawnPoints[i] = spawn2.transform.GetChild(i).gameObject;
						}
					}

					if (team == 2)
					{
						base1.transform.Find("FlagCapture").gameObject.AddComponent<EnemyBase>();
						base2.transform.Find("FlagCapture").gameObject.AddComponent<TeamBase>();

						spawnPoints = new GameObject[spawn2.transform.childCount];
						for (int i = 0; i < spawn2.transform.childCount; i++)
						{
							spawnPoints[i] = spawn2.transform.GetChild(i).gameObject;
						}

						enemySpawnPoints = new GameObject[spawn1.transform.childCount];
						for (int i = 0; i < spawn1.transform.childCount; i++)
						{
							enemySpawnPoints[i] = spawn1.transform.GetChild(i).gameObject;
						}
					}

					length = spawnPoints.Length;

					SpawnTeam5v5();
					SpawnEnemies5v5();
				}
			}
		}

		if (GameMode.multiplayer)
		{

		}

	}

	void SpawnTeam1v1()
	{
		if (GameMode.singleplayer)
		{
			Instantiate<GameObject>(player, spawnPoints[0].transform.position, spawnPoints[0].transform.rotation, null);
		}		
	}

	void SpawnTeam5v5()
	{
		if (GameMode.singleplayer)
		{
			Instantiate(teamPlayer, spawnPoints[0].transform.position, spawnPoints[0].transform.rotation, null);
			Instantiate(teamPlayer, spawnPoints[1].transform.position, spawnPoints[1].transform.rotation, null);
			Instantiate(player, spawnPoints[2].transform.position, spawnPoints[2].transform.rotation, null);
			Instantiate(teamPlayer, spawnPoints[3].transform.position, spawnPoints[3].transform.rotation, null);
			Instantiate(teamPlayer, spawnPoints[4].transform.position, spawnPoints[4].transform.rotation, null);
		}
	}

	void SpawnEnemies1v1()
	{
		if (GameMode.singleplayer)
		{
			Instantiate(enemyPlayer, enemySpawnPoints[0].transform.position, enemySpawnPoints[0].transform.rotation, null);
		}		
	}

	void SpawnEnemies5v5()
	{
		if (GameMode.singleplayer)
		{
			Instantiate(enemyPlayer, enemySpawnPoints[0].transform.position, enemySpawnPoints[0].transform.rotation, null);
			Instantiate(enemyPlayer, enemySpawnPoints[1].transform.position, enemySpawnPoints[1].transform.rotation, null);
			Instantiate(enemyPlayer, enemySpawnPoints[2].transform.position, enemySpawnPoints[2].transform.rotation, null);
			Instantiate(enemyPlayer, enemySpawnPoints[3].transform.position, enemySpawnPoints[3].transform.rotation, null);
			Instantiate(enemyPlayer, enemySpawnPoints[4].transform.position, enemySpawnPoints[4].transform.rotation, null);
		}		
	}
}
