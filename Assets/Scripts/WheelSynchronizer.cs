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
		if (duration == 0f) {
			return;
		}
		
		if(BasicNetworkBehaviour.isQuaternionNAN(syncRotationStart)) 
			return;
		if(BasicNetworkBehaviour.isQuaternionNAN(syncRotationEnd)) 
			return;
		if (float.IsNaN (duration))
			return;
		if (BasicNetworkBehaviour.isQuaternionZero(syncRotationEnd)) 
			return;
		if (BasicNetworkBehaviour.isQuaternionZero(syncRotationStart))
			return;

		wheel.rotation = Quaternion.Lerp (syncRotationStart, syncRotationEnd, duration);
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


