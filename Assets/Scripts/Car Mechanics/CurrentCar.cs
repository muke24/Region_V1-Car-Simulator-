using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentCar : MonoBehaviour
{
    public GameObject currentCar = null;
    public CarFind carFind;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentCar = carFind.closestCar.gameObject;
    }
}
