#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class AllSettings : MonoBehaviour
{
	// This is so badly coded that I really didnt want to comment on this script. I should have used lists or arrays >:-/

	public Light[] allLights;
	public Light sunLight;
	[Space(10)]
	public GameObject rtrDropdown;
	public GameObject shadowDistAll;
	[Space(10)]
	public Dropdown shadowType;
	public Dropdown rtReflections;
	[Space(10)]
	public Button applyButton;
	[Space(10)]
	//public Slider volumeSlider;
	public Slider resolutionSlider;
	public Slider reflectSlider;
	public Slider shadowSlider;
	public Slider shadowDistanceSlider;
	public Slider mouseX;
	public Slider mouseY;
	public Slider adsMouseX;
	public Slider adsMouseY;
	[Space(10)]
	public Toggle fullscreenToggle;

	public Toggle postProcessingToggle;

	public Toggle colourGradingToggle;
	public Toggle autoExposureToggle;
	public Toggle depthOfFieldToggle;
	public Toggle motionBlurToggle;
	public Toggle screenSpaceReflectionsToggle;
	public Toggle ambientOcclusionToggle;
	public Toggle bloomToggle;

	[Space(10)]
	public InputField mXinput;
	public InputField mYinput;
	public InputField adsMXinput;
	public InputField adsMYinput;
	[Space(10)]
	public MouseLook mlX;
	public MouseLook mlY;
	[Space(10)]
	public ReflectionProbe ReflectionProbe1;
	[Space(10)]
	//public Text volumeText;
	public Text resolutionText;
	public Text resolutionSize;
	public Text reflectionValueText;
	public Text shadowValueText;
	public Text shadowDistanceText;
	[Space(10)]
	public InputField customReflect;
	public InputField customShadows;
	[Space(10)]
	public string resText;
	[Space(10)]
	//public int volumeInt;
	public int resolutionIntWidth;
	public int resolutionIntHeight;
	[Space(10)]
	public int resWidth;
	public int resHeight;
	[Space(10)]
	public bool resChanged;
	public bool settingsChanged;
	[Space(10)]
	public GameObject worldPostProcess;
	public GameObject weaponPostProcess;

	// Start is called before the first frame update
	void Start()
	{
		// Gets the x axis and y axis mouseLook scripts thats attached to the player in the scene at the start
		mlX = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>();
		mlY = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").GetComponent<MouseLook>();

		// Gets the reflection probe attached to the player at the start
		ReflectionProbe1 = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").transform.Find("Reflection Probe").GetComponent<ReflectionProbe>();

		// Gets post process gameobjects thats attached to the player (I have no clue why I put it on the player, but I did, and I didnt have time to take it off of it)
		worldPostProcess = GameObject.FindGameObjectWithTag("Player").transform.Find("PostProcessingEffectsWorld").gameObject;
		weaponPostProcess = GameObject.FindGameObjectWithTag("Player").transform.Find("PostProcessingEffectsGun").gameObject;

		// Sets the resolution changed and settings changed bool to false, then sets the res width and height to the screen width and height at the start
		resChanged = false;
		settingsChanged = false;
		resWidth = Screen.width;
		resHeight = Screen.height;

		// Sets the texts to the slider values
		mXinput.text = mouseX.value.ToString();
		mYinput.text = mouseY.value.ToString();
		adsMXinput.text = adsMouseX.value.ToString();
		adsMYinput.text = adsMouseY.value.ToString();

		// Applies the default settings which are the set values on the sliders at the start
		ResolutionDrag();
		ReflectionDrag();
		Shadows();
		ShadowDistDrag();
		ApplySettings();
	}

	// Update is called once per frame
	void Update()
	{
		if (mlX == null)
		{
			// Gets the x axis and y axis mouseLook scripts thats attached to the player in the scene at the start
			mlX = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>();
			mlY = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").GetComponent<MouseLook>();

			// Gets the reflection probe attached to the player at the start
			ReflectionProbe1 = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").transform.Find("Reflection Probe").GetComponent<ReflectionProbe>();

			// Gets post process gameobjects thats attached to the player (I have no clue why I put it on the player, but I did, and I didnt have time to take it off of it)
			worldPostProcess = GameObject.FindGameObjectWithTag("Player").transform.Find("PostProcessingEffectsWorld").gameObject;
			weaponPostProcess = GameObject.FindGameObjectWithTag("Player").transform.Find("PostProcessingEffectsGun").gameObject;
		}

		// Hides the apply button if the settings have been applied
		if (settingsChanged == false)
		{
			applyButton.gameObject.SetActive(false);
		}

		// Hides the apply button if the settings have been applied
		if (settingsChanged == true)
		{
			applyButton.gameObject.SetActive(true);
		}

		// This would change the volume if there was any sounds playing in the game
		//volumeInt = Mathf.RoundToInt(volumeSlider.value);
		//volumeText.text = volumeSlider.value.ToString() + "%";
	}

	public void ApplySettings()
	{
		// Gets all the lights in the scene when the apply button has been pressed
		allLights = FindObjectsOfType<Light>();

		// If the res has been changed when the apply button has been pressed than set the resolution to the value set by the player on the slider
		if (resChanged)
		{
			Screen.SetResolution(resolutionIntWidth, resolutionIntHeight, fullscreenToggle.isOn);

			resChanged = false;
		}

		// If the settings changed bool is true and aply button pressed than apply the settings that are set from the sliders

		if (settingsChanged == true)
		{
			#region Reflections
			if (reflectSlider.value == 0)
			{
				ReflectionProbe1.enabled = false;
				reflectionValueText.text = "Off";
			}
			if (reflectSlider.value == 1)
			{
				ReflectionProbe1.enabled = true;
				ReflectionProbe1.resolution = 128;
				reflectionValueText.text = "Low";
			}
			if (reflectSlider.value == 2)
			{
				ReflectionProbe1.enabled = true;
				ReflectionProbe1.resolution = 512;
				reflectionValueText.text = "Medium";
			}
			if (reflectSlider.value == 3)
			{
				ReflectionProbe1.enabled = true;
				ReflectionProbe1.resolution = 2048;
				reflectionValueText.text = "High";
			}
			if (reflectSlider.value == 4)
			{
				ReflectionProbe1.enabled = true;
				ReflectionProbe1.resolution = int.Parse(customReflect.text);
				reflectionValueText.text = "Custom";
			}
			#endregion

			#region Realtime Reflections
			if (rtReflections.value == 0)
			{
				ReflectionProbe1.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
				ReflectionProbe1.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
			}
			if (rtReflections.value == 1)
			{
				ReflectionProbe1.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
				ReflectionProbe1.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
			}
			if (rtReflections.value == 2)
			{
				ReflectionProbe1.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
				ReflectionProbe1.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
			}
			#endregion

			#region Shadows
			if (shadowSlider.value == 0)
			{
				foreach (Light light in allLights)
				{
					light.shadowResolution = LightShadowResolution.FromQualitySettings;
					shadowValueText.text = "Auto";
				}
			}
			if (shadowSlider.value == 1)
			{
				foreach (Light light in allLights)
				{
					light.shadows = LightShadows.None;
					QualitySettings.shadows = 0;
					light.shadowCustomResolution = 128;
					shadowValueText.text = "None";
				}
			}
			if (shadowSlider.value == 2)
			{
				foreach (Light light in allLights)
				{
					light.shadowCustomResolution = 512;
					shadowValueText.text = "Low";
				}
			}
			if (shadowSlider.value == 3)
			{
				foreach (Light light in allLights)
				{
					light.shadowCustomResolution = 1024;
					shadowValueText.text = "Medium";
				}
			}
			if (shadowSlider.value == 4)
			{
				foreach (Light light in allLights)
				{
					light.shadowCustomResolution = 2048;
					shadowValueText.text = "High";
				}
			}
			if (shadowSlider.value == 5)
			{
				foreach (Light light in allLights)
				{
					light.shadowCustomResolution = 4096;
					shadowValueText.text = "Very High";
				}
			}
			if (shadowSlider.value == 6)
			{
				foreach (Light light in allLights)
				{
					light.shadowCustomResolution = int.Parse(customShadows.text);
					shadowValueText.text = "Custom";
				}
			}

			if (shadowType.value == 0)
			{
				foreach (Light light in allLights)
				{
					light.shadows = LightShadows.Hard;
				}
				sunLight.shadows = LightShadows.Soft;
			}

			if (shadowType.value == 1)
			{
				foreach (Light light in allLights)
				{
					light.shadows = LightShadows.Soft;
				}
				sunLight.shadows = LightShadows.Soft;
			}

			if (shadowType.value == 2)
			{
				foreach (Light light in allLights)
				{
					light.shadows = LightShadows.Hard;
				}
				sunLight.shadows = LightShadows.Hard;
			}
			#endregion

			#region Shadow Distance
			QualitySettings.shadowDistance = shadowDistanceSlider.value;
			QualitySettings.shadowProjection = ShadowProjection.CloseFit;
			#endregion

			#region Mouse Sensitivity
			mlX.sensitivityX = mouseX.value;
			mlY.sensitivityY = mouseY.value;
			mlX.sensitivityXads = adsMouseX.value;
			mlY.sensitivityYads = adsMouseY.value;
			#endregion

			// Sets the settings changed bool to false so the apply button disappears
			settingsChanged = false;
		}

		//AudioListener.volume = volumeSlider.value;
	}

	#region These are voids that will be used by the UI in the settings when they have been interacted with
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
			customReflect.gameObject.SetActive(false);
			reflectionValueText.gameObject.SetActive(true);
		}
		if (reflectSlider.value == 1)
		{
			reflectionValueText.text = "Low";
			rtrDropdown.SetActive(true);
			customReflect.gameObject.SetActive(false);
			reflectionValueText.gameObject.SetActive(true);
		}
		if (reflectSlider.value == 2)
		{
			reflectionValueText.text = "Medium";
			rtrDropdown.SetActive(true);
			customReflect.gameObject.SetActive(false);
			reflectionValueText.gameObject.SetActive(true);
		}
		if (reflectSlider.value == 3)
		{
			reflectionValueText.text = "High";
			rtrDropdown.SetActive(true);
			customReflect.gameObject.SetActive(false);
			reflectionValueText.gameObject.SetActive(true);
		}
		if (reflectSlider.value == 4)
		{
			reflectionValueText.text = "Custom";
			rtrDropdown.SetActive(true);
			customReflect.gameObject.SetActive(true);
			reflectionValueText.gameObject.SetActive(false);
		}
	}

	public void ShadowDistDrag()
	{
		shadowDistanceText.text = shadowDistanceSlider.value.ToString();
	}

	// Activates when the shadow slider is being moved
	public void Shadows()
	{
		if (shadowSlider.value == 0)
		{
			shadowValueText.text = "Auto";

			shadowValueText.gameObject.SetActive(true);
			customShadows.gameObject.SetActive(false);

			shadowDistAll.SetActive(true);
		}
		if (shadowSlider.value == 1)
		{
			shadowValueText.text = "None";

			shadowValueText.gameObject.SetActive(true);
			customShadows.gameObject.SetActive(false);

			shadowDistAll.SetActive(false);
		}
		if (shadowSlider.value == 2)
		{
			shadowValueText.text = "Low";

			shadowValueText.gameObject.SetActive(true);
			customShadows.gameObject.SetActive(false);

			shadowDistAll.SetActive(true);
		}
		if (shadowSlider.value == 3)
		{
			shadowValueText.text = "Medium";

			shadowValueText.gameObject.SetActive(true);
			customShadows.gameObject.SetActive(false);

			shadowDistAll.SetActive(true);
		}
		if (shadowSlider.value == 4)
		{
			shadowValueText.text = "High";

			shadowValueText.gameObject.SetActive(true);
			customShadows.gameObject.SetActive(false);

			shadowDistAll.SetActive(true);
		}
		if (shadowSlider.value == 5)
		{
			shadowValueText.text = "Very High";

			shadowValueText.gameObject.SetActive(true);
			customShadows.gameObject.SetActive(false);

			shadowDistAll.SetActive(true);
		}
		if (shadowSlider.value == 6)
		{
			shadowValueText.text = "Custom";

			shadowValueText.gameObject.SetActive(false);
			customShadows.gameObject.SetActive(true);

			shadowDistAll.SetActive(true);
		}
	}

	public void PostProcessingToggle()
	{
		if (!postProcessingToggle.isOn)
		{
			colourGradingToggle.interactable = false;
			autoExposureToggle.interactable = false;
			depthOfFieldToggle.interactable = false;
			motionBlurToggle.interactable = false;
			screenSpaceReflectionsToggle.interactable = false;
			ambientOcclusionToggle.interactable = false;
			bloomToggle.interactable = false;

			worldPostProcess.SetActive(false);
			weaponPostProcess.SetActive(false);
		}
		if (postProcessingToggle.isOn)
		{
			colourGradingToggle.interactable = true;
			autoExposureToggle.interactable = true;
			depthOfFieldToggle.interactable = true;
			motionBlurToggle.interactable = true;
			screenSpaceReflectionsToggle.interactable = true;
			ambientOcclusionToggle.interactable = true;
			bloomToggle.interactable = true;

			worldPostProcess.SetActive(true);
			weaponPostProcess.SetActive(true);
		}
	}

	public void ColourGradingToggle()
	{
		if (colourGradingToggle.isOn)
		{
			worldPostProcess.transform.Find("ColorGrading").gameObject.SetActive(true);
			weaponPostProcess.transform.Find("ColorGrading").gameObject.SetActive(true);
		}
		else
		{
			worldPostProcess.transform.Find("ColorGrading").gameObject.SetActive(false);
			weaponPostProcess.transform.Find("ColorGrading").gameObject.SetActive(false);
		}
	}

	public void AutoExposureToggle()
	{
		if (autoExposureToggle.isOn)
		{
			worldPostProcess.transform.Find("AutoExposure").gameObject.SetActive(true);
			weaponPostProcess.transform.Find("AutoExposure").gameObject.SetActive(true);
		}
		else
		{
			worldPostProcess.transform.Find("AutoExposure").gameObject.SetActive(false);
			weaponPostProcess.transform.Find("AutoExposure").gameObject.SetActive(false);
		}
	}

	public void DepthOfFieldToggle()
	{
		if (depthOfFieldToggle.isOn)
		{
			worldPostProcess.transform.Find("DepthOfField").gameObject.SetActive(true);
			weaponPostProcess.transform.Find("DepthOfField").gameObject.SetActive(true);
		}
		else
		{
			worldPostProcess.transform.Find("DepthOfField").gameObject.SetActive(false);
			weaponPostProcess.transform.Find("DepthOfField").gameObject.SetActive(false);
		}
	}

	public void MotionBlurToggle()
	{
		if (motionBlurToggle.isOn)
		{
			worldPostProcess.transform.Find("MotionBlur").gameObject.SetActive(true);
			weaponPostProcess.transform.Find("MotionBlur").gameObject.SetActive(true);
		}
		else
		{
			worldPostProcess.transform.Find("MotionBlur").gameObject.SetActive(false);
			weaponPostProcess.transform.Find("MotionBlur").gameObject.SetActive(false);
		}
	}

	public void ScreenSpaceReflectionsToggle()
	{
		if (screenSpaceReflectionsToggle.isOn)
		{
			worldPostProcess.transform.Find("ScreenSpaceReflections").gameObject.SetActive(true);
			weaponPostProcess.transform.Find("ScreenSpaceReflections").gameObject.SetActive(true);
		}
		else
		{
			worldPostProcess.transform.Find("ScreenSpaceReflections").gameObject.SetActive(false);
			weaponPostProcess.transform.Find("ScreenSpaceReflections").gameObject.SetActive(false);
		}
	}

	public void AmbientOcclusionToggle()
	{
		if (ambientOcclusionToggle.isOn)
		{
			worldPostProcess.transform.Find("AmbientOcclusion").gameObject.SetActive(true);
			weaponPostProcess.transform.Find("AmbientOcclusion").gameObject.SetActive(true);
		}
		else
		{
			worldPostProcess.transform.Find("AmbientOcclusion").gameObject.SetActive(false);
			weaponPostProcess.transform.Find("AmbientOcclusion").gameObject.SetActive(false);
		}
	}

	public void BloomToggle()
	{
		if (bloomToggle.isOn)
		{
			worldPostProcess.transform.Find("Bloom").gameObject.SetActive(true);
			weaponPostProcess.transform.Find("Bloom").gameObject.SetActive(true);
		}
		else
		{
			worldPostProcess.transform.Find("Bloom").gameObject.SetActive(false);
			weaponPostProcess.transform.Find("Bloom").gameObject.SetActive(false);
		}
	}

	#endregion
}

// This code is written by Peter Thompson
#endregion