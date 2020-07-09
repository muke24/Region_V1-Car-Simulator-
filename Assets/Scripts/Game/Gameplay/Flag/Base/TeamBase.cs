using UnityEngine;

public class TeamBase : MonoBehaviour
{
	private Material teamMat;
	private GameObject flag;
	private GameObject player;
	private CurrentWeapon currentWeapon;
	private NetworkManagerLobby nml;

	// Start is called before the first frame update
	void Start()
	{
		if (!GameMode.captureTheFlag)
		{
			Destroy(gameObject);
		}

		if (GameMode.captureTheFlag)
		{
			teamMat = Resources.Load<Material>("Materials/Base/TeamBase");

			Renderer renderer = GetComponent<Renderer>();
			renderer.material = teamMat;

			flag = GameObject.FindGameObjectWithTag("Flag");
			if (GameMode.singleplayer)
			{
				player = GameObject.FindGameObjectWithTag("Player");
				currentWeapon = player.GetComponent<CurrentWeapon>();
			}
			if (GameMode.multiplayer)
			{
				nml = FindObjectOfType<NetworkManagerLobby>();
				player = nml.playerPrefab;
				currentWeapon = player.GetComponent<CurrentWeapon>();
			}
		}		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			currentWeapon.currentWeapon = currentWeapon.mainWeapon;
			currentWeapon.changeWeapon = true;
			currentWeapon.flag = false;
			flag.SetActive(true);
		}
	}	
}