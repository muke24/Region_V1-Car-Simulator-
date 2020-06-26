using Boo.Lang;
using UnityEngine;
using UnityEngine.UI;

public class Errors : MonoBehaviour
{
	private static Errors instance;

	public static Object errorObject;
	public static GameObject warningPanel;  // Warning panel
	public static Text errorCodeText;       // Text that shows error code
	public static Text errorText;           // Text that shows error reason

	public static bool error000 = false;
	public static bool error001 = false;
	public static bool error002 = false;
	public static bool error003 = false;
	public static bool error004 = false;
	public static bool error005 = false;
	public static bool error006 = false;

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
			Instantiate(Resources.Load("Error Panel/WarningPanel") as GameObject, GameObject.FindGameObjectWithTag("MainCanvas").transform);
			GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel(Clone)").name = "WarningPanel";
			errorCodeText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorCodeText").GetComponent<Text>();
			errorText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorMessage").GetComponent<Text>();

			errorCodeText.text = "Error:000";
			errorText.text = "Unknown Error";
			Debug.LogError("Error:000 - Unknown Error");
			error001 = true;
		}
	}
	public static void Error001()       // Internet connection error, internet connection lost
	{
		if (error001 == false)
		{
			Instantiate(Resources.Load("Error Panel/WarningPanel") as GameObject, GameObject.FindGameObjectWithTag("MainCanvas").transform);
			GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel(Clone)").name = "WarningPanel";
			errorCodeText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorCodeText").GetComponent<Text>();
			errorText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorMessage").GetComponent<Text>();

			errorCodeText.text = "Error:001";
			errorText.text = "Internet connection lost";
			Debug.LogError("Error:001 - Internet connection lost");
			error001 = true;
		}
	}
	public static void Error002()       // Server connection error, could not find server with host name
	{
		if (error002 == false)
		{
			Instantiate(Resources.Load("Error Panel/WarningPanel") as GameObject, GameObject.FindGameObjectWithTag("MainCanvas").transform);
			GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel(Clone)").name = "WarningPanel";
			errorCodeText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorCodeText").GetComponent<Text>();
			errorText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorMessage").GetComponent<Text>();

			errorCodeText.text = "Error:002";
			errorText.text = "Could not find server with host name";
			Debug.LogError("Error:002 - Server connection error, could not find server with host name");
			error002 = true;
		}
	}
	public static void Error003()       // Application, integrity error
	{
		if (error003 == false)
		{
			Instantiate(Resources.Load("Error Panel/WarningPanel") as GameObject, GameObject.FindGameObjectWithTag("MainCanvas").transform);
			GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel(Clone)").name = "WarningPanel";
			errorCodeText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorCodeText").GetComponent<Text>();
			errorText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorMessage").GetComponent<Text>();

			errorCodeText.text = "Error:003";
			errorText.text = "Detected game is not genuine, or has been tampered with.";
			Debug.LogError("Error:003 - Game is not genuine, or has been tampered with");
			error003 = true;

			UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.FatalError);
		}
	}
	public static void Error004()       // Application, integrity check error
	{
		if (error004 == false)
		{
			Instantiate(Resources.Load("Error Panel/WarningPanel") as GameObject, GameObject.FindGameObjectWithTag("MainCanvas").transform);
			GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel(Clone)").name = "WarningPanel";
			errorCodeText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorCodeText").GetComponent<Text>();
			errorText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorMessage").GetComponent<Text>();

			errorCodeText.text = "Error:004";
			errorText.text = "Cannot detect game integrity, multiplayer services will be disabled. Please check if you have updated Region to the latest version available.";
			Debug.LogWarning("Error:004 - Cannot detect game integrity, multiplayer services will be disabled.");
			error004 = true;

			GameMode.genuineGame = false;
		}
	}
	public static void Error005()       // Unknown, unknown error
	{
		if (error005 == false)
		{
			Instantiate(Resources.Load("Error Panel/WarningPanel") as GameObject, GameObject.FindGameObjectWithTag("MainCanvas").transform);
			GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel(Clone)").name = "WarningPanel";
			errorCodeText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorCodeText").GetComponent<Text>();
			errorText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorMessage").GetComponent<Text>();

			errorCodeText.text = "Error:005";
			errorText.text = "Unknown Error";
			Debug.LogWarning("Error:005 - Unknown Error");
			error005 = true;
		}
	}
	public static void Error006()       // Unknown, unknown error
	{
		if (error006 == false)
		{
			Instantiate(Resources.Load("Error Panel/WarningPanel") as GameObject, GameObject.FindGameObjectWithTag("MainCanvas").transform);
			GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel(Clone)").name = "WarningPanel";
			errorCodeText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorCodeText").GetComponent<Text>();
			errorText = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("WarningPanel").transform.Find("ErrorMessage").GetComponent<Text>();

			errorCodeText.text = "Error:006";
			errorText.text = "Unknown Error";
			Debug.LogWarning("Error:006 - Unknown Error");
			error006 = true;
		}
	}
}
