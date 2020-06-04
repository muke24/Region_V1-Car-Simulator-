using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
	public GameObject[] spawnPoints;

	public GameObject enemy;

	public int randomSpawnPoint = 0;

	public int length = 0;

	public float timer = 1f;

	// Start is called before the first frame update
	void Start()
	{
		spawnPoints = new GameObject[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			spawnPoints[i] = transform.GetChild(i).gameObject;
		}

		length = spawnPoints.Length;
	}

	// Update is called once per frame
	void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			SpawnEnemy();
		}
	}

	void SpawnEnemy()
	{
		timer = 5f;
		randomSpawnPoint = Random.Range(0, spawnPoints.Length);
		Instantiate<GameObject>(enemy, spawnPoints[randomSpawnPoint].transform.position + new Vector3(Random.Range(-5,5), spawnPoints[randomSpawnPoint].transform.position.y, Random.Range(-5, 5)), spawnPoints[randomSpawnPoint].transform.rotation, null);
	}
}
