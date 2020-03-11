using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
	public Button applyButton;

	//public Slider volumeSlider;
	public Slider resolutionSlider;

	//public Text volumeText;
	public Text resolutionText;
	public Text resolutionSize;

	public string resText;

	//public int volumeInt;
	public int resolutionIntWidth;
	public int resolutionIntHeight;

	public int resWidth;
	public int resHeight;

	public bool resChanged;
	public bool settingsChanged;

	// Start is called before the first frame update
	void Start()
	{
		resChanged = false;
		settingsChanged = false;
		resWidth = Screen.width;
		resHeight = Screen.height;
	}

	// Update is called once per frame
	void Update()
	{
		if (settingsChanged == false)
		{
			applyButton.gameObject.SetActive(false);
		}

		if (settingsChanged == true)
		{
			applyButton.gameObject.SetActive(true);
		}

		resolutionIntWidth = Mathf.RoundToInt(resWidth * resolutionSlider.value);
		resolutionIntHeight = Mathf.RoundToInt(resHeight * resolutionSlider.value);

		resolutionText.text = resText;
		resText = resolutionSlider.value.ToString();
		if (resText.Length > 3)
		{
			resText = resText.Substring(0, 3);
		}

		//volumeInt = Mathf.RoundToInt(volumeSlider.value);
		//volumeText.text = volumeSlider.value.ToString() + "%";


		resolutionSize.text = resolutionIntWidth.ToString() + " x " + resolutionIntHeight.ToString();
	}

	public void ApplySettings()
	{
		if (resChanged == true)
		{
			Screen.SetResolution(resolutionIntWidth, resolutionIntHeight, FullScreenMode.ExclusiveFullScreen);
			resChanged = false;
		}
		settingsChanged = false;
		//AudioListener.volume = volumeSlider.value;
	}
	public void ResolutionChanged()
	{
		resChanged = true;
	}
	public void SettingsHasChanged()
	{
		settingsChanged = true;
	}

}
