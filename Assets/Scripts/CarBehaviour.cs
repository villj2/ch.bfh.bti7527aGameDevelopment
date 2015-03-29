﻿using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {

	/* Custom modifications
	/******************************
	 * - Max. Speed
	 * - Driving Direction Check
	 * - Braking: Force down, Friction of wheels, brake torque
	 * - SteerFactor depending on carspeed
	 * 
	 */ 

	private const float MAX_SPEED_KMH = 150f;

	public WheelCollider 	wheelFL;
	public WheelCollider 	wheelFR;
	public WheelCollider 	wheelRL;
	public WheelCollider 	wheelRR;
	public float 			maxTorque = 500f;
	public float			normalBrake = 30000f;
	public float			fullBrake = 40000f;
	public GUIText 			guiSpeed;
	public Texture2D        guiSpeedDisplay;
	public Texture2D        guiSpeedPointer;
	public Transform		centerOfMass;
	public AudioClip		audioClipBrake;
	public Material			brakeLightMaterial;
	public Material 		backLightMaterial;
	public GameObject		backLightL;
	public GameObject		backLightR;

	private float           _currentSpeedKMH;
	private Rigidbody 		_rigidBody;
	private float			_motorTorque;
	private WheelFrictionCurve _frictionFLTmp;
	private WheelFrictionCurve _frictionFRTmp;
	private float			_brakeTorque;
	private ParticleSystem	_dustR;
	private ParticleSystem 	_dustL;
	private AudioSource		_audioSourceBrake;
	private Material		_stdBackLightMaterial;

	private float thrustTorque;

	// Use this for initialization
	void Start ()
	{
		_rigidBody = GetComponent<Rigidbody> ();
		//_rigidBody.centerOfMass = centerOfMass.transform.localPosition;
		_frictionFLTmp = wheelFL.forwardFriction;
		_frictionFRTmp = wheelFR.forwardFriction;

		_dustL = GameObject.Find ("DustL").GetComponent<ParticleSystem> ();

		_audioSourceBrake = (AudioSource)gameObject.AddComponent<AudioSource>();
		_audioSourceBrake.clip = audioClipBrake;
		_audioSourceBrake.loop = true;
		_audioSourceBrake.volume = 0.7f;
		_audioSourceBrake.playOnAwake = false;

		_stdBackLightMaterial = backLightL.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame constanc time per frame
	void FixedUpdate ()
	{
		_currentSpeedKMH = _rigidBody.velocity.magnitude * 3.6f;
		_motorTorque = maxTorque * Input.GetAxis("Vertical");
		guiSpeed.text = _currentSpeedKMH.ToString("0") + " KMH";


		/* MAX SPEED */
		if (_currentSpeedKMH > MAX_SPEED_KMH)
		{
			_rigidBody.velocity = (MAX_SPEED_KMH/3.6f) * _rigidBody.velocity.normalized;
		}


		/* ACCLERATION / BRAKING */
		// 0: still, 1: forward, -1: backward
		int drivingDirection = 0;

		if (_currentSpeedKMH > 5 && Vector3.Angle (transform.forward, _rigidBody.velocity) < 50f) {
			drivingDirection = 1;
		} else if(_currentSpeedKMH > 5 && Vector3.Angle(transform.forward, _rigidBody.velocity) >= 50f) {
			drivingDirection = -1;
		}

		bool doBraking = false;
		doBraking = (Input.GetAxis ("Vertical") < -0.1 && drivingDirection == 1) || (Input.GetAxis ("Vertical") > 0.1 && drivingDirection == -1);

		/* ADD FORCE TO WHEELS DEPENDING ON SPEED */
		wheelFL.attachedRigidbody.AddForce(-transform.up * 100 * wheelFL.attachedRigidbody.velocity.magnitude);
		wheelFR.attachedRigidbody.AddForce(-transform.up * 100 * wheelFR.attachedRigidbody.velocity.magnitude);

		//Debug.Log (doBraking);

		if (doBraking || FullBrake()) {

			// Brake or Fullbrake

			_frictionFLTmp.extremumSlip = 1f;
			_frictionFLTmp.extremumValue = 1.5f;
			_frictionFLTmp.asymptoteSlip = 1f;
			_frictionFLTmp.asymptoteValue = 1.5f;

			_frictionFRTmp.extremumSlip = 1f;
			_frictionFRTmp.extremumValue = 1.5f;
			_frictionFRTmp.asymptoteSlip = 1f;
			_frictionFRTmp.asymptoteValue = 1.5f;

			wheelFL.forwardFriction = _frictionFLTmp;
			wheelFR.forwardFriction = _frictionFRTmp;
			wheelFL.sidewaysFriction = _frictionFLTmp;
			wheelFR.sidewaysFriction = _frictionFRTmp;

			if(FullBrake())
			{
				_brakeTorque = fullBrake;
			} 
			else
			{
				_brakeTorque = normalBrake;
			}

			//Debug.Log(_brakeTorque);

			wheelFL.motorTorque = 0;
			wheelFR.motorTorque = 0;
			wheelFL.brakeTorque = _brakeTorque;
			wheelFR.brakeTorque = _brakeTorque;
			
			_dustL.enableEmission = true;

			backLightL.GetComponent<Renderer>().material = brakeLightMaterial;
			backLightR.GetComponent<Renderer>().material = brakeLightMaterial;
			
		} else {

			// Accelerate or nothing

			_frictionFLTmp.extremumSlip = 0.4f;
			_frictionFLTmp.extremumValue = 1;
			_frictionFRTmp.extremumSlip = 0.4f;
			_frictionFRTmp.extremumValue = 1;
			
			wheelFL.forwardFriction = _frictionFLTmp;
			wheelFR.forwardFriction = _frictionFRTmp;
			
			wheelFL.brakeTorque = 0f;
			wheelFR.brakeTorque = 0f;
			wheelFL.motorTorque = _motorTorque;
			wheelFR.motorTorque = _motorTorque;

			_dustL.enableEmission = false;

			if(drivingDirection == -1)
			{
				//Debug.Log("show backwards light");
				
				backLightL.GetComponent<Renderer>().material = backLightMaterial;
				backLightR.GetComponent<Renderer>().material = backLightMaterial;
			}
			else
			{
				backLightL.GetComponent<Renderer>().material = _stdBackLightMaterial;
				backLightR.GetComponent<Renderer>().material = _stdBackLightMaterial;
			}
		}



		/* STEERING */
		float steerFactor = Mathf.Max (-10f / MAX_SPEED_KMH * _currentSpeedKMH + 20f);
		wheelFL.steerAngle = steerFactor * Input.GetAxis("Horizontal");
		wheelFR.steerAngle = wheelFL.steerAngle;



	}

	// OnGUI is called on every frame when the orthographic GUI is rendered
	void OnGUI() 
	{   GUI.Box(new Rect(0, 0, 140, 140), guiSpeedDisplay);
		GUIUtility.RotateAroundPivot(Mathf.Abs(_currentSpeedKMH) + 40, new Vector2(70,70));
		GUI.DrawTexture(new Rect(0, 0, 140, 140), guiSpeedPointer, ScaleMode.StretchToFill);
	}

	void Update()
	{
		SetAudioPitch();
	}

	void SetAudioPitch()
	{
		float gearSpeedDelta = 25.0f;
		int gear = System.Math.Min((int)(_currentSpeedKMH / gearSpeedDelta), 5);
		float gearSpeedMin = gear * gearSpeedDelta;
		GetComponent<AudioSource>().pitch = (_currentSpeedKMH - gearSpeedMin) / gearSpeedDelta * 0.5f + 0.4f;
	}

	bool FullBrake()
	{
		// check if space is down
		return Input.GetKey ("space");
	}
}