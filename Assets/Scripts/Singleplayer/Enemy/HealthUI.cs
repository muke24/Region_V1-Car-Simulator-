using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject playerCam;
    public GameObject carCam;

    public float scaleMultiplier;

    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").gameObject;
        carCam = GameObject.FindGameObjectWithTag("CarCam");        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Car.inCar)
        {
            GetComponent<Canvas>().worldCamera = playerCam.GetComponent<Camera>();
            if (Vector3.Distance(playerCam.transform.position, GetComponent<Canvas>().transform.position) > 10)
            {
                GetComponent<Canvas>().transform.localScale = new Vector3(Vector3.Distance(playerCam.transform.position, GetComponent<Canvas>().transform.position) / 10 * scaleMultiplier, Vector3.Distance(playerCam.transform.position, GetComponent<Canvas>().transform.position) / 10 * scaleMultiplier, Vector3.Distance(playerCam.transform.position, GetComponent<Canvas>().transform.position) / 10 * scaleMultiplier);
            }
            else
            {
                GetComponent<Canvas>().transform.localScale = new Vector3(0.006f, 0.006f, 1f);
            }

            Vector3 v = playerCam.transform.position - transform.position;
            v.x = v.z = 0.0f;
            transform.LookAt(playerCam.transform.position - v);
            transform.Rotate(0, 180, 0);
        }

        if (Car.inCar)
        {
            GetComponent<Canvas>().worldCamera = carCam.GetComponent<Camera>();
            if (Vector3.Distance(carCam.transform.position, GetComponent<Canvas>().transform.position) > 10)
            {
                GetComponent<Canvas>().transform.localScale = new Vector3(Vector3.Distance(carCam.transform.position, GetComponent<Canvas>().transform.position) / 10 * scaleMultiplier, Vector3.Distance(carCam.transform.position, GetComponent<Canvas>().transform.position) / 10 * scaleMultiplier, Vector3.Distance(carCam.transform.position, GetComponent<Canvas>().transform.position) / 10 * scaleMultiplier);
            }
            else
            {
                GetComponent<Canvas>().transform.localScale = new Vector3(0.006f, 0.006f, 1f);
            }

            Vector3 v = carCam.transform.position - transform.position;
            v.x = v.z = 0.0f;
            transform.LookAt(carCam.transform.position - v);
            transform.Rotate(0, 180, 0);
        }
    }
}
