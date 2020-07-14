using UnityEngine;
using UnityEngine.AI;

public class BreakObject : MonoBehaviour
{
	private Rigidbody[] rigids;
	private Renderer[] renderers;
	private bool destroyingParent = false;
	[SerializeField]
	private float timer = 15f;
	[SerializeField]
	private float fadeTime = 2f;

	private void Start()
	{
		if (name.Contains("BreakableFence"))
		{
			GetComponent<FixedJoint>().connectedBody = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Rigidbody>();
		}

		rigids = new Rigidbody[GetComponentsInChildren<Rigidbody>().Length];
		rigids = GetComponentsInChildren<Rigidbody>();
		renderers = new Renderer[GetComponentsInChildren<Renderer>().Length];
		renderers = GetComponentsInChildren<Renderer>();
		Physics.IgnoreCollision(GetComponent<BoxCollider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>());
	}

	private void Update()
	{
		if (destroyingParent)
		{
			if (timer > 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				foreach (Renderer rend in renderers)
				{
					Color color = rend.material.color;
					if (rend.material.enableInstancing)
					{
						Shader shader = rend.material.shader;
						rend.material = null;
						Material mat = new Material(shader);
						mat.enableInstancing = false;
						mat.ToFadeMode();
						mat.color = color;						
						rend.material = mat;
					}
					else
					{
						if (color.a > 0)
						{
							color.a -= Time.deltaTime / fadeTime;
							rend.material.color = color;
						}
						if (color.a < 0)
						{
							Destroy(gameObject);							
						}						
					}					
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		foreach (Rigidbody rigid in rigids)
		{
			if (rigid != null)
			{
				if (rigid.isKinematic && other.transform.CompareTag("Car"))
				{
					rigid.isKinematic = false;					

					if (!destroyingParent)
					{
						rigid.AddForce(other.transform.forward, ForceMode.Force);
						Destroy(GetComponent<NavMeshObstacle>(), 1f);
						destroyingParent = true;
					}
				}
			}
		}
	}

}

#region Extension class to change material transparency
public static class MaterialExtensions
{
	public static void ToOpaqueMode(this Material material)
	{
		material.SetOverrideTag("RenderType", "");
		material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
		material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
		material.SetInt("_ZWrite", 1);
		material.DisableKeyword("_ALPHATEST_ON");
		material.DisableKeyword("_ALPHABLEND_ON");
		material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		material.renderQueue = -1;
	}

	public static void ToFadeMode(this Material material)
	{
		material.SetOverrideTag("RenderType", "Transparent");
		material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		material.SetInt("_ZWrite", 0);
		material.DisableKeyword("_ALPHATEST_ON");
		material.EnableKeyword("_ALPHABLEND_ON");
		material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
	}
}
#endregion
