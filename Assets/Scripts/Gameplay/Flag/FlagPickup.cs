using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPickup : MonoBehaviour
{
    private GameObject player;
    private CurrentWeapon currentWeapon;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentWeapon = player.GetComponent<CurrentWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 2)
        {
            currentWeapon.currentWeapon = currentWeapon.flagPistol;
            currentWeapon.changeWeapon = true;
            gameObject.SetActive(false);
        }
    }
}
