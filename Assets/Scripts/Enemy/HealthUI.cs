using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public float scaleMultiplier;

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Camera.main.transform.position, GetComponent<Canvas>().transform.position) > 10)
        {
            GetComponent<Canvas>().transform.localScale = new Vector3(Vector3.Distance(Camera.main.transform.position, GetComponent<Canvas>().transform.position) / 10 * scaleMultiplier, Vector3.Distance(Camera.main.transform.position, GetComponent<Canvas>().transform.position) / 10 * scaleMultiplier, Vector3.Distance(Camera.main.transform.position, GetComponent<Canvas>().transform.position) / 10 * scaleMultiplier);
        }
        else
        {
            GetComponent<Canvas>().transform.localScale = new Vector3(0.006f, 0.006f, 1f);
        }
        
        Vector3 v = Camera.main.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(Camera.main.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
