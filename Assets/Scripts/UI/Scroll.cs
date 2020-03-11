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

		if (scrollbar.value >= 0 && scrollbar.value <= 1)
		{
			if (Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				scrollbar.value = scrollbar.value + 0.05f;
			}

			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				scrollbar.value = scrollbar.value - 0.05f;
			}
		}
		if (scrollbar.value < 0)
		{
			scrollbar.value = 0;
		}
		if (scrollbar.value > 1)
		{
			scrollbar.value = 1;
		}
	}

	public void ScrollToPosition()
	{

		graphicsSettings.transform.localPosition = scroll * 1000;
		scrollY = scrollbar.value;
	}
}
