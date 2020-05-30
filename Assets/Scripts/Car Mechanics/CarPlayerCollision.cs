using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlayerCollision : MonoBehaviour
{
	public float highestVelocity;

	private Rigidbody rigid;
	
	// Start is called before the first frame update
	void Start()
	{
		rigid = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void FixedUpdate()
	{

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (UIManager.carSpeed > 10f)
		{
			if (collision.transform.root.gameObject.tag == "Enemy")
			{
				Enemy enemy = collision.transform.root.GetComponent<Enemy>();
				enemy.curHealth -= UIManager.carSpeed * rigid.mass;
			}
		}
	}
}
