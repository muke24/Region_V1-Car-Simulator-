#region This code is written by Peter Thompson
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text speedText;
	public Text fpsText;
	public static float carSpeed;
	public CurrentWeapon currentWeapon;

	public Text ammoText;

	private float fpsTimer = 0.2f;
	private WaitForSeconds waitForSeconds = new WaitForSeconds(0.3f);

	private void Start()
	{
		currentWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<CurrentWeapon>();
		StartCoroutine(DoCheck());
	}

	public void ChangeText(float speed)
	{
		float s = speed * 3.6f;
		carSpeed = Mathf.Round(s);
		speedText.text = carSpeed + " KM/H";
	}

	private void Update()
	{
		if (fpsTimer > 0)
		{
			fpsTimer -= Time.unscaledDeltaTime;
		}
		else
		{
			fpsText.text = "FPS: " + Mathf.Round(1 / Time.smoothDeltaTime).ToString();
			fpsTimer = 0.2f;
		}
	}

	IEnumerator DoCheck()
	{
		while (true)
		{
			AmmoCheck();
			yield return waitForSeconds;
		}
	}

	public void AmmoCheck()
	{
		if (currentWeapon.currentWeapon == 1)
		{
			if (!GameConsole._cheat1)
			{
				ammoText.text = Sniper.ammoCount.ToString() + " / " + Sniper.maxAmmo.ToString();
			}
			if (GameConsole._cheat1)
			{
				ammoText.text = "∞ / ∞";
			}
		}

		if (currentWeapon.currentWeapon == 2)
		{

		}

		if (currentWeapon.currentWeapon == 3)
		{

		}
	}
}
// This code is written by Peter Thompson
#endregion