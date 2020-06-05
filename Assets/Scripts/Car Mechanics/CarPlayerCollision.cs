using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlayerCollision : MonoBehaviour
{
	private Rigidbody rigid;

	// Start is called before the first frame update
	void Start()
	{
		rigid = GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (rigid.mass > 1f)
		{
			if (collision.transform.root.gameObject.tag == "Enemy")
			{
				Enemy enemy = collision.transform.root.GetComponent<Enemy>();
				enemy.curHealth -= rigid.velocity.magnitude * rigid.mass / 2;
			}
		}
	}
}
