using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public GameObject pause;

    public float amount;
    public float maxAmount;
    public float smoothAmount;
    //public MouseLook mouseLook;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the initial position variable to the first start position
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause.activeInHierarchy)
        {
            // If not aiming
            if (!Input.GetButton("Aim"))
            {
                float movementX = -Input.GetAxis("Mouse X") * amount;
                float movementY = -Input.GetAxis("Mouse Y") * amount;

                movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
                movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

                Vector3 finalPosition = new Vector3(movementX, movementY, 0);
                transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
            }

            // If aiming
            if (Input.GetButton("Aim"))
            {
                // Sets local float to 
                float movementX = -Input.GetAxis("Mouse X") * (amount * 0.3f);
                float movementY = -Input.GetAxis("Mouse Y") * (amount * 0.3f);

                movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
                movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

                Vector3 finalPosition = new Vector3(movementX, movementY, 0);
                transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
            }
        }        
    }
}
