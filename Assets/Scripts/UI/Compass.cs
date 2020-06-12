using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
	public RawImage compassScrollTexture;
	public Transform playerPositionInWorld;

	private void Start()
	{
		playerPositionInWorld = GameObject.FindGameObjectWithTag("Player").transform;
	}

	// Update is called once per frame
	void Update()
	{
		compassScrollTexture.uvRect = new Rect(playerPositionInWorld.localEulerAngles.y / 360f, 0, 1, 1);
	}
}
