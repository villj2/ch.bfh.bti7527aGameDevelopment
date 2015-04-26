using UnityEngine;
using System.Collections;

public class WheelBehaviour : MonoBehaviour {

	public WheelCollider wheelCollider;
	
	private SkidmarkBehaviour _skidmarks;
	private int _skidmarkLast;
	private Vector3 _skidmarkLastPos;

	// Use this for initialization
	void Start ()
	{
		// Get skidmarks script (not available in sceneMenu)
		GameObject skidmarksGO = GameObject.Find("Skidmarks");

		if (skidmarksGO) {
			_skidmarks = skidmarksGO.GetComponent<SkidmarkBehaviour>();
		}

		_skidmarkLast = -1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Quaternion quat;
		Vector3 position;

		wheelCollider.GetWorldPose (out position, out quat);
		transform.position = position;
		transform.rotation = quat;

		if (Application.loadedLevel != 0) {
			transform.rotation = quat;
		}

		WheelHit hit;
		if (wheelCollider.GetGroundHit (out hit)) {
			DoSkidmarking(hit);
		}
	}

	void DoSkidmarking(WheelHit hit)
	{
		Vector3 wheelVelo = wheelCollider.attachedRigidbody.GetPointVelocity (hit.point);

		if(Input.GetKey("space") || CarBehaviour.IsDrifting)
		{
			if (Vector3.Distance(_skidmarkLastPos, hit.point) > 0.1f)
			{
				Vector3 posNew = hit.point;

				// adjust skidmark position according to WheelCollider Position (left or right) to center skidmark
				if(wheelCollider.name == "WheelColliderFL" || wheelCollider.name == "WheelColliderRL")
				{
					posNew.x += 0.15f;
				}

				if(wheelCollider.name == "WheelColliderFR" || wheelCollider.name == "WheelColliderRR")
				{
					posNew.x -= 0.15f;
				}

				_skidmarkLast = _skidmarks.Add(posNew + wheelVelo*Time.deltaTime, hit.normal, 0.5f, _skidmarkLast);
				_skidmarkLastPos = hit.point;
			}
		}
		else _skidmarkLast = -1;
	}
}
