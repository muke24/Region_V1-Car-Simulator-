using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenThroughWorld : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < -25)
        {
            player.transform.position = new Vector3(player.transform.position.x, 2, player.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.transform.position = new Vector3(player.transform.position.x, 2, player.transform.position.z);
        }
    }
}
