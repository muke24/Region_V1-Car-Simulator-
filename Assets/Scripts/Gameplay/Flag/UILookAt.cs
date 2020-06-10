using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILookAt : MonoBehaviour
{
	public GameObject flag;
	public float scaleMultiplier = 2;

	// Update is called once per frame
	void Update()
	{
		GetComponent<Canvas>().transform.localScale = new Vector3 (Vector3.Distance(Camera.main.transform.position, GetComponent<Canvas>().transform.position) * scaleMultiplier, Vector3.Distance(Camera.main.transform.position, GetComponent<Canvas>().transform.position) * scaleMultiplier, Vector3.Distance(Camera.main.transform.position, GetComponent<Canvas>().transform.position) * scaleMultiplier);

		transform.position = new Vector3(flag.transform.position.x, flag.transform.position.y + 4f, flag.transform.position.z);

		Vector3 v = Camera.main.transform.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt(Camera.main.transform.position - v);
		transform.Rotate(0, 180, 0);

		if (!flag.activeSelf)
		{
			GetComponentInChildren<Image>().enabled = false;
		}
	}
}
