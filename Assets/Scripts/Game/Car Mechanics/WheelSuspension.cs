using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSuspension : MonoBehaviour
{
	public GameObject[] wheels;
	public WheelCollider[] wheelColliders;

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			Quaternion rot;
			Vector3 pos;
			wheelColliders[i].GetWorldPose(out pos, out rot);

			wheels[i].transform.position = pos;
			wheels[i].transform.rotation = rot;
		}
	}
}
