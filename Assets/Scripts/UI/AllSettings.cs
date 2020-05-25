using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AllSettings : MonoBehaviour
{
	//
	//
	// CHANGE ALL QUALITY SETTINGS USING QualitySettings.SetQualityLevel()
	//
	//

	public List<Light> lights;

	public GameObject rtrDropdown;

	public Button applyButton;

	//public Slider volumeSlider;
	public Slider resolutionSlider;
	public Slider reflectSlider;
	public Slider shadowSlider;
	public Slider mouseX;
	public Slider mouseY;
	public Slider adsMouseX;
	public Slider adsMouseY;

	public InputField mXinput;
	public InputField mYinput;
	public InputField adsMXinput;
	public InputField adsMYinput;

	public MouseLook mlX;
	public MouseLook mlY;

	public Dropdown rtReflections;

	public ReflectionProbe ReflectionProbe1;
	public ReflectionProbe ReflectionProbe2;

	//public Text volumeText;
	public Text resolutionText;
	public Text resolutionSize;
	public Text reflectionValueText;
	public Text shadowValueText;

	public string resText;

	//public int volumeInt;
	public int resolutionIntWidth;
	public int resolutionIntHeight;

	public int resWidth;
	public int resHeight;

	public int realtimeReflections = 0;

	public bool start = true;
	public bool resChanged;
	public bool settingsChanged;
	public bool dragging = false;

	// Start is called before the first frame update
	void Start()
	{
		start = true;
		if (start)
		{
			ApplySettings();
		}

		resChanged = false;
		settingsChanged = false;
		resWidth = Screen.width;
		resHeight = Screen.height;

		DefaultSettings();

		mXinput.text = mouseX.value.ToString();
		mYinput.text = mouseY.value.ToString();
		adsMXinput.text = adsMouseX.value.ToString();
		adsMYinput.text = adsMouseY.value.ToString();

		ResolutionDrag();
		ReflectionDrag();
		Shadows();
		
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

		//volumeInt = Mathf.RoundToInt(volumeSlider.value);
		//volumeText.text = volumeSlider.value.ToString() + "%";
	}

	public void ApplySettings()
	{
		if (resChanged)
		{
			Screen.SetResolution(resolutionIntWidth, resolutionIntHeight, FullScreenMode.ExclusiveFullScreen);

			resChanged = false;
		}
		if (settingsChanged == true || start == true)
		{
			if (reflectSlider.value == 0)
			{
				ReflectionProbe1.enabled = false;
				ReflectionProbe2.enabled = false;
				reflectionValueText.text = "Off";
			}
			if (reflectSlider.value == 1)
			{
				ReflectionProbe1.enabled = true;
				ReflectionProbe2.enabled = true;
				ReflectionProbe1.resolution = 128;
				ReflectionProbe2.resolution = 128;
				reflectionValueText.text = "Low";
			}
			if (reflectSlider.value == 2)
			{
				ReflectionProbe1.enabled = true;
				ReflectionProbe2.enabled = true;
				ReflectionProbe1.resolution = 512;
				ReflectionProbe2.resolution = 512;
				reflectionValueText.text = "Medium";
			}
			if (reflectSlider.value == 3)
			{
				ReflectionProbe1.enabled = true;
				ReflectionProbe2.enabled = true;
				ReflectionProbe1.resolution = 2048;
				ReflectionProbe2.resolution = 2048;
				reflectionValueText.text = "High";
			}
			if (reflectSlider.value == 4)
			{
				ReflectionProbe1.enabled = true;
				ReflectionProbe2.enabled = true;
				reflectionValueText.text = "Custom";
			}

			if (rtReflections.value == 0)
			{
				ReflectionProbe1.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
				ReflectionProbe2.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
				ReflectionProbe1.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
				ReflectionProbe2.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
			}
			if (rtReflections.value == 1)
			{
				ReflectionProbe1.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
				ReflectionProbe2.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
				ReflectionProbe1.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
				ReflectionProbe2.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
			}
			if (rtReflections.value == 2)
			{
				ReflectionProbe1.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
				ReflectionProbe2.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
				ReflectionProbe1.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
				ReflectionProbe2.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
			}

			if (shadowSlider.value == 0)
			{
				foreach (Light light in lights)
				{
					light.shadowResolution = UnityEngine.Rendering.LightShadowResolution.FromQualitySettings;
					shadowValueText.text = "Auto";
				}
			}
			if (shadowSlider.value == 1)
			{
				foreach (Light light in lights)
				{
					light.shadows = LightShadows.None;
					light.shadowResolution = UnityEngine.Rendering.LightShadowResolution.Low;
					shadowValueText.text = "None";
				}
			}
			if (shadowSlider.value == 2)
			{
				foreach (Light light in lights)
				{
					light.shadows = LightShadows.Hard;
					light.shadowResolution = UnityEngine.Rendering.LightShadowResolution.Low;
					shadowValueText.text = "Low";
				}
			}
			if (shadowSlider.value == 3)
			{
				foreach (Light light in lights)
				{
					light.shadows = LightShadows.Hard;
					light.shadowResolution = UnityEngine.Rendering.LightShadowResolution.Medium;
					shadowValueText.text = "Medium";
				}
			}
			if (shadowSlider.value == 4)
			{
				foreach (Light light in lights)
				{
					light.shadows = LightShadows.Hard;
					light.shadowResolution = UnityEngine.Rendering.LightShadowResolution.High;
					shadowValueText.text = "High";
				}
			}
			if (shadowSlider.value == 5)
			{
				foreach (Light light in lights)
				{
					light.shadows = LightShadows.Soft;
					light.shadowResolution = UnityEngine.Rendering.LightShadowResolution.VeryHigh;
					shadowValueText.text = "Very High";
				}
			}

			mlX.sensitivityX = mouseX.value;
			mlY.sensitivityY = mouseY.value;
			mlX.sensitivityXads = adsMouseX.value;
			mlY.sensitivityYads = adsMouseY.value;

			start = false;
			settingsChanged = false;
		}

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

	public void SensOnDragX()
	{
		mXinput.text = mouseX.value.ToString();
	}

	public void SensOnDragY()
	{
		mYinput.text = mouseY.value.ToString();
	}

	public void ChangeInputFieldX()
	{
		mouseX.value = float.Parse(mXinput.text);
		if (float.Parse(mXinput.text) > 1000f)
		{
			mXinput.text = "1000";
		}
		if (float.Parse(mXinput.text) < 1f)
		{
			mXinput.text = "1";
		}
	}

	public void ChangeInputFieldY()
	{
		mouseY.value = float.Parse(mYinput.text);
		if (float.Parse(mYinput.text) > 1000f)
		{
			mYinput.text = "1000";
		}
		if (float.Parse(mYinput.text) < 1f)
		{
			mYinput.text = "1";
		}
	}

	public void ADSSensOnDragX()
	{
		adsMXinput.text = adsMouseX.value.ToString();
	}

	public void ADSSensOnDragY()
	{
		adsMYinput.text = adsMouseY.value.ToString();
	}

	public void ADSChangeInputFieldX()
	{
		adsMouseX.value = float.Parse(adsMXinput.text);
		if (float.Parse(adsMXinput.text) > 1000f)
		{
			adsMXinput.text = "1000";
		}
		if (float.Parse(adsMXinput.text) < 1f)
		{
			adsMXinput.text = "1";
		}
	}

	public void ADSChangeInputFieldY()
	{
		adsMouseY.value = float.Parse(adsMYinput.text);
		if (float.Parse(adsMYinput.text) > 1000f)
		{
			adsMYinput.text = "1000";
		}
		if (float.Parse(adsMYinput.text) < 1f)
		{
			adsMYinput.text = "1";
		}
	}

	public void ResolutionDrag()
	{
		resolutionIntWidth = Mathf.RoundToInt(resWidth * resolutionSlider.value);
		resolutionIntHeight = Mathf.RoundToInt(resHeight * resolutionSlider.value);

		resolutionText.text = resText;
		resText = resolutionSlider.value.ToString();

		if (resText.Length > 3)
		{
			resText = resText.Substring(0, 3);
		}

		resolutionSize.text = resolutionIntWidth.ToString() + " x " + resolutionIntHeight.ToString();
	}

	public void ReflectionDrag()
	{
		if (reflectSlider.value == 0)
		{
			reflectionValueText.text = "Off";

			rtrDropdown.SetActive(false);
		}
		if (reflectSlider.value == 1)
		{
			reflectionValueText.text = "Low";
			rtrDropdown.SetActive(true);
		}
		if (reflectSlider.value == 2)
		{
			reflectionValueText.text = "Medium";
			rtrDropdown.SetActive(true);
		}
		if (reflectSlider.value == 3)
		{
			reflectionValueText.text = "High";
			rtrDropdown.SetActive(true);
		}
		if (reflectSlider.value == 4)
		{
			reflectionValueText.text = "Custom";
			rtrDropdown.SetActive(true);
		}
	}

	public void Shadows()
	{
		if (shadowSlider.value == 0)
		{
			shadowValueText.text = "Auto";
		}
		if (shadowSlider.value == 1)
		{
			shadowValueText.text = "None";
		}
		if (shadowSlider.value == 2)
		{
			shadowValueText.text = "Low";
		}
		if (shadowSlider.value == 3)
		{
			shadowValueText.text = "Meduim";
		}
		if (shadowSlider.value == 4)
		{
			shadowValueText.text = "High";
		}
		if (shadowSlider.value == 5)
		{
			shadowValueText.text = "Very High";
		}		
	}

	void DefaultSettings()
	{
		rtReflections.value = 1;
		reflectSlider.value = 1;
		ReflectionProbe1.enabled = true;
		ReflectionProbe2.enabled = true;
		ReflectionProbe1.resolution = 128;
		ReflectionProbe2.resolution = 128;
		reflectionValueText.text = "Low";
	}
}
