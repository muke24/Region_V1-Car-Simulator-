using Boo.Lang;
using UnityEngine;
using UnityEngine.UI;

public class Errors : MonoBehaviour
{
	private static Errors instance;

	public static Object errorObject;
	public static GameObject warningPanel;  // Warning panel
	public static GameObject errorCodeText;       // Text that shows error code
	public static GameObject errorText;           // Text that shows error reason

	public static bool error000 = false;
	public static bool error001 = false;
	public static bool error002 = false;
	public static bool error003 = false;
	public static bool error004 = false;
	public static bool error005 = false;

	// Singleton
	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public static void Error000()       // Unknown, unknown error
	{
		if (error000 == false)
		{
			Instantiate(errorObject);

			errorCodeText.GetComponent<Text>().text = "Error:000";
			errorText.GetComponent<Text>().text = "Unknown Error";
			error000 = true;
		}
	}
	public static void Error001()       // Internet connection error, internet connection lost
	{
		if (error001 == false)
		{
			Instantiate(Resources.Load("Error Panel/WarningPanel") as GameObject, GameObject.FindGameObjectWithTag("MainCanvas").transform);
			GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel(Clone)").name = "WarningPanel";
			errorCodeText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorCodeText").gameObject;
			errorText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorMessage").gameObject;

			errorCodeText.GetComponent<Text>().text = "Error:001";
			errorText.GetComponent<Text>().text = "Internet connection lost";
			Debug.Log("Error: Internet connection lost");
			error001 = true;
		}
	}
	public static void Error002()       // Internet connection error, could not find server with host name
	{
		Instantiate(warningPanel);

		errorCodeText.GetComponent<Text>().text = "Error:002";
		errorText.GetComponent<Text>().text = "Could not find server with host name";
	}
	public static void Error003()       // 
	{
		Instantiate(warningPanel);

		errorCodeText.GetComponent<Text>().text = "Error:003";
		errorText.GetComponent<Text>().text = "Unknown Error";
	}
	public static void Error004()       // 
	{
		Instantiate(warningPanel);

		errorCodeText.GetComponent<Text>().text = "Error:004";
		errorText.GetComponent<Text>().text = "Unknown Error";
	}
	public static void Error005()       // 
	{
		Instantiate(warningPanel);

		errorCodeText.GetComponent<Text>().text = "Error:005";
		errorText.GetComponent<Text>().text = "Unknown Error";
	}
	public static void Error006()       // 
	{
		Instantiate(warningPanel);

		errorCodeText.GetComponent<Text>().text = "Error:006";
		errorText.GetComponent<Text>().text = "Unknown Error";
	}
}
