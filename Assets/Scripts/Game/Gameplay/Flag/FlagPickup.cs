using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPickup : MonoBehaviour
{
    public GameObject player;
    public CurrentWeapon currentWeapon;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentWeapon = player.GetComponent<CurrentWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            currentWeapon = player.GetComponent<CurrentWeapon>();
        }
        if (player != null)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 2)
            {
                currentWeapon.currentWeapon = currentWeapon.secondaryWeapon;
                currentWeapon.flag = true;
                currentWeapon.changeWeapon = true;
                gameObject.SetActive(false);
            }
        }        
    }
}
