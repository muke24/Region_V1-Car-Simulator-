using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBase : MonoBehaviour
{
    public CurrentWeapon currentWeapon;
    public GameObject sniper;
    public GameObject flag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon.currentWeapon == currentWeapon.flagPistol)
        {
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < 3)
            {
                currentWeapon.currentWeapon = 1;
                sniper.SetActive(true);
                flag.SetActive(false);
                //currentWeapon.animator.SetBool("Exit", false);
            }
        }
    }
}
