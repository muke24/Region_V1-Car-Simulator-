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
		if (UIManager.carSpeed > 5)
		{
			if (rigid.mass > 10f)
			{
				if (collision.transform.root.gameObject.tag == "Enemy")
				{
					Enemy enemy = collision.transform.root.GetComponent<Enemy>();
					enemy.curHealth -= UIManager.carSpeed / 2;
				}
			}			
		}
		if (UIManager.carSpeed < -5)
		{
			if (rigid.mass > 10f)
			{
				if (collision.transform.root.gameObject.tag == "Enemy")
				{
					Enemy enemy = collision.transform.root.GetComponent<Enemy>();
					enemy.curHealth -= UIManager.carSpeed * -1 / 2;
				}
			}
		}
	}
}
