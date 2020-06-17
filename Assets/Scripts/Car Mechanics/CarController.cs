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

	public List<WheelCollider> throttleWheels;
	public List<GameObject> steeringWheels;
	public List<GameObject> tailLightColour;

	public float strengthCoefficient = 20000f;
	public float maxTurn = 20f;
	public float brakeStrength;

	public bool AWD;
	public bool RWD;
	public bool FWD;
	
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
			if (im.brake)
			{
				HandBrake();
			}
			else
			{
				if (AWD)
				{
					wheel.motorTorque = strengthCoefficient * Time.deltaTime * im.throttle;
					wheel.brakeTorque = 0f;
				}				
			}
		}

		if (!im.brake)
		{
			if (RWD)
			{
				throttleWheels[0].motorTorque = (strengthCoefficient * 2) * Time.deltaTime * im.throttle;
				throttleWheels[0].brakeTorque = 0f;
				throttleWheels[2].motorTorque = (strengthCoefficient * 2) * Time.deltaTime * im.throttle;
				throttleWheels[2].brakeTorque = 0f;
			}
			if (FWD)
			{
				throttleWheels[1].motorTorque = (strengthCoefficient * 2) * Time.deltaTime * im.throttle;
				throttleWheels[1].brakeTorque = 0f;
				throttleWheels[0].brakeTorque = 0f;
				throttleWheels[3].motorTorque = (strengthCoefficient * 2) * Time.deltaTime * im.throttle;
				throttleWheels[3].brakeTorque = 0f;
				throttleWheels[2].brakeTorque = 0f;
			}
		}

		foreach (GameObject wheel in steeringWheels)
		{
			wheel.GetComponent<WheelCollider>().steerAngle = maxTurn * im.steer;
			wheel.transform.localEulerAngles = new Vector3(0f, im.steer * maxTurn, 0f);
		}		
	}

	void Brake()
	{
		foreach (WheelCollider wheel in throttleWheels)
		{
			wheel.motorTorque = 0f;
			wheel.brakeTorque = brakeStrength * Time.deltaTime;
		}		
	}

	void HandBrake()
	{
		if (im.brake)
		{
			if (AWD)
			{
				throttleWheels[0].motorTorque = 0f;
				throttleWheels[0].brakeTorque = brakeStrength * Time.deltaTime;
				throttleWheels[2].motorTorque = 0f;
				throttleWheels[2].brakeTorque = brakeStrength * Time.deltaTime;

				throttleWheels[1].motorTorque = strengthCoefficient * Time.deltaTime * im.throttle;
				throttleWheels[1].brakeTorque = 0f;
				throttleWheels[3].motorTorque = strengthCoefficient * Time.deltaTime * im.throttle;
				throttleWheels[3].brakeTorque = 0f;
			}
			if (RWD)
			{
				throttleWheels[0].motorTorque = 0f;
				throttleWheels[0].brakeTorque = (brakeStrength * 2) * Time.deltaTime;
				throttleWheels[2].motorTorque = 0f;
				throttleWheels[2].brakeTorque = (brakeStrength * 2) * Time.deltaTime;
			}
			if (FWD)
			{
				throttleWheels[0].motorTorque = 0f;
				throttleWheels[0].brakeTorque = (brakeStrength * 2) * Time.deltaTime;
				throttleWheels[2].motorTorque = 0f;
				throttleWheels[2].brakeTorque = (brakeStrength * 2) * Time.deltaTime;

				throttleWheels[1].motorTorque = (strengthCoefficient * 2) * Time.deltaTime * im.throttle;
				throttleWheels[1].brakeTorque = 0f;
				throttleWheels[3].motorTorque = (strengthCoefficient * 2) * Time.deltaTime * im.throttle;
				throttleWheels[3].brakeTorque = 0f;
			}
		}
	}
}
// This code is written by Peter Thompson
#endregion