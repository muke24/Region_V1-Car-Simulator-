using UnityEngine;

public class DestroyNetworkManager : MonoBehaviour
{
	void Awake()
	{
		if (GameMode.singleplayer || !GameMode.singleplayer && !GameMode.multiplayer)
		{
			if (FindObjectOfType<NetworkManagerLobby>() != null)
			{
				Destroy(FindObjectOfType<NetworkManagerLobby>().gameObject);
			}			
		}
	}
}
