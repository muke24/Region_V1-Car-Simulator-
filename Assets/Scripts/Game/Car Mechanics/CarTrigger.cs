using UnityEngine;

public class CarTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarFind.inCarTrigger = true;
            print("Player has entered a car trigger");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (CarFind.inCarTrigger == false)
        {
            if (other.CompareTag("Player"))
            {
                CarFind.inCarTrigger = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!Car.inCar)
        {
            if (other.CompareTag("Player"))
            {
                CarFind.inCarTrigger = false;
                print("Player has exited a car trigger");
            }
        }
    }
}
