using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
	public List<Light> lights;
	public List<Light> tailLights;


	public virtual void ToggleHeadlights()
	{
		foreach (Light light in lights)
		{
			light.intensity = light.intensity == 0 ? 15 : 0;
			Debug.Log("Headlights Toggled");
		}

	}

	public virtual void ToggleBrakeLightsOn()
	{
		foreach (Light bl in tailLights)
		{
			bl.intensity = bl.intensity = 0.75f;
		}
	}
	public virtual void ToggleBrakeLightsOff()
	{
		foreach (Light bl in tailLights)
		{
			bl.intensity = bl.intensity = 0;
		}
	}
}
