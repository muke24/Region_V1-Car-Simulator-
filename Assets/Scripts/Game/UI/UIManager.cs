#region This code is written by Peter Thompson
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text speedText;
	public Text fpsText;
	public static float carSpeed;
	public CurrentWeapon currentWeapon;

	public Text ammoText;

	[SerializeField]
	private Sniper sniper = null;

	private float fpsTimer = 0.2f;

	private void Start()
	{
		currentWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<CurrentWeapon>();
		sniper = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").Find("GunPivot").Find("ScopePivot").Find("L96_Black_Full").GetComponent<Sniper>();
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

	private void FixedUpdate()
	{
		if (sniper != null)
		{
			AmmoCheck();
		}
	}

	public void AmmoCheck()
	{
		if (currentWeapon.currentWeapon == 1)
		{
			if (!Console._cheat1)
			{
				ammoText.text = sniper.iammoCount.ToString() + " / " + sniper.imaxAmmo.ToString();
			}
			if (Console._cheat1)
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