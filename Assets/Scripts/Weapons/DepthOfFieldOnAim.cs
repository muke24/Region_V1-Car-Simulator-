using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DepthOfFieldOnAim : MonoBehaviour
{
    public PostProcessVolume postProcess;
    public PostProcessProfile aimingProfile;
    public PostProcessProfile notAimingProfile;
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("Aim"))
        {
            postProcess.profile = aimingProfile;
        }
        if (!animator.GetBool("Aim"))
        {
            postProcess.profile = notAimingProfile;
        }
    }
}
