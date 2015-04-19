using UnityEngine;
using System.Collections;
using UnityEditor;

public class Prefs : MonoBehaviour {

	public static float carBodyHue;
	public static float carBodySaturation;
	public static float carBodyLuminance;

	public static float suspensionDistance;
	public static float suspensionSpring;
	public static float suspensionDamper;

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
	}

	public static void Save()
	{
		PlayerPrefs.SetFloat("carBodyHue", carBodyHue);
		PlayerPrefs.SetFloat("carBodySaturation", carBodySaturation);
		PlayerPrefs.SetFloat("carBodyLuminance", carBodyLuminance);

		PlayerPrefs.SetFloat ("suspensionDistance", suspensionDistance);
		PlayerPrefs.SetFloat ("suspensionSpring", suspensionSpring);
		PlayerPrefs.SetFloat ("suspensionDamper", suspensionDamper);
	}

	public static void SetBodyMaterial(ref Material bodyMat)
	{
		bodyMat.color = EditorGUIUtility.HSVToRGB(carBodyHue, carBodySaturation, carBodyLuminance);
	}

	public static void SetWheelColliderSuspension(ref WheelCollider flWheelCol, ref WheelCollider frWheelCol, ref WheelCollider rlWheelCol, ref WheelCollider rrWheelCol)
	{
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
}
