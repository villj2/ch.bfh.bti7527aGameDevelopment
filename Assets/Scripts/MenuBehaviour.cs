using UnityEngine;
using System.Collections;

public class MenuBehaviour : MonoBehaviour {

	public Material CarBodyMaterial;

	public WheelCollider WheelColliderFL;
	public WheelCollider WheelColliderFR;
	public WheelCollider WheelColliderRL;
	public WheelCollider WheelColliderRR;

	// Use this for initialization
	void Start ()
	{
		Prefs.Load ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void OnButtonStartClick()
	{
		Application.LoadLevel (1);
	}

	public void OnButtonMenuClick()
	{
		Application.LoadLevel (0);
	}

	public void OnSliderHueChange(float val)
	{
		Prefs.carBodyHue = val;
		Prefs.SetBodyMaterial (ref CarBodyMaterial);
	}

	public void OnSliderSaturationChange(float val)
	{
		Prefs.carBodySaturation = val;
		Prefs.SetBodyMaterial (ref CarBodyMaterial);
	}

	public void OnSliderLuminanceChange(float val)
	{
		Prefs.carBodyLuminance = val;
		Prefs.SetBodyMaterial (ref CarBodyMaterial);
	}

	public void OnSliderDistanceChange(float val)
	{
		Prefs.suspensionDistance = val;
		Prefs.SetWheelColliderSuspension (ref WheelColliderFL, ref WheelColliderFR, ref WheelColliderRL, ref WheelColliderRR);
	}

	public void OnSliderSpringChange(float val)
	{
		Prefs.suspensionSpring = val;
		Prefs.SetWheelColliderSuspension (ref WheelColliderFL, ref WheelColliderFR, ref WheelColliderRL, ref WheelColliderRR);
	}

	public void OnSliderDamperChange(float val)
	{
		Prefs.suspensionDamper = val;
		Prefs.SetWheelColliderSuspension (ref WheelColliderFL, ref WheelColliderFR, ref WheelColliderRL, ref WheelColliderRR);
	}

	void OnApplicationQuit()
	{
		Prefs.Save ();
	}
}
