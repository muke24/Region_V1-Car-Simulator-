using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNetPlayer : MonoBehaviour
{
	public GameObject playerPrefab;
	
	void CreateNetworkId()
	{
		playerPrefab.AddComponent<NetworkIdentity>();
	}
}
