using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teams : MonoBehaviour
{
    public float team;

    public GameObject spawn1;
    public GameObject spawn2;

    // Start is called before the first frame update
    void Awake()
    {
        if (!GameMode.singleplayer && !GameMode.multiplayer)
        {
            team = Random.Range(1, 3);
        }

        if (GameMode.singleplayer)
        {
            team = Random.Range(1, 3);
        }

        if (GameMode.multiplayer)
        {

        }
    }

    void PlayerPos()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
