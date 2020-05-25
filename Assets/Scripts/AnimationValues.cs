using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationValues : MonoBehaviour
{
	[SerializeField]
	private PlayerAnimations pA;

	public bool moving;
	public bool movingLeft;
	public bool movingRight;
	public bool aim;
	public bool jump;
	public bool falling;
	public bool isGrounded;
	public float moveSpeed;
	public int ammoCount;
	public bool shoot;
	public bool bolt;
	public bool reload;
	public float boltActionFloat;

	// Update is called once per frame
	void Update()
	{
		moving = pA.playerAnimation.GetBool("Moving");
		movingLeft = pA.playerAnimation.GetBool("MovingLeft");
		movingRight = pA.playerAnimation.GetBool("MovingRight");
		aim = pA.playerAnimation.GetBool("Aim");
		jump = pA.playerAnimation.GetBool("Jump");
		falling = pA.playerAnimation.GetBool("Falling");
		isGrounded = pA.playerAnimation.GetBool("IsGrounded");
		moveSpeed = pA.playerAnimation.GetFloat("MoveSpeed");
		shoot = pA.playerAnimation.GetBool("Shoot");
		bolt = pA.playerAnimation.GetBool("Bolt");
		reload = pA.playerAnimation.GetBool("Reload");
		boltActionFloat = pA.playerAnimation.GetFloat("BoltActionFloat");
	}
}
