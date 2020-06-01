using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPost : MonoBehaviour
{
    [SerializeField]
    private Material lightMat;

    private Light spotLight;

    // Start is called before the first frame update
    void Start()
    {
        spotLight = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
