#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionProbePosition : MonoBehaviour
{
    [SerializeField]
    private Interact interact;

    public Transform seat1Pos;
    public ReflectionProbe reflectionProbe;
    public GameObject carCam;
    public GameObject playerCam;

    // Start is called before the first frame update
    void Start()
    {
        interact = GetComponent<Interact>();
        reflectionProbe = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").transform.Find("Reflection Probe").GetComponent<ReflectionProbe>();
        playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (interact.inCar)
        {
            reflectionProbe.transform.parent = carCam.transform;
            reflectionProbe.transform.localPosition = Vector3.zero;
        }

        if (!interact.inCar)
        {
            reflectionProbe.transform.parent = playerCam.transform;
            reflectionProbe.transform.localPosition = Vector3.zero;
        }
    }
}
// This code is written by Peter Thompson
#endregion