using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public static int maxCount = 12;
    public static int ammoCount = 12;
    public int imaxCount = 12;
    public int iammoCount = 12;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        imaxCount = maxCount;
        iammoCount = ammoCount;
    }

    void Shoot()
    {

    }
}
