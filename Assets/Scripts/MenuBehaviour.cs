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

	private Image _myCHImage;



	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Menu behaviour load");

		// FIXME verursacht falsche target position
		Prefs.Load ();
		Prefs.SetWheelColliderSuspension (ref WheelColliderFL, ref WheelColliderFR, ref WheelColliderRL, ref WheelColliderRR);


		
		_myCHImage = GameObject.Find("Image_CH").GetComponent<Image>();
		//_myCHImage.sprite = CH1;

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

		int a = (int)( val*100);
		Debug.Log (string.Format (" crosshair slider {0}", a));


		if (a >= 0 && a < 10)
			_myCHImage.sprite = CH1;
		if (a >= 10 && a < 20)
			_myCHImage.sprite = CH2;
		if (a >= 20 && a < 30)
			_myCHImage.sprite = CH3;
		if (a >= 30 && a < 40)
			_myCHImage.sprite = CH4;
		if (a >= 40 && a < 50)
			_myCHImage.sprite = CH5;
		if (a >= 50 && a < 60)
			_myCHImage.sprite = CH6;
		if (a >= 60 && a < 70)
			_myCHImage.sprite = CH7;
		if (a >= 70 && a < 80)
			_myCHImage.sprite = CH8;
		if (a >= 80 && a < 90)
			_myCHImage.sprite = CH9;
		if (a >= 90 )
			_myCHImage.sprite = CH10;



		Prefs.Save ();
	}
}
