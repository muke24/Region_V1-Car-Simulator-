#region This code is written by Peter Thompson
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HostPlayerDrag : MonoBehaviour
{
	private GameObject selectedDragGameObject;
	private Transform firstParent;
	private Vector3 firstPos = Vector3.zero;
	private Vector3 mOffset;
	public GameObject lastDragParent = null;
	public GameObject dragParent = null;

	private bool playerOnSpot = false;

	public List<RaycastResult> RaycastMouse()
	{
		PointerEventData pointerData = new PointerEventData(EventSystem.current)
		{
			pointerId = -1,
		};

		pointerData.position = Input.mousePosition;

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerData, results);

		//Debug.Log(results.Count);

		return results;
	}

	private void FixedUpdate()
	{
		if (Input.GetMouseButton(0))
		{
			HighlightParentPanel();
		}
	}

	private void Update()
	{
		// I could of used Unity's "OnMouseDown()", "OnMouseDrag()" and "OnMouseUp()", 
		// but for some reason when I tried they weren't even being called for some reason. 

		if (GameMode.hosting || !GameMode.hosting && !GameMode.joining)
		{
			if (Input.GetMouseButtonDown(0))
			{
				MouseDown();
			}
			if (Input.GetMouseButton(0))
			{
				MouseDrag();
			}
			if (Input.GetMouseButtonUp(0))
			{
				MouseUp();
			}
		}
	}

	private void MouseDown()
	{
		var mousePos = Input.mousePosition;
		mousePos.x -= Screen.width / 2;
		mousePos.y -= Screen.height / 2;

		foreach (RaycastResult item in RaycastMouse())
		{
			if (item.gameObject.CompareTag("PlayerPanel"))
			{
				selectedDragGameObject = item.gameObject;
				firstPos = selectedDragGameObject.transform.localPosition;
				firstParent = selectedDragGameObject.transform.parent;
				mOffset = selectedDragGameObject.transform.position - mousePos;
				item.gameObject.GetComponent<Image>().raycastTarget = false;
				selectedDragGameObject.transform.SetParent(transform);
			}
		}
	}

	private void MouseDrag()
	{
		var mousePos = Input.mousePosition;
		mousePos.x -= Screen.width / 2;
		mousePos.y -= Screen.height / 2;

		if (selectedDragGameObject != null)
		{
			selectedDragGameObject.transform.position = mousePos + mOffset;
		}
	}

	private void HighlightParentPanel()
	{
		if (RaycastMouse().Count > 0)
		{
			foreach (RaycastResult item in RaycastMouse())
			{
				if (selectedDragGameObject != null)
				{
					if (item.gameObject.CompareTag("PlayerPanelParent"))
					{
						if (dragParent == lastDragParent)
						{
							dragParent = item.gameObject;
							lastDragParent = null;
						}

						if (dragParent != lastDragParent && dragParent != item.gameObject && lastDragParent != dragParent)
						{
							lastDragParent = dragParent;
							dragParent = item.gameObject;
							dragParent.GetComponent<Outline>().enabled = true;
						}
					}

					else
					{
						if (lastDragParent != null)
						{
							lastDragParent.GetComponent<Outline>().enabled = false;
						}
					}
				}
			}
		}
		else
		{
			if (dragParent != null)
			{
				dragParent.GetComponent<Outline>().enabled = false;
				dragParent = null;
			}

			if (lastDragParent != null)
			{
				lastDragParent.GetComponent<Outline>().enabled = false;
				lastDragParent = null;
			}
		}
	}

	private void MouseUp()
	{
		if (RaycastMouse().Count == 0)
		{
			selectedDragGameObject.transform.SetParent(firstParent);
			selectedDragGameObject.transform.localPosition = firstPos;
			selectedDragGameObject.GetComponent<Image>().raycastTarget = true;

			firstPos = Vector3.zero;
			firstParent = null;
			selectedDragGameObject = null;
			mOffset = Vector3.zero;
		}

		else
		{
			foreach (RaycastResult item in RaycastMouse())
			{
				if (selectedDragGameObject != null)
				{
					if (dragParent != null)
					{
						dragParent.GetComponent<Outline>().enabled = false;
						dragParent = null;
					}

					if (lastDragParent != null)
					{
						lastDragParent.GetComponent<Outline>().enabled = false;
						lastDragParent = null;
					}

					if (item.gameObject.CompareTag("PlayerPanel") && item.gameObject != selectedDragGameObject)
					{
						playerOnSpot = true;
						Debug.Log("Another player is on this spot already");
					}

					if (!item.gameObject.CompareTag("PlayerPanelParent") && !playerOnSpot)
					{
						selectedDragGameObject.transform.SetParent(firstParent);
						selectedDragGameObject.transform.localPosition = firstPos;
						selectedDragGameObject.GetComponent<Image>().raycastTarget = true;

						firstPos = Vector3.zero;
						firstParent = null;
						selectedDragGameObject = null;
						mOffset = Vector3.zero;
						return;
					}

					if (item.gameObject.CompareTag("PlayerPanelParent") && !playerOnSpot)
					{
						selectedDragGameObject.transform.SetParent(item.gameObject.transform);
						selectedDragGameObject.transform.localPosition = Vector3.zero;
						selectedDragGameObject.GetComponent<Image>().raycastTarget = true;

						firstPos = Vector3.zero;
						firstParent = null;
						selectedDragGameObject = null;
						mOffset = Vector3.zero;
						return;
					}

					if (playerOnSpot)
					{
						selectedDragGameObject.transform.SetParent(firstParent);
						selectedDragGameObject.transform.localPosition = firstPos;
						selectedDragGameObject.GetComponent<Image>().raycastTarget = true;

						firstPos = Vector3.zero;
						firstParent = null;
						selectedDragGameObject = null;
						mOffset = Vector3.zero;
						playerOnSpot = false;
						return;
					}
				}
			}
		}
	}
}
// This code is written by Peter Thompson
#endregion
