using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null;
    [SerializeField] private MenuManager menuManager = null;

    [Header("UI")]
    //[SerializeField] private GameObject landingPagePanel = null;
    //[SerializeField] private InputField ipAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        networkManager.onClientConnected += HandleClientConnected;
        networkManager.onClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        networkManager.onClientConnected -= HandleClientConnected;
        networkManager.onClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        menuManager.JoinLocalHost();

        //string ipAddress = ipAddressInputField.text;
        string ipAddress = "localhost";

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;

        gameObject.SetActive(false);
        //landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
