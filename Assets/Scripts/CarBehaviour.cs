using UnityEngine;
using System.Collections;

public class CarBehaviour : MonoBehaviour {

	/* Custom modifications
	/******************************
	 * - Max. Speed
	 * - Driving Direction Check
	 * - Braking: Force down, Friction of wheels, brake torque
	 * - SteerFactor depending on carspeed
	 * 
	 * 29.03.2015
	 * - Smoke Emission, based on speed
	 * - Brake Audio
	 * - Full / Normal Brake
	 * - Check if car is on ground (for smoke emission, brake audio)
	 * 
	 * 12.04.2015
	 * - Countdown Audio
	 * - Countdown Text
	 * - Car Engine Start Audio
	 * - Menuwechsel
	 * - Platform Rotation
	 * 
	 * 19.04.2015
	 * - Menu: Car Color Modification
	 * - Menu: Car Suspension Modification
	 * - Saving Properties in Menu for Scene1
	 * 
	 * 26.04.2015
	 * - Skidmarks + Alignment (center of wheel)
	 * - Different Frictions full / normal brake
	 * - No smoke / sound on normal braking
	 * - Smoke / Sound / Skidmarks while drifting
	 */ 

	private const float MAX_SPEED_KMH = 150f;

	public static bool IsDrifting = false;

	public WheelCollider 	wheelFL;
	public WheelCollider 	wheelFR;
	public WheelCollider 	wheelRL;
	public WheelCollider 	wheelRR;
	public float 			maxTorque = 20000f; //500f;
	public float			normalBrake = 30000f;
	public float			fullBrake = 40000f;
	public GUIText 			guiSpeed;
	public Texture2D        guiSpeedDisplay;
	public Texture2D        guiSpeedPointer;
	public Transform		centerOfMass;
	public AudioClip		audioClipBrake;
	public AudioClip		audioClipEngineStart;
	public Material			brakeLightMaterial;
	public Material 		backLightMaterial;
	public GameObject		backLightL;
	public GameObject		backLightR;

	private float           _currentSpeedKMH;
	private Rigidbody 		_rigidBody;
	private float			_motorTorque;
	private WheelFrictionCurve _frictionTmp;
	private float			_brakeTorque;
	private ParticleSystem	_dustR;
	private ParticleSystem 	_dustL;
	private AudioSource		_audioSourceBrake;
	private AudioSource		_audioSourceEngineStart;
	private Material		_stdBackLightMaterial;

	void Start ()
	{
		_rigidBody = GetComponent<Rigidbody> ();
		_rigidBody.centerOfMass = centerOfMass.transform.localPosition;
		_frictionTmp = wheelFL.forwardFriction;

		_dustL = GameObject.Find ("DustL").GetComponent<ParticleSystem> ();
		_dustR = GameObject.Find ("DustR").GetComponent<ParticleSystem> ();

		_audioSourceBrake = (AudioSource)gameObject.AddComponent<AudioSource>();
		_audioSourceBrake.clip = audioClipBrake;
		_audioSourceBrake.loop = true;
		_audioSourceBrake.volume = 0.7f;
		_audioSourceBrake.playOnAwake = false;

		_audioSourceEngineStart = (AudioSource)gameObject.AddComponent<AudioSource>();
		_audioSourceEngineStart.clip = audioClipEngineStart;
		_audioSourceEngineStart.loop = false;
		_audioSourceEngineStart.volume = 1f;
		_audioSourceEngineStart.playOnAwake = true;
		_audioSourceEngineStart.Play ();

		_stdBackLightMaterial = backLightL.GetComponent<Renderer>().material;

		SetWheelColliderSuspension();
	}
	
