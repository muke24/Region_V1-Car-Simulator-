#region This code is written by Peter Thompson
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	private static Player instance;
	private NetworkManagerLobby nml;

	[Header("Player")]
	#region Player
	public float maxHealth = 100;
	public float curHealth = 100;
	public Slider healthBar;
	public Text healthText;
	public float healthRegenTimer = 10f;
	public bool wasShot = false;
	public bool healthIsRegening = false;
	#endregion

	[Header("Death")]
	#region Death
	public GameObject weapon;
	public GameObject oldWeapon;
	public Camera playerCam;
	public GameObject playerRagdoll;
	public bool isDead = false;
	public bool createPlayerRagdoll = false;
	public bool destroyPlayer = false;
	private GameObject playerGO;
	#endregion

	[Header("Networking")]
	#region Networking
	public int id;
	public string username;
	#endregion

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}

		if (GameMode.singleplayer)
		{
			username = "";
		}

		if (!GameMode.multiplayer)
		{
			Destroy(GetComponent<PlayerNetworkMovement>());
			Destroy(GetComponent<NetworkIdentity>());
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		curHealth = maxHealth;

		healthBar = Pause.healthBar;
		healthText = Pause.healthText;

		playerGO = gameObject;
	}

	private void Update()
	{
		if (curHealth < 0f)
		{
			curHealth = 0f;
		}

		healthBar.value = curHealth;
		healthText.text = "HP: " + curHealth.ToString();

		if (curHealth > 0)
		{
			if (wasShot)
			{
				healthRegenTimer -= Time.deltaTime;
				healthIsRegening = false;
			}
			if (wasShot && healthRegenTimer <= 0)
			{
				healthRegenTimer = 10f;
				wasShot = false;
				healthIsRegening = true;
			}

			if (healthIsRegening && curHealth < 100f)
			{
				curHealth += Time.deltaTime * 6f;
			}

			if (curHealth > 100f)
			{
				curHealth = 100f;
			}
			if (healthIsRegening && curHealth == 100)
			{
				healthIsRegening = false;
			}
		}
	}

	public void SetHealth(float _health)
	{
		curHealth = _health;

		if (curHealth <= 0f)
		{
			Die();
		}
	}

	public void Initialize(int _id, string _username)
	{
		if (GameMode.multiplayer)
		{
			id = _id;
			username = _username;
			curHealth = maxHealth;
		}
	}

	/// <summary>
	/// Kill's player
	/// </summary>
	public void Die()
	{
		if (curHealth == 0)
		{
			isDead = true;
			if (isDead)
			{
				createPlayerRagdoll = true;
				if (createPlayerRagdoll)
				{
					Instantiate(playerRagdoll, playerGO.transform.position, playerGO.transform.rotation);
					Instantiate(weapon, oldWeapon.transform.position, oldWeapon.transform.rotation);

					createPlayerRagdoll = false;
					destroyPlayer = true;
				}
			}

			if (destroyPlayer)
			{
				//Destroy(playerGO);
				playerGO.SetActive(false);
				destroyPlayer = false;
			}
		}
	}
}
// This code is written by Peter Thompson
#endregion