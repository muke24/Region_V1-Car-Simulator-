#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Player : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;

    public MeshRenderer[] mesh;

    private void Awake()
    {
        mesh = FindObjectsOfType<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
	{
		health = maxHealth;
        foreach (MeshRenderer allMesh in mesh)
        {
            allMesh.probeAnchor = transform;            
        }
    }
}
// This code is written by Peter Thompson
#endregion