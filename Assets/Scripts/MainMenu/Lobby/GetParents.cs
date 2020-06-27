using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetParents : MonoBehaviour
{
	public GameObject[] dragParents1;
	public GameObject[] dragParents2;
	public GameObject[] dragParentsAll;

	// Start is called before the first frame update
	void Start()
	{
		dragParents1 = new GameObject[GameObject.FindGameObjectsWithTag("PlayerPanelParent").Length / 2];
		dragParents2 = new GameObject[GameObject.FindGameObjectsWithTag("PlayerPanelParent").Length / 2];
		dragParentsAll = new GameObject[GameObject.FindGameObjectsWithTag("PlayerPanelParent").Length];

		for (int i = 0; i < dragParentsAll.Length; i++)
		{
			dragParentsAll[i] = GameObject.FindGameObjectsWithTag("PlayerPanelParent")[i];

			if (dragParentsAll[i].transform.parent.name == "Team1")
			{
				dragParents1[i] = dragParentsAll[i];
			}

			if (dragParentsAll[i].transform.parent.name == "Team2")
			{
				dragParents2[i - 5] = dragParentsAll[i];
			}
		}
	}
}
