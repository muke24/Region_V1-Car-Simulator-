#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigid;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;

    void FixedUpdate()
    {
        // Makes the car float up and down when its reached the water submerge depth
        if (transform.position.y < 0)
        {
            float displacementMultiplier = Mathf.Clamp01((- transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigid.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }
}
// This code is written by Peter Thompson
#endregion