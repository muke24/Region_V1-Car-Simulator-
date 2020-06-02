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
		//if (animator.GetCurrentAnimatorStateInfo(0).IsName("SniperZoom"))
		if (animator.GetBool("Aim"))
		{
			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f)
			{
				postProcess.profile = aimingProfile;
			}
		}

		else
		{
			postProcess.profile = notAimingProfile;
		}
	}
}
