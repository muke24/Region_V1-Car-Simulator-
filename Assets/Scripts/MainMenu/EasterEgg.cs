#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
	public bool Button1;
	public bool Button2;
	public bool Button3;
	public bool Button4;

	private Vector3 mOffset;
	private float mZCoord;

	public void Button1Clicked()
	{
		Button1 = true;
	}

	public void Button2Clicked()
	{
		Button2 = true;
	}

	public void Button3Clicked()
	{
		Button3 = true;
	}

	public void Button4Clicked()
	{
		Button4 = true;
	}

	private void OnMouseDown()
	{
		if (Button1 && Button2 && Button3 && Button4)
		{
			GetComponent<Rigidbody>().isKinematic = false;

			mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
			mOffset = gameObject.transform.position - GetMouseWorldPos();
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
		if (Button1 && Button2 && Button3 && Button4)
		{
			transform.position = GetMouseWorldPos() + mOffset;
		}		
	}
}
// This code is written by Peter Thompson
#endregion