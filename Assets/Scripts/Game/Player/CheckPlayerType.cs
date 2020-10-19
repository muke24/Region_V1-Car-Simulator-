using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CheckPlayerType : NetworkBehaviour
{
    [SerializeField]
    private GameObject networkPlayerModel;
    [SerializeField]
    private GameObject[] objectsToRemove = new GameObject[5];

    [Client]
    // Start is called before the first frame update
    void Awake()
    {
        if (!isLocalPlayer)
        {
			foreach (GameObject item in objectsToRemove)
			{
                item.SetActive(false);
			}
            networkPlayerModel.SetActive(true);
        }
		else
		{
            foreach (GameObject item in objectsToRemove)
            {
                item.SetActive(true);
            }
            networkPlayerModel.SetActive(false);
        }
    }
}
