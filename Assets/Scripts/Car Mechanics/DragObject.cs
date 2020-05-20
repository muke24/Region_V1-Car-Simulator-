using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
	public GameObject paused;
	public GameObject inCar;

	private Vector3 mOffset;
	private float mZCoord;

	private void OnMouseDown()
	{
		if (paused.activeSelf == false)
		{
			if (inCar.activeSelf == true)
			{
				mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
				mOffset = gameObject.transform.position - GetMouseWorldPos();
			}
		}
	}

	private Vector3 GetMouseWorldPos()
	{
		// Pixel Coordinates x,y
		Vector3 mousePoint = Input.mousePosition;

		// z coordinate of game object on screen
		mousePoint.z = mZCoord;

		return Camera.main.ScreenToWorldPoint(mousePoint);
	}

	private void OnMouseDrag()
	{
		if (paused.activeSelf == false)
		{
			if (inCar.activeSelf == true)
			{
				transform.position = GetMouseWorldPos() + mOffset;
			}
		}
	}
}
