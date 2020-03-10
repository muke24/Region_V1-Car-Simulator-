using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollider : MonoBehaviour
{
	public Collider hood;
	public Collider bumper;
	// Start is called before the first frame update
	void Start()
    {
		Physics.IgnoreCollision(hood.GetComponent<Collider>(), bumper.GetComponent<Collider>());
	}

}
