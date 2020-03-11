using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
	public GameObject graphicsSettings;
	public Scrollbar scrollbar;

	public Vector3 scroll;
	public float scrollY;

	void Update()
	{
		scroll = new Vector3(0, scrollY, 0);
		
	}

	public void ScrollToPosition()
	{
		graphicsSettings.transform.localPosition = scroll * 1000;
		scrollY = scrollbar.value;
	}
}
