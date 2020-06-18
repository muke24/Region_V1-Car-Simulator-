using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObjectCollision : MonoBehaviour
{
	private Rigidbody rigid;

	[SerializeField]
	private Light spotLight = null;
	[SerializeField]
	private GameObject halo = null;
	[SerializeField]
	private MeshRenderer lightObject = null;
	[SerializeField]
	private Material lightOff = null;

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

			var force = collision.relativeVelocity.magnitude * otherMass;

			if (force > 400f)
			{
				rigid.isKinematic = false;
				rigid.AddForce(collision.relativeVelocity * 5, ForceMode.Force);
				lightObject.material = lightOff;

				Destroy(spotLight);
				Destroy(halo);
				Destroy(this, 0.1f);
			}
		}
	}	
}
