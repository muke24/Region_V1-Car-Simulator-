using Mirror;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private NetworkManagerLobby nml;

    // Start is called before the first frame update
    void Start()
    {
        if (GameMode.singleplayer || !GameMode.singleplayer && !GameMode.multiplayer)
        {
            foreach (var item in FindObjectsOfType<NetworkIdentity>())
            {
                Destroy(item);
            }
        }
    }
}
