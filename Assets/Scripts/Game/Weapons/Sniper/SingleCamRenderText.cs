using UnityEngine;

public class SingleCamRenderText : MonoBehaviour
{
	public bool singleCamMode = false;
	public Camera playerCam;
	public Camera scopeCam;
	public RenderTexture scopeRenderTexture;

	// Update is called once per frame
	void Update()
	{
		if (singleCamMode)
		{
			if (GetComponent<PlayerAnimations>().scoped)
			{
				if (scopeCam.enabled == true)
				{
					RenderTexture.active = null;
					scopeCam.targetTexture = null;
					scopeCam.enabled = false;
				}

				playerCam.targetTexture = scopeRenderTexture;

				RenderTexture.active = scopeRenderTexture;
				playerCam.Render();

				RenderTexture.active = null;
				playerCam.targetTexture = null;
			}
			else
			{
				RenderTexture.active = null;
				playerCam.targetTexture = null;
			}
		}
		else
		{
			if (GetComponent<PlayerAnimations>().scoped)
			{
				if (scopeCam.enabled == false)
				{
					scopeCam.enabled = true;
					RenderTexture.active = scopeRenderTexture;
					scopeCam.targetTexture = scopeRenderTexture;
				}
			}
		}
	}

	public void CamModeChange()
	{
		if (singleCamMode)
		{
			singleCamMode = false;
			scopeCam.enabled = true;
			RenderTexture.active = scopeRenderTexture;
			scopeCam.targetTexture = scopeRenderTexture;
		}

		else
		{
			singleCamMode = true;
			RenderTexture.active = null;
			scopeCam.targetTexture = null;
			scopeCam.enabled = false;
		}
	}
}
