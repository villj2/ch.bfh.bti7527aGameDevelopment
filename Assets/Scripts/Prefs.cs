using UnityEngine;
using System.Collections;

public class Prefs : MonoBehaviour {

	public static float carBodyHue;
	public static float carBodySaturation;
	public static float carBodyLuminance;

	public static float suspensionDistance;
	public static float suspensionSpring;
	public static float suspensionDamper;

	public static Texture2D crosshair;

	public static void Load()
	{
		Debug.Log ("Load");

		carBodyHue = PlayerPrefs.GetFloat("carBodyHue", 0.0f);
		carBodySaturation = PlayerPrefs.GetFloat("carBodySaturation", 1.0f);
		carBodyLuminance = PlayerPrefs.GetFloat("carBodyLuminance", 1.0f);

		/*suspensionDistance = PlayerPrefs.GetFloat ("suspensionDistance", 0.2f);
		suspensionSpring = PlayerPrefs.GetFloat ("suspensionSpring", 35000f);
		suspensionDamper = PlayerPrefs.GetFloat ("suspensionDamper", 4500f);*/
		suspensionDistance = 0.2f;
		suspensionSpring = 35000f;
		suspensionDamper = 4500f;

		string c = PlayerPrefs.GetString ("crosshair");

		//crosshair = Resources.Load (System.IO.Path.Combine ("crosshairs", c), typeof(Texture));
	}

	public static void Save()
	{
		PlayerPrefs.SetFloat("carBodyHue", carBodyHue);
		PlayerPrefs.SetFloat("carBodySaturation", carBodySaturation);
		PlayerPrefs.SetFloat("carBodyLuminance", carBodyLuminance);

		PlayerPrefs.SetFloat ("suspensionDistance", suspensionDistance);
		PlayerPrefs.SetFloat ("suspensionSpring", suspensionSpring);
		PlayerPrefs.SetFloat ("suspensionDamper", suspensionDamper);

//		PlayerPrefs.SetString ("crosshair", crosshair.name);
	}

	public static void SetBodyMaterial(ref Material bodyMat)
	{
		bodyMat.color = HSVToRGB(carBodyHue, carBodySaturation, carBodyLuminance);
	}

	public static void SetWheelColliderSuspension(ref WheelCollider flWheelCol, ref WheelCollider frWheelCol, ref WheelCollider rlWheelCol, ref WheelCollider rrWheelCol)
	{
		if (suspensionDamper == 0)
			suspensionDamper = 4500f;

		if (suspensionSpring == 0)
			suspensionSpring = 35000f;

		if (suspensionDistance == 0)
			suspensionDistance = 0.2f;

		flWheelCol.suspensionDistance = suspensionDistance;
		frWheelCol.suspensionDistance = suspensionDistance;
		rlWheelCol.suspensionDistance = suspensionDistance;
		rrWheelCol.suspensionDistance = suspensionDistance;

		JointSpring mySpringF = new JointSpring ();
		JointSpring mySpringR = new JointSpring ();
		mySpringF.spring = suspensionSpring;
		mySpringF.damper = suspensionDamper;
		mySpringR.spring = suspensionSpring;
		mySpringR.damper = suspensionDamper;
		
		flWheelCol.suspensionSpring = mySpringF;
		frWheelCol.suspensionSpring = mySpringF;
		rlWheelCol.suspensionSpring = mySpringR;
		rrWheelCol.suspensionSpring = mySpringR;
	}

	public static Color HSVToRGB(float H, float S, float V)
	{
		if (S == 0f)
			return new Color(V,V,V);
		else if (V == 0f)
			return Color.black;
		else
		{
			Color col = Color.black;
			float Hval = H * 6f;
			int sel = Mathf.FloorToInt(Hval);
			float mod = Hval - sel;
			float v1 = V * (1f - S);
			float v2 = V * (1f - S * mod);
			float v3 = V * (1f - S * (1f - mod));
			switch (sel + 1)
			{
			case 0:
				col.r = V;
				col.g = v1;
				col.b = v2;
				break;
			case 1:
				col.r = V;
				col.g = v3;
				col.b = v1;
				break;
			case 2:
				col.r = v2;
				col.g = V;
				col.b = v1;
				break;
			case 3:
				col.r = v1;
				col.g = V;
				col.b = v3;
				break;
			case 4:
				col.r = v1;
				col.g = v2;
				col.b = V;
				break;
			case 5:
				col.r = v3;
				col.g = v1;
				col.b = V;
				break;
			case 6:
				col.r = V;
				col.g = v1;
				col.b = v2;
				break;
			case 7:
				col.r = V;
				col.g = v3;
				col.b = v1;
				break;
			}
			col.r = Mathf.Clamp(col.r, 0f, 1f);
			col.g = Mathf.Clamp(col.g, 0f, 1f);
			col.b = Mathf.Clamp(col.b, 0f, 1f);
			return col;
		}
	}
}
