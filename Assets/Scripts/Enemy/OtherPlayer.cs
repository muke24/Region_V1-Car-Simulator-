using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    public GameObject arms;
    public GameObject lookCam;
    public GameObject head;

    public float y;

    // Update is called once per frame
    void Update()
    {
        y = 0f + (lookCam.transform.localRotation.x / 10 * 3);

        head.transform.rotation = lookCam.transform.localRotation;
        if (lookCam.transform.localRotation.x > 0)
        {
            arms.transform.localPosition = new Vector3(0.14f, y, -0.075f);
        }
    }
}
