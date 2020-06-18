using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManage : MonoBehaviour
{
	public static UIManage instance;

	public GameObject startMenu;
	public InputField usernameField;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Debug.Log("Instance already exists, destroying object!");
			Destroy(this);
		}
	}

	// Attempts to connect to the server
	public void ConnectToServer()
	{
		startMenu.SetActive(false);
		usernameField.interactable = false;
		Client.instance.ConnectToServer();
	}
}