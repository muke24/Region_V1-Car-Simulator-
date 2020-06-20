using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!GameMode.captureTheFlag)
        {
            Destroy(transform.Find("FlagCanvas").gameObject);
            Destroy(transform.Find("Flag").gameObject);
        }
    }
}
