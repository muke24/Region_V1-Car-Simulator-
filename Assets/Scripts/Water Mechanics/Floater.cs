#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
	public List<Rigidbody> rigidList;
	public List<float> dragBeforeSubmerged;

	public float depthBeforeSubmerged = 1f;
	public float displacementAmount = 2f;
	public float dragForce = 1f;

	private void Start()
	{
		//depthBeforeSubmerged = GetComponent<Collider>().bounds.max.y - GetComponent<Collider>().bounds.max.y * 2;
	}

	void FixedUpdate()
	{
		// Makes the car float up and down when its reached the water submerge depth
		if (transform.position.y > 0)
		{
			float displacementMultiplier = Mathf.Clamp01((transform.position.y) / depthBeforeSubmerged) * displacementAmount;
			foreach (Rigidbody rigidbody in rigidList)
			{
				rigidbody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
			}
		}
		if (transform.position.y <= 0)
		{
			float displacementMultiplier = Mathf.Clamp01((-transform.position.y) / depthBeforeSubmerged) * displacementAmount;
			foreach (Rigidbody rigidbody in rigidList)
			{
				rigidbody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.TryGetComponent<Player>(out Player player))
		{
			if (other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
			{
				dragBeforeSubmerged.Add(other.attachedRigidbody.drag);
				rigidList.Add(other.attachedRigidbody);

				for (int i = 0; i < rigidList.Count; i++)
				{
					if (other.attachedRigidbody == rigidList[i])
					{
						if (other.attachedRigidbody.mass < dragForce)
						{
							other.attachedRigidbody.drag += dragForce;
						}						
					}

					if (i != 0)
					{
						if (rigidList[i] == rigidList[i - 1])
						{
							rigidList[i - 1].drag -= dragForce;
							rigidList.Remove(rigidList[i]);
						}
					}

					if (dragBeforeSubmerged[i] >= dragForce)
					{
						dragBeforeSubmerged[i] -= dragForce;
					}
				}				
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.TryGetComponent<Player>(out Player player))
		{
			if (other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
			{
				for (int i = 0; i < rigidList.Count; i++)
				{
					if (other.GetComponent<Rigidbody>() == rigidList[i])
					{
						other.GetComponent<Rigidbody>().drag = dragBeforeSubmerged[i];
						dragBeforeSubmerged.Remove(dragBeforeSubmerged[i]);
						rigidList.Remove(rigidList[i]);						
					}
				}
			}
		}
	}
}
// This code is written by Peter Thompson
#endregion