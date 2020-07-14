using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObjectCollision : MonoBehaviour
{
	private Rigidbody rigid;

	// Start is called before the first frame update
	void Start()
	{
		rigid = GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.rigidbody)
		{
			float otherMass = collision.rigidbody.mass;

			float force = collision.relativeVelocity.magnitude * otherMass;

			if (force > 400f)
			{
				rigid.isKinematic = false;
				rigid.AddForce(collision.relativeVelocity * 5, ForceMode.Force);

				gameObject.isStatic = false;

				Destroy(this, 0.1f);
			}
		}
	}
}
