using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlaneSpawner : NetworkBehaviour
{
	[SerializeField]
	private Transform spawnLocation;
	[SerializeField]
	private GameObject dropPlane;
	[SerializeField]
	private PlaneTrigger planeTrigger;
#if !UNITY_EDITOR
	[SyncVar]
#endif
	private Quaternion pivotRotation;
#if !UNITY_EDITOR
	[SyncVar]
#endif
	private Quaternion spawnRotation;

	private void Start()
	{
		SelectStartLocation();
	}
#if !UNITY_EDITOR
	[Server]
#endif
	private void SelectStartLocation()
	{
		
		pivotRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
		spawnRotation = Quaternion.AngleAxis(-90f + Random.Range(-20f, 20f), Vector3.up);

		SpawnPlane();
	}
#if !UNITY_EDITOR
	[ClientRpc]
#endif
	private void SpawnPlane()
	{
		transform.rotation = pivotRotation;
		spawnLocation.localRotation = spawnRotation;

		Instantiate(dropPlane, spawnLocation.position, spawnLocation.rotation);
		planeTrigger.planeSpawned = true;
		planeTrigger.DetectAndIgnoreTriggers();
	}
}
