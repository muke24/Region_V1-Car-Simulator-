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
	private GameObject lastDragParent = null;
	private GameObject dragParent = null;
	private bool playerOnSpot = false;

	private List<RaycastResult> RaycastMouse()
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

	public void HighlightParentPanel()
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

	public void MouseDown()
	{
		if (GameMode.hosting || !GameMode.hosting && !GameMode.joining)
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
	}

	public void MouseDrag()
	{
		if (GameMode.hosting || !GameMode.hosting && !GameMode.joining)
		{
			var mousePos = Input.mousePosition;
			mousePos.x -= Screen.width / 2;
			mousePos.y -= Screen.height / 2;

			if (selectedDragGameObject != null)
			{
				selectedDragGameObject.transform.position = mousePos + mOffset;
			}
		}		
	}

	public void MouseUp()
	{
		if (GameMode.hosting || !GameMode.hosting && !GameMode.joining)
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
							Debug.Log("Switching player '" + selectedDragGameObject.GetComponentInChildren<Text>().text + "' with player '" + item.gameObject.GetComponentInChildren<Text>().text + "'");
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
							Transform transform1 = item.gameObject.transform.parent;
							Transform transform2 = firstParent;

							selectedDragGameObject.transform.SetParent(transform1);
							selectedDragGameObject.transform.localPosition = firstPos;
							selectedDragGameObject.GetComponent<Image>().raycastTarget = true;

							item.gameObject.transform.SetParent(transform2);
							item.gameObject.transform.localPosition = firstPos;

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
}
// This code is written by Peter Thompson
#endregion
