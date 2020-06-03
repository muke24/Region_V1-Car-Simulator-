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
		// If the player has the aim button pressed
		if (Input.GetButton("Aim"))
		{
			// Checks if all the animations that would use the depth of field feature is active
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("SniperZoom") || animator.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomMove") || animator.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomMoveRight") || animator.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomMoveLeft") || animator.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomBoltAction") || animator.GetCurrentAnimatorStateInfo(0).IsName("SniperZoomShoot"))
			{
				// Checks if the reload bool is set to false in the sniper animation. 
				if (!animator.GetBool("Reload"))
				{
					// Checks if the current animation isn't the snipers reload animation
					if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
					{
						// Checks if the scoped in animations is 20% into its animation to stop the post process from activating before the sniper is fully scoped in
						if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f)
						{
							// Changes the post processing profile to the one that has the depth of field set to blurry
							postProcess.profile = aimingProfile;
						}
					}
				}
			}
		}		

		// If any other animation is playing then set the post processing profile without the blurry depth of field 
		else
		{
			postProcess.profile = notAimingProfile;
		}

		// If the player doesn't have the aim button down then set the post processing profile without the blurry depth of field 
		if (!Input.GetButton("Aim"))
		{
			postProcess.profile = notAimingProfile;
		}
	}
}
