#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentCar : MonoBehaviour
{
    public GameObject currentCar = null;
    public CarFind carFind;

    private void Start()
    {
        carFind = GameObject.FindGameObjectWithTag("Player").GetComponent<CarFind>();
        currentCar = carFind.closestCar.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (carFind == null)
        {
            carFind = GameObject.FindGameObjectWithTag("Player").GetComponent<CarFind>();
            currentCar = carFind.closestCar.gameObject;
        }

        // Gets the current car from the closest car script. This script is placed on the manager gameobject as its always active
        currentCar = carFind.closestCar.gameObject;
    }
}
// This code is written by Peter Thompson
#endregion