using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuBehaviour : MonoBehaviour {

	public Material CarBodyMaterial;

	public WheelCollider WheelColliderFL;
	public WheelCollider WheelColliderFR;
	public WheelCollider WheelColliderRL;
	public WheelCollider WheelColliderRR;

	public Sprite CH1;
	public Sprite CH2;
	public Sprite CH3;
	public Sprite CH4;
	public Sprite CH5;
	public Sprite CH6;
	public Sprite CH7;
	public Sprite CH8;
	public Sprite CH9;
	public Sprite CH10;
	public Sprite CH11;



	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Menu behaviour load");

		// FIXME verursacht falsche target position
		Prefs.Load ();
		Prefs.SetWheelColliderSuspension (ref WheelColliderFL, ref WheelColliderFR, ref WheelColliderRL, ref WheelColliderRR);
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
		Prefs.Save ();
	}

	public void OnSliderSpringChange(float val)
	{
		Prefs.suspensionSpring = val;
		Prefs.SetWheelColliderSuspension (ref WheelColliderFL, ref WheelColliderFR, ref WheelColliderRL, ref WheelColliderRR);
		Prefs.Save ();
	}

	public void OnSliderDamperChange(float val)
	{
		Prefs.suspensionDamper = val;
		Prefs.SetWheelColliderSuspension (ref WheelColliderFL, ref WheelColliderFR, ref WheelColliderRL, ref WheelColliderRR);
		Prefs.Save ();
	}

	void OnApplicationQuit()
	{
		Prefs.Save ();
	}

	public void OnSliderCrosshairChange(float val)
	{
		//Prefs.crosshair = val;
		Debug.Log (string.Format (" crosshair slider {0}", val));
		Image img;

		img = GameObject.FindObjectOfType<Image>();

		int a = (int)( val*100);
		Debug.Log (string.Format (" crosshair slider {0}", a));
			//img =Resources
		img.sprite = CH4;


			


		Prefs.Save ();
	}
}
