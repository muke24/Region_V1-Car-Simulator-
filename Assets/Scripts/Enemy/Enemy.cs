using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float maxHealth = 100;
	public float curHealth = 100;

	public bool ragdoll;

	// Start is called before the first frame update
	void Start()
	{
		curHealth = maxHealth;
	}

	// Update is called once per frame
	void Update()
	{
		if (curHealth <= 0f)
		{
			this.gameObject.SetActive(false);
		}
	}
}
