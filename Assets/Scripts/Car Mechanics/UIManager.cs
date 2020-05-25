using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text text;
	public Text fpsText;
	public Text ammoText;
	public Sniper sniper;

	private void Start()
	{
		
	}

	public virtual void ChangeText(float speed)
	{
		float s = speed * 3.6f;
		text.text = Mathf.Round(s) + " KM/H";
	}

	private void Update()
	{
		fpsText.text = "FPS: " + (Mathf.Round(1/Time.smoothDeltaTime)).ToString();
		AmmoCheck();
	}

	public void AmmoCheck()
	{
		if (sniper.gameObject)
		{
			ammoText.text = sniper.ammoCount.ToString() + " / " + sniper.maxAmmo.ToString();
		}
	}
}
