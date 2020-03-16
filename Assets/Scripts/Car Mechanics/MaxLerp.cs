using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxLerp : MonoBehaviour
{
	public Vector3 firePos;

	void PullBackOrb(Vector3 mousePos)
	{
		if (Vector3.Distance(firePos, mousePos) > 3)
		{
			// find the mousePos offset from firePos:
			Vector3 offset = mousePos - firePos;
			// clamp the offset distance to 3:
			offset = Vector2.ClampMagnitude(offset, 3);
			// adjust mousePos to this distance:
			mousePos = firePos + offset;
		}
		Vector3 lerpPos = Vector2.Lerp(transform.position, mousePos, 10 * Time.deltaTime);
		transform.position = lerpPos;
	}
}
