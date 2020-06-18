#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
	public List<Light> lights;
	public List<Light> tailLights;

	public virtual void ToggleHeadlights()
	{
		// For each of the head lights, toggle on or off
		foreach (Light light in lights)
		{
			light.intensity = light.intensity == 0 ? 1 : 0;
			Debug.Log("Headlights Toggled");
		}
	}

	public virtual void ToggleBrakeLightsOn()
	{
		// For each of the brake lights, toggle on
		foreach (Light bl in tailLights)
		{
			bl.intensity = bl.intensity = 0.75f;
		}
	}

	public virtual void ToggleBrakeLightsOff()
	{
		// For each of the brake lights, toggle off
		foreach (Light bl in tailLights)
		{
			bl.intensity = bl.intensity = 0;
		}
	}
}
// This code is written by Peter Thompson
#endregion