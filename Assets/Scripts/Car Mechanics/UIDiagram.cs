using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDiagram : MonoBehaviour
{
	public GameObject door;
	public Text doorText;
	public JointLock currentDoor;
	public string doorTextStart;

	void Start()
	{
		doorTextStart = doorText.text;
		currentDoor = door.GetComponent<JointLock>();
	}

	void Update()
	{
		if (door.GetComponent<JointLock>().enabled)
		{
			if (door.GetComponent<JointLock>().lockState && door.GetComponent<JointLock>().enabled)
			{
				doorText.text = doorTextStart + "\n(Closed)";
			}

			if (!door.GetComponent<JointLock>().lockState && door.GetComponent<JointLock>().enabled)
			{
				doorText.text = doorTextStart + "\n(Opened)";
			}
			return;
		}

		if (!door.GetComponent<JointLock>().enabled)
		{
			doorText.text = doorTextStart + "\n(Broken)";
		}


		/*if (!door.GetComponent<JointLock>().lockState)
		{
			doorText.text = doorTextStart + "\n(Opened)";
		}
		*/
	}
}
