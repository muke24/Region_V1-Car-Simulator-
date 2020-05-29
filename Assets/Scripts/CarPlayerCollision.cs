using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlayerCollision : MonoBehaviour
{
	public Enemy enemy;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (UIManager.carSpeed > 10f)
		{
			if (collision.transform.root.gameObject.tag == "Enemy")
			{
				enemy = collision.transform.root.GetComponent<Enemy>();
				enemy.ragdoll = true;
			}
		}
		
	}
}
