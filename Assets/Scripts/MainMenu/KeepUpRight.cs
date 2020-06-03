using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUpRight : MonoBehaviour
{

    public Quaternion rotation;

    private void Start()
    {
        rotation = transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = rotation;
    }
}
