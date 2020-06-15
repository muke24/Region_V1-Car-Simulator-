#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CarInputManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LightingManager))]
public class CarController : MonoBehaviour
{
	[SerializeField]
	private CarInputManager im = null;
	[SerializeField]
	private LightingManager lm = null;
	[SerializeField]
	private UIManager uim = null;
	[SerializeField]
	private Transform CM = null;
	[SerializeField]
	private Rigidbody rb = null;

	//public WheelCollider wc;

	public List<WheelCollider> throttleWheels;
	public List<GameObject> steeringWheels;
	public List<GameObject> tailLightColour;

	public float strengthCoefficient = 20000f;
	public float maxTurn = 20f;
	public float brakeStrength;
	
	// Start is called before the first frame update
	void Start()
	{
		im = GetComponent<CarInputManager>();
		rb = GetComponent<Rigidbody>();
		lm = GetComponent<LightingManager>();
		uim = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("Canvas").GetComponent<UIManager>();

		if (CM)
		{
			rb.centerOfMass = CM.localPosition;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (im.lightToggle)
		{
			lm.ToggleHeadlights();
		}

		foreach (GameObject tl in tailLightColour)
		{
			tl.GetComponent<Renderer>().material.SetColor("_EmissionColor", im.brake ? new Color(0.5f, 0.111f, 0.111f) : Color.black);
		}
		if (im.brake)
		{
			lm.ToggleBrakeLightsOn();
		}
		if (im.brake == false)
		{
			lm.ToggleBrakeLightsOff();
		}

		uim.ChangeText(transform.InverseTransformVector(rb.velocity).z);
	}

	void FixedUpdate()
	{
		foreach (WheelCollider wheel in throttleWheels)
		{

			if (im.brake == true)
			{
				wheel.motorTorque = 0f;
				wheel.brakeTorque = brakeStrength * Time.deltaTime;
			}
			else
			{
				wheel.motorTorque = strengthCoefficient * Time.deltaTime * im.throttle;
				wheel.brakeTorque = 0f;
			}
		}

		foreach (GameObject wheel in steeringWheels)
		{
			wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * im.steer;
			wheel.transform.localEulerAngles = new Vector3(0f, im.steer * maxTurn, 0f);
		}		
	}
}
// This code is written by Peter Thompson
#endregion