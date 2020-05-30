﻿#region This code is written by Peter Thompson
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text speedText;
	public Text fpsText;
	public static float carSpeed;

	public Text ammoText;

	[SerializeField]
	private Sniper sniper = null;

	public void ChangeText(float speed)
	{
		float s = speed * 3.6f;
		carSpeed = Mathf.Round(s);
		speedText.text = carSpeed + " KM/H";
	}

	private void Update()
	{
		fpsText.text = "FPS: " + (Mathf.Round(1/Time.smoothDeltaTime)).ToString();
		if (sniper != null)
		{
			AmmoCheck();
		}		
	}

	public void AmmoCheck()
	{
		if (sniper.gameObject)
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
	}
}
// This code is written by Peter Thompson
#endregion