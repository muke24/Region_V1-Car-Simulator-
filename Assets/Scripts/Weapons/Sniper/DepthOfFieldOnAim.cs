using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DepthOfFieldOnAim : MonoBehaviour
{
	public Animator animator;

	public PostProcessVolume postProcessColorGrading;
	public PostProcessVolume postProcessAutoExposure;
	public PostProcessVolume postProcessDepthOfField;
	public PostProcessVolume postProcessMotionBlur;
	public PostProcessVolume postProcessScreenSpaceReflections;
	public PostProcessVolume postProcessAmbient;
	public PostProcessVolume postProcessBloom;

	[Space(10)]
	
	// Aiming Profiles

	public PostProcessProfile aimingColorGradingProfile;
	public PostProcessProfile aimingAutoExposureProfile;
	public PostProcessProfile aimingDepthOfFieldProfile;
	public PostProcessProfile aimingMotionBlurProfile;
	public PostProcessProfile aimingScreenSpaceReflectionsProfile;
	public PostProcessProfile aimingAmbientProfile;
	public PostProcessProfile aimingBloomProfile;

	[Space(10)]

	// Not Aiming Profiles

	public PostProcessProfile notAimingColorGradingProfile;
	public PostProcessProfile notAimingAutoExposureProfile;
	public PostProcessProfile notAimingDepthOfFieldProfile;
	public PostProcessProfile notAimingMotionBlurProfile;
	public PostProcessProfile notAimingScreenSpaceReflectionsProfile;
	public PostProcessProfile notAimingAmbientProfile;
	public PostProcessProfile notAimingBloomProfile;

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
							AimingProfiles();
						}
					}
				}
			}
		}		

		// If any other animation is playing then set the post processing profile without the blurry depth of field 
		else
		{
			NotAimingProfiles();
		}

		// If the player doesn't have the aim button down then set the post processing profile without the blurry depth of field 
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("SniperReload"))
		{
			NotAimingProfiles();
		}
	}

	void AimingProfiles()
	{
		postProcessColorGrading.profile = aimingColorGradingProfile;
		postProcessAutoExposure.profile = aimingAutoExposureProfile;
		postProcessDepthOfField.profile = aimingDepthOfFieldProfile;
		postProcessMotionBlur.profile = aimingMotionBlurProfile;
		postProcessScreenSpaceReflections.profile = aimingScreenSpaceReflectionsProfile;
		postProcessAmbient.profile = aimingAmbientProfile;
		postProcessBloom.profile = aimingBloomProfile;
	}

	void NotAimingProfiles()
	{
		postProcessColorGrading.profile = notAimingColorGradingProfile;
		postProcessAutoExposure.profile = notAimingAutoExposureProfile;
		postProcessDepthOfField.profile = notAimingDepthOfFieldProfile;
		postProcessMotionBlur.profile = notAimingMotionBlurProfile;
		postProcessScreenSpaceReflections.profile = notAimingScreenSpaceReflectionsProfile;
		postProcessAmbient.profile = notAimingAmbientProfile;
		postProcessBloom.profile = notAimingBloomProfile;
	}
}
