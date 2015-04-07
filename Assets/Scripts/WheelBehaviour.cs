using UnityEngine;
using System.Collections;

public class WheelBehaviour : MonoBehaviour {

	public WheelCollider wheelCollider;

	// Use this for initialization
	void Start ()
	{}
	
	// Update is called once per frame
	void Update ()
	{
		Quaternion quat;
		Vector3 position;

		wheelCollider.GetWorldPose (out position, out quat);
		transform.position = position;
		transform.rotation = quat;
	}
}
