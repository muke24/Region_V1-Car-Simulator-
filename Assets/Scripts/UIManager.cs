using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text text;
	public Text fpsText;

	public virtual void ChangeText(float speed)
	{
		float s = speed * 3.6f;
		text.text = Mathf.Round(s) + " KM/H";
	}

	private void Update()
	{
		fpsText.text = "FPS: " + (Mathf.Round(1f / Time.deltaTime)).ToString();
	}
}
