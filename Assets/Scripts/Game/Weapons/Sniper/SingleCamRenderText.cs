using UnityEngine;

public class SingleCamRenderText : MonoBehaviour
{
	public Camera playerCam;
	public RenderTexture scopeRenderTexture;

	// Update is called once per frame
	void Update()
	{
		//if (GetComponent<PlayerAnimations>().scoped)
		//{
		//	playerCam.targetTexture = scopeRenderTexture;

		//	RenderTexture.active = scopeRenderTexture;
		//	playerCam.Render();

		//	RenderTexture.active = null;
		//	playerCam.targetTexture = null;
		//}
		//else
		//{
		//	RenderTexture.active = null;
		//	playerCam.targetTexture = null;
		//}
	}
}
