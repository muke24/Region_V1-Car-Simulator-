using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBase : MonoBehaviour
{
    public CurrentWeapon currentWeapon;
    public GameObject flag;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<CurrentWeapon>();
        flag = GameObject.FindGameObjectWithTag("Flag");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon.currentWeapon == currentWeapon.secondaryWeapon && currentWeapon.flag)
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 3)
            {
                currentWeapon.currentWeapon = currentWeapon.mainWeapon;
                currentWeapon.changeWeapon = true;
                currentWeapon.flag = false;
                flag.SetActive(true);
            }
        }
    }
}
