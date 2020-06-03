using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform target;
    public Animator animator;

    public float amount;
    public float maxAmount;
    public float smoothAmount;

    private Vector3 initialPosition;
    private bool finishedAnim = false;


    // Update is called once per frame
    void Update()
    {
        if (!finishedAnim)
        {
            transform.LookAt(target);
        }
        
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            animator.enabled = false;
            if (!finishedAnim)
            {
                initialPosition = transform.localPosition;
                finishedAnim = true;
            }
            
        }

        if (finishedAnim)
        {
            float movementX = -Input.GetAxis("Mouse X") * amount;
            float movementY = -Input.GetAxis("Mouse Y") * amount;

            movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
            movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

            Vector3 finalPosition = new Vector3(movementX, movementY, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
        }        
    }
}
