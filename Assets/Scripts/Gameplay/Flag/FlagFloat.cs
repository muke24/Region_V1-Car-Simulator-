using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagFloat : MonoBehaviour
{
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float freq = 1f;

    Vector3 positionOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    void Start()
    {
        // Starting position of the flag
        positionOffset = transform.position;
    }

    void Update()
    {
        // Spins the flag around
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Makes the flag float up and down
        tempPos = positionOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * freq) * amplitude;

        transform.position = tempPos;
    }
}
