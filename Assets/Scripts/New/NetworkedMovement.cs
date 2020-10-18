using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkedMovement : NetworkBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerMaxSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private bool debug;

    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
		if (isLocalPlayer)
		{
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = cameraOffset;
            Camera.main.transform.rotation = Quaternion.identity;

            rigid = GetComponent<Rigidbody>();
		}
		else
		{

		}
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

		if (debug)
		{
            Camera.main.transform.localPosition = cameraOffset;
		}

		if (Input.GetAxis("Horizontal") != Mathf.Epsilon || Input.GetAxis("Vertical") != Mathf.Epsilon)
		{
			Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			if (rigid.velocity.magnitude < playerMaxSpeed)
                rigid.AddForce(movementDirection * playerSpeed * Time.deltaTime * 100);

        }
    }


}
