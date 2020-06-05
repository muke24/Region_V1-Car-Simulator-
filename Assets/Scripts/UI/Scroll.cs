using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
	public GameObject allSettings;
	public Scrollbar scrollbar;

	public Vector3 scroll;
	public float scrollY;

	public float scrollSensitivity = 0.1f;
	public int scrollMaxValue = 1000;

	void Update()
	{
		scroll = new Vector3(0, scrollY, 0);

		if (scrollbar.value >= 0 && scrollbar.value <= 1)
		{
			if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
			{
				scrollbar.value += scrollSensitivity;
			}

			if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
			{
				scrollbar.value -= scrollSensitivity;
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
		allSettings.transform.localPosition = scroll * scrollMaxValue;
		scrollY = scrollbar.value;
	}
}
