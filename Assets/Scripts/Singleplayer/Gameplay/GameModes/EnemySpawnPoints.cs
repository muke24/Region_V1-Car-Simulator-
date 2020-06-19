using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoints : MonoBehaviour
{
	public GameObject[] spawnPoints;

	public GameObject enemy;

	public int length = 0;

	// Start is called before the first frame update
	void Start()
	{
		spawnPoints = new GameObject[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			spawnPoints[i] = transform.GetChild(i).gameObject;
		}

		length = spawnPoints.Length;

		SpawnEnemies();
	}

	void SpawnEnemies()
	{
		Instantiate<GameObject>(enemy, spawnPoints[0].transform.position, spawnPoints[0].transform.rotation, null);
		Instantiate<GameObject>(enemy, spawnPoints[1].transform.position, spawnPoints[1].transform.rotation, null);
		Instantiate<GameObject>(enemy, spawnPoints[2].transform.position, spawnPoints[2].transform.rotation, null);
		Instantiate<GameObject>(enemy, spawnPoints[3].transform.position, spawnPoints[3].transform.rotation, null);
		Instantiate<GameObject>(enemy, spawnPoints[4].transform.position, spawnPoints[4].transform.rotation, null);
	}
}