	// Update is called once per frame constanc time per frame
	void FixedUpdate ()
	{
		_currentSpeedKMH = _rigidBody.velocity.magnitude * 3.6f;
		_motorTorque = maxTorque * Input.GetAxis("Vertical");

		guiSpeed.text = _currentSpeedKMH.ToString("0") + " KMH";

		// MAX SPEED
		if (_currentSpeedKMH > MAX_SPEED_KMH)
		{
			_rigidBody.velocity = (MAX_SPEED_KMH/3.6f) * _rigidBody.velocity.normalized;
		}


		// ACCLERATION / BRAKING
		// 0: still, 1: forward, -1: backward
		int drivingDirection = 0;

		if (_currentSpeedKMH > 5 && Vector3.Angle (transform.forward, _rigidBody.velocity) < 50f) {
			drivingDirection = 1;
		} else if(_currentSpeedKMH > 5 && Vector3.Angle(transform.forward, _rigidBody.velocity) >= 50f) {
			drivingDirection = -1;
		}

		// Check if car is drifting
		IsDrifting = Vector3.Angle (transform.forward, _rigidBody.velocity) > 20f && _currentSpeedKMH > 5 && drivingDirection == 1;

		bool doBraking = false;
		doBraking = (Input.GetAxis ("Vertical") < -0.1 && drivingDirection == 1) || (Input.GetAxis ("Vertical") > 0.1 && drivingDirection == -1);

		/* ADD FORCE DOWN TO WHEELS DEPENDING ON SPEED */
		wheelFL.attachedRigidbody.AddForce(-transform.up * 100 * wheelFL.attachedRigidbody.velocity.magnitude);
		wheelFR.attachedRigidbody.AddForce(-transform.up * 100 * wheelFR.attachedRigidbody.velocity.magnitude);

		if (doBraking || FullBrake()) {

			// BRAKE OR FULLBRAKE

			if(FullBrake())
			{
				_frictionTmp.extremumSlip = 1f;
				_frictionTmp.extremumValue = 1.5f;
				_frictionTmp.asymptoteSlip = 1f;
				_frictionTmp.asymptoteValue = 1.5f;

				_brakeTorque = fullBrake;
			} 
			else
			{
				_frictionTmp.extremumSlip = 0.7f;
				_frictionTmp.extremumValue = 1f;
				_frictionTmp.asymptoteSlip = 0.7f;
				_frictionTmp.asymptoteValue = 1f;

				_brakeTorque = normalBrake;
			}

			wheelFL.forwardFriction = _frictionTmp;
			wheelFL.sidewaysFriction = _frictionTmp;
			wheelFR.forwardFriction = _frictionTmp;
			wheelFR.sidewaysFriction = _frictionTmp;
			wheelRL.forwardFriction = _frictionTmp;
			wheelRL.sidewaysFriction = _frictionTmp;
			wheelRR.forwardFriction = _frictionTmp;
			wheelRR.sidewaysFriction = _frictionTmp;

			wheelFL.motorTorque = 0;
			wheelFR.motorTorque = 0;
			wheelFL.brakeTorque = _brakeTorque;
			wheelFR.brakeTorque = _brakeTorque;

			// SMOKE / BRAKE AUDIO
			if(_currentSpeedKMH > 5 && FullBrake())
			{
				if(!IsDrifting)
				{
					_dustL.emissionRate = _dustR.emissionRate = Mathf.Max(_currentSpeedKMH * 1.3f, 2);
				}

				if(wheelRL.isGrounded) _dustL.enableEmission = true;
				if(wheelRR.isGrounded) _dustR.enableEmission = true;

				if(!_audioSourceBrake.isPlaying && wheelRL.isGrounded && wheelRR.isGrounded)
				{
					_audioSourceBrake.Play();
				}
			}
			else
			{
				_dustL.enableEmission = _dustR.enableEmission = false;

				if(_audioSourceBrake.isPlaying)
				{
					_audioSourceBrake.Stop();
				}
			}

			backLightL.GetComponent<Renderer>().material = brakeLightMaterial;
			backLightR.GetComponent<Renderer>().material = brakeLightMaterial;
			
		} else {

			// ACCELERATE OR NOTHING

			_frictionTmp.extremumSlip = 0.4f;
			_frictionTmp.extremumValue = 1;
			_frictionTmp.extremumSlip = 0.4f;
			_frictionTmp.extremumValue = 1;

			wheelFL.forwardFriction = _frictionTmp;
			wheelFR.forwardFriction = _frictionTmp;
			wheelRL.forwardFriction = _frictionTmp;
			wheelRR.forwardFriction = _frictionTmp;
			
			wheelFL.brakeTorque = 0f;
			wheelFR.brakeTorque = 0f;
			wheelFL.motorTorque = _motorTorque;
			wheelFR.motorTorque = _motorTorque;

			// SMOKE
			if(!IsDrifting)
			{
				_dustL.enableEmission = _dustR.enableEmission = false;
			}

			// BRAKE AUDIO
			if(_audioSourceBrake.isPlaying && !IsDrifting)
			{
				_audioSourceBrake.Stop();
			}

			if(drivingDirection == -1)
			{
				backLightL.GetComponent<Renderer>().material = backLightMaterial;
				backLightR.GetComponent<Renderer>().material = backLightMaterial;
			}
			else
			{
				backLightL.GetComponent<Renderer>().material = _stdBackLightMaterial;
				backLightR.GetComponent<Renderer>().material = _stdBackLightMaterial;
			}
		}

		// Play Audio if drifting and show smoke
		if (IsDrifting) {

			if(!_audioSourceBrake.isPlaying && wheelRL.isGrounded && wheelRR.isGrounded)
			{
				_audioSourceBrake.Play();
			}

			_dustL.emissionRate = _dustR.emissionRate = Mathf.Max(_currentSpeedKMH * 3f, 4);
			if(wheelRL.isGrounded) _dustL.enableEmission = true;
			if(wheelRR.isGrounded) _dustR.enableEmission = true;
		}

		// STEERING
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
		GetComponent<AudioSource> ().volume = 1;
		GetComponent<AudioSource>().pitch = (_currentSpeedKMH - gearSpeedMin) / gearSpeedDelta * 0.5f + 0.4f;
	}

	bool FullBrake()
	{
		// check if space is down
		return Input.GetKey ("space");
	}

	void SetWheelColliderSuspension ()
	{
		Debug.Log (PlayerPrefs.GetFloat ("suspensionDistance", 0.2f));
		Debug.Log (PlayerPrefs.GetFloat ("suspensionSpring", 35000f));
		Debug.Log (PlayerPrefs.GetFloat ("suspensionDamper", 4500f));

		Prefs.suspensionDistance = PlayerPrefs.GetFloat ("suspensionDistance", 0.2f);
		Prefs.suspensionSpring = PlayerPrefs.GetFloat ("suspensionSpring", 35000f);
		Prefs.suspensionDamper = PlayerPrefs.GetFloat ("suspensionDamper", 4500f);

		Prefs.SetWheelColliderSuspension (ref wheelFL, ref wheelFR, ref wheelRL, ref wheelRR);
	}
}