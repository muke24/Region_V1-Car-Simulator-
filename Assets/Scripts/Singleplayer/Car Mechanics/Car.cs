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

    public CurrentCar _currentCar;

    private void Awake()
    {
        _currentCar = GameObject.FindGameObjectWithTag("Manager").GetComponent<CurrentCar>();

        inCarPlayer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inCar)
        {
            _currentCar.currentCar.transform.Find("Seats").Find("Seat1").Find("DrivingPlayer").gameObject.SetActive(true);
            //inCarPlayer.SetActive(true);
        }
        if (!inCar)
        {
            _currentCar.currentCar.transform.Find("Seats").Find("Seat1").Find("DrivingPlayer").gameObject.SetActive(false);
            //inCarPlayer.SetActive(false);
        }
    }
}
// This code is written by Peter Thompson
#endregion