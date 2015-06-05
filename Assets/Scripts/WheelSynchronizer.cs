using UnityEngine;
using System.Collections;


public class WheelSynchronizer
{
	private Transform wheel;

	private Quaternion syncRotationStart = Quaternion.identity;
	private Quaternion syncRotationEnd = Quaternion.identity;

	public WheelSynchronizer (Transform wheel)
	{
		this.wheel = wheel;
	}

	public void DisableWheelScript() {
		wheel.GetComponent<WheelBehaviour>().enabled = false;
	}

	public void SyncedMovement(float duration)
	{ 		
		wheel.rotation = BasicNetworkBehaviour.LerpQuaternion (syncRotationStart, syncRotationEnd, duration);
	}
	
	public void OnOutgonigSync(BitStream stream, NetworkMessageInfo info) {
		Quaternion syncRotation = wheel.rotation;
		stream.Serialize(ref syncRotation);
	}
	
	public void OnIncomingSync(BitStream stream, NetworkMessageInfo info) {
		Quaternion syncRotation = Quaternion.identity;
		stream.Serialize(ref syncRotation);
		
		syncRotationStart = wheel.rotation;
		syncRotationEnd = syncRotation;
	}

}


