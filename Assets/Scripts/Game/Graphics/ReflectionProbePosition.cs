#region This code is written by Peter Thompson
using System.Collections;
using UnityEngine;

public class ReflectionProbePosition : MonoBehaviour
{
	public Transform seat1Pos;
	public GameObject carCam;
	private ReflectionProbe reflectionProbe;
	private GameObject playerCam;
	private WaitForSeconds waitForSeconds = new WaitForSeconds(0.3f);

	// Start is called before the first frame update
	void Start()
	{
		// Retrieves the reflection probe
		reflectionProbe = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").transform.Find("Reflection Probe").GetComponent<ReflectionProbe>();
		// Retrieves the player's camera
		playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").gameObject;
		StartCoroutine(DoCheck());
	}

	IEnumerator DoCheck()
	{
		while (true)
		{
			if (reflectionProbe == null)
			{
				// Throw an error
				//

				reflectionProbe = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").transform.Find("Reflection Probe").GetComponent<ReflectionProbe>();
				// Retrieves the player's camera
				playerCam = GameObject.FindGameObjectWithTag("Player").transform.Find("Camera").gameObject;
			}

			// Sets the reflection probe to the car's camera position
			if (Car.inCar)
			{
				if (reflectionProbe.transform.parent != carCam.transform)
				{
					reflectionProbe.transform.parent = carCam.transform;
					reflectionProbe.transform.localPosition = Vector3.zero;
				}
			}

			// Sets the reflection probe to the player's camera position
			if (!Car.inCar)
			{
				if (reflectionProbe.transform.parent != playerCam.transform)
				{
					reflectionProbe.transform.parent = playerCam.transform;
					reflectionProbe.transform.localPosition = Vector3.zero;
				}
			}
			yield return waitForSeconds;
		}
	}
}
// This code is written by Peter Thompson
#endregion