using UnityEngine;

public class CarTrigger : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Car.inCar)
        {
            if (other.gameObject == player && !CarFind.inCarTrigger)
            {
                CarFind.inCarTrigger = true;
                print("Player has entered a car trigger (OnTriggerEnter)");
            }
        }        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Car.inCar)
        {
            if (other.gameObject == player && !CarFind.inCarTrigger)
            {
                CarFind.inCarTrigger = true;
                print("Player has entered a car trigger (OnTriggerStay)");
            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!Car.inCar)
        {
            if (other.gameObject == player && CarFind.inCarTrigger)
            {
                CarFind.inCarTrigger = false;
                print("Player has exited a car trigger ()");
            }
        }
    }
}
