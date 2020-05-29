using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
	public Collider head;
	public Collider body;

	public Collider leftUpperArm;
	public Collider rightUpperArm;

	public Collider leftLowerArm;
	public Collider rightLowerArm;

	public Collider leftUpperLeg;
	public Collider rightUpperLeg;

	public Collider leftLowerLeg;
	public Collider rightLowerLeg;

	// Start is called before the first frame update
	void Start()
	{
		// Ignore head and body collisions ↓
		Physics.IgnoreCollision(head.GetComponent<Collider>(), body.GetComponent<Collider>());

		// Ignore upper left arm and body collisions ↓
		Physics.IgnoreCollision(leftUpperArm.GetComponent<Collider>(), body.GetComponent<Collider>());

		// Ignore upper right arm and body collisions ↓ 
		Physics.IgnoreCollision(rightUpperArm.GetComponent<Collider>(), body.GetComponent<Collider>());

		// Ignore upper left arm and lower left arm collisions ↓ 
		Physics.IgnoreCollision(leftUpperArm.GetComponent<Collider>(), leftLowerArm.GetComponent<Collider>());

		// Ignore upper right arm and lower right arm collisions ↓
		Physics.IgnoreCollision(rightUpperArm.GetComponent<Collider>(), rightLowerArm.GetComponent<Collider>());

		// Ignore upper left leg and body collisions ↓
		Physics.IgnoreCollision(leftUpperLeg.GetComponent<Collider>(), body.GetComponent<Collider>());

		// Ignore upper right leg and body collisions ↓
		Physics.IgnoreCollision(rightUpperLeg.GetComponent<Collider>(), body.GetComponent<Collider>());

		// Ignore upper left leg and lower left leg collisions ↓ 
		Physics.IgnoreCollision(leftLowerLeg.GetComponent<Collider>(), leftUpperLeg.GetComponent<Collider>());

		// Ignore upper right leg and lower right leg collisions ↓ 
		Physics.IgnoreCollision(rightLowerLeg.GetComponent<Collider>(), rightUpperLeg.GetComponent<Collider>());
	}
	
	void Update()
	{
		
	}
}
