#region This code is written by Peter Thompson
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
	public Button logInButton;
	public Button registerButton;
	public Button logOutButton;
	public Button backButton;
	public Text typeText;
	public Text loggedInText;
	public Button loginButton;
	public Toggle rememberMe;
	public InputField usernameField;
	public InputField passwordField;
	public GameObject mainGO;
	public GameObject regLogGO;
	public bool loggedIn = false;

	public static string playerUsername { get; private set; }
	public static bool openedOnce { get; set; } = false;
	public int logInPanelInt = 0;
	public string username;

	private IEnumerator coroutine;
	private string playerPrefsNameKey = "PlayerName";

	private void Start()
	{
		bool lostConnection = false;
		OnStart(lostConnection);

		coroutine = CheckForInternet(5f, lostConnection);
		StartCoroutine(coroutine);
	}

	private void OnStart(bool lostConnection)
	{
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			SetUpInputField();

			if (usernameField.text != "")
			{
				logInButton.gameObject.SetActive(false);
				registerButton.gameObject.SetActive(false);
				logOutButton.gameObject.SetActive(true);

				loggedInText.text = "Logged In as " + usernameField.text;
			}
			else
			{
				LogOut();
			}
		}
		else
		{
			LostConnection();
		}
	}

	public void MainPanel()
	{
		logInPanelInt = 0;

		mainGO.SetActive(true);
		regLogGO.SetActive(false);

		if (usernameField.text != "")
		{
			loggedInText.text = "Logged In as " + usernameField.text;
		}
	}

	public void RegisterPanel()
	{
		logInPanelInt = 1;

		mainGO.SetActive(false);
		regLogGO.SetActive(true);
		typeText.text = "Register";
	}

	public void LogInPanel()
	{
		logInPanelInt = 2;

		mainGO.SetActive(false);
		regLogGO.SetActive(true);
		typeText.text = "Log In";
	}

	public void RegOrLogin()
	{
		if (usernameField.text.Length >= 3 && passwordField.text.Length >= 6 || usernameField.text.Length >= 3 && !passwordField.interactable)
		{
			if (logInPanelInt == 1)
			{
				// Register
				Register();
			}

			if (logInPanelInt == 2)
			{
				// Log In
				LogIn();
			}
		}
	}

	private void Register()
	{
		typeText.text = "Registering...";
	}

	private void LogIn()
	{
		typeText.text = "Logging In...";

		SavePlayerName();
	}

	private void SetUpInputField()
	{
		if (!PlayerPrefs.HasKey(playerPrefsNameKey))
		{
			loginButton.interactable = false;
			return;
		}

		string defaultName = PlayerPrefs.GetString(playerPrefsNameKey);

		usernameField.text = defaultName;

		SetPlayerName(defaultName);
	}

	public void SetPlayerName(string name)
	{
		if (usernameField.text.Length >= 3)
		{
			loginButton.interactable = true;
		}
		else
		{
			loginButton.interactable = false;
		}
	}

	private void SavePlayerName()
	{
		playerUsername = usernameField.text;

		if (rememberMe.isOn)
		{
			PlayerPrefs.SetString(playerPrefsNameKey, playerUsername);
		}
		if (!rememberMe.isOn)
		{
			PlayerPrefs.SetString(playerPrefsNameKey, "");
		}

		logInButton.gameObject.SetActive(false);
		registerButton.gameObject.SetActive(false);
		logOutButton.gameObject.SetActive(true);

		loggedIn = true;

		MainPanel();
	}

	public void LogOut()
	{
		loggedIn = false;
		PlayerPrefs.SetString(playerPrefsNameKey, "");
		usernameField.text = "";
		loggedInText.text = "Not logged in";

		logInButton.gameObject.SetActive(true);
		registerButton.gameObject.SetActive(true);
		logOutButton.gameObject.SetActive(false);
	}

	public void GoBack()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			SetUpInputField();

			logInPanelInt = 0;

			mainGO.SetActive(true);
			regLogGO.SetActive(false);

			logInButton.gameObject.SetActive(true);
			registerButton.gameObject.SetActive(true);
			logOutButton.gameObject.SetActive(false);
		}
		else
		{
			LostConnection();
		}
	}

	private void LostConnection()
	{
		loggedIn = false;

		logInPanelInt = 0;

		mainGO.SetActive(true);
		regLogGO.SetActive(false);

		logInButton.gameObject.SetActive(false);
		registerButton.gameObject.SetActive(false);
		logOutButton.gameObject.SetActive(false);

		loggedInText.text = "Connect to a network to log in";
		loggedInText.color = Color.red;

		Errors.Error001();
	}

	private void RegainConnection()
	{
		loggedIn = false;

		logInPanelInt = 0;

		mainGO.SetActive(true);
		regLogGO.SetActive(false);

		logInButton.gameObject.SetActive(true);
		registerButton.gameObject.SetActive(true);
		logOutButton.gameObject.SetActive(false);

		loggedInText.text = "Not logged in";
		loggedInText.color = Color.white;
	}

	IEnumerator CheckForInternet(float waitTime, bool lostConnection)
	{
		while (true)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				LostConnection();
				lostConnection = true;
				yield return lostConnection;
			}
			else
			{
				if (lostConnection)
				{
					RegainConnection();
				}
				lostConnection = false;
				yield return lostConnection;
			}

			yield return new WaitForSeconds(waitTime);
		}
	}
}
// This code is written by Peter Thompson
#endregion