using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
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
