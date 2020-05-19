using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
	public float DayLength;

	[SerializeField]
	private float skySpeed = 1f;

    // Update is called once per frame
    void Update()
    {
		skySpeed = Time.deltaTime / DayLength;
		transform.Rotate(0, skySpeed, 0);
	}
}
