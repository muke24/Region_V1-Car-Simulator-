using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSettingsAtStart : MonoBehaviour
{
	private Resolution[] resolutions;
	private List<string> resStringW = new List<string>();
	private List<string> resStringH = new List<string>();
	private int resolutionCountW;
	private int resolutionCountH;
	private string maxResW;
	private string maxResH;

	private void Awake()
	{
		Errors.error000 = false;
		Errors.error001 = false;
		Errors.error002 = false;
		Errors.error003 = false;
		Errors.error004 = false;
		Errors.error005 = false;
		Errors.error006 = false;

		if (!Application.genuine)
		{
			Errors.Error003();
		}
		if (!Application.genuineCheckAvailable)
		{
			Errors.Error004();
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		// Shows all supported resolutions in the resolutions array
		resolutions = Screen.resolutions;

		foreach (Resolution res in resolutions)
		{
			// Add resolutions from array to the res lists
			resStringW.Add(res.width.ToString());
			resStringH.Add(res.height.ToString());
		}

		// Gets the total number of resolutions
		resolutionCountW = resStringW.Count;
		resolutionCountH = resStringH.Count;

		// Sets the maxRes strings to the total count of elements in the resString lists, 
		// then taking away one as the elements start at zero, and the count of all the elements start at one
		maxResW = resStringW[resolutionCountW - 1];
		maxResH = resStringH[resolutionCountH - 1];

		// Sets screen resolution to max supported resolution
		Screen.SetResolution(int.Parse(maxResW), int.Parse(maxResH), true);
		Debug.Log("Screen set to maximum supported resolution - " + maxResW + "x" + maxResH);
	}
}
