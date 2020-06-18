#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public static bool inCar = false;

    public GameObject inCarPlayer;

    public GameObject door1;
    public GameObject door2 = null;
    public GameObject door3 = null;
    public GameObject door4 = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inCar)
        {
            inCarPlayer.SetActive(true);
        }
        if (!inCar)
        {
            inCarPlayer.SetActive(false);
        }
    }
}
// This code is written by Peter Thompson
#endregion