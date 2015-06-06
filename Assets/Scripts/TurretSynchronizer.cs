using UnityEngine;
using System.Collections;


public class TurretSynchronizer
{
	private Transform turret;

	private Quaternion syncRotationStart = Quaternion.identity;
	private Quaternion syncRotationEnd = Quaternion.identity;

	public TurretSynchronizer (Transform turret)
	{
		this.turret = turret;
	}

	public void DisableTurretScript() {
		turret.GetComponent<TurretBehavioiur>().enabled = false;
		turret.GetComponent<UnityStandardAssets.Utility.SimpleMouseRotator> ().enabled = false;
	}

	public void SyncedMovement(float duration)
	{ 		
		turret.rotation = BasicNetworkBehaviour.LerpQuaternion (syncRotationStart, syncRotationEnd, duration);
	}
	
	public void OnOutgonigSync(BitStream stream, NetworkMessageInfo info) {
		Quaternion syncRotation = turret.rotation;
		stream.Serialize(ref syncRotation);
	}
	
	public void OnIncomingSync(BitStream stream, NetworkMessageInfo info) {
		Quaternion syncRotation = Quaternion.identity;
		stream.Serialize(ref syncRotation);
		
		syncRotationStart = turret.rotation;
		syncRotationEnd = syncRotation;
	}

}


