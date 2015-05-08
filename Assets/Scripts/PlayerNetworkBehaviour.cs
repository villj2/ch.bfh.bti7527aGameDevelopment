using UnityEngine;
using System.Collections;

public class PlayerNetworkBehaviour : BasicNetworkBehaviour {

	private Quaternion _syncRotationWFLStart = Quaternion.identity;
	private Quaternion _syncRotationWFLEnd = Quaternion.identity;
	private Quaternion _syncRotationWFRStart = Quaternion.identity;
	private Quaternion _syncRotationWFREnd = Quaternion.identity;
	private Quaternion _syncRotationWRLStart = Quaternion.identity;
	private Quaternion _syncRotationWRLEnd = Quaternion.identity;
	private Quaternion _syncRotationWRRStart = Quaternion.identity;
	private Quaternion _syncRotationWRREnd = Quaternion.identity;

	protected override void OnOpponentUpdate() {
		GetComponent<CarBehaviour>().enabled = false;
		Transform trans = rigidbody.transform;
		trans.Find("Turret").GetComponent<TurretBehavioiur>().enabled = false;
		trans.Find ("Wheels/FL").GetComponent<WheelBehaviour>().enabled = false;
		trans.Find ("Wheels/FR").GetComponent<WheelBehaviour>().enabled = false;
		trans.Find ("Wheels/RL").GetComponent<WheelBehaviour>().enabled = false;
		trans.Find ("Wheels/RR").GetComponent<WheelBehaviour>().enabled = false;
	}

	protected override void SyncedMovement(float duration)
	{ 
		base.SyncedMovement (duration);

		Transform trans = rigidbody.transform;

		trans.Find ("Wheels/FL").rotation = Quaternion.Lerp (_syncRotationWFLStart, _syncRotationWFLEnd, duration);
		trans.Find ("Wheels/FR").rotation = Quaternion.Lerp (_syncRotationWFRStart, _syncRotationWFREnd, duration);
		trans.Find ("Wheels/RL").rotation = Quaternion.Lerp (_syncRotationWRLStart, _syncRotationWRLEnd, duration);
		trans.Find ("Wheels/RR").rotation = Quaternion.Lerp (_syncRotationWRRStart, _syncRotationWRREnd, duration);

	}
	
	protected override void OnOutgonigSync(BitStream stream, NetworkMessageInfo info) {
		base.OnOutgonigSync (stream, info);

		Transform trans = rigidbody.transform;

		Quaternion syncRotationWFL = trans.Find ("Wheels/FL").rotation;
		Quaternion syncRotationWFR = trans.Find ("Wheels/FR").rotation;
		Quaternion syncRotationWRL = trans.Find ("Wheels/RL").rotation;
		Quaternion syncRotationWRR = trans.Find ("Wheels/RR").rotation;

		stream.Serialize(ref syncRotationWFL);
		stream.Serialize(ref syncRotationWFR);
		stream.Serialize(ref syncRotationWRL);
		stream.Serialize(ref syncRotationWRR);
	}
	
	protected override void OnIncomingSync(BitStream stream, NetworkMessageInfo info) {
		base.OnIncomingSync (stream, info);

		Transform trans = rigidbody.transform;

		Quaternion syncRotationWFL = Quaternion.identity;
		Quaternion syncRotationWFR = Quaternion.identity;
		Quaternion syncRotationWRL = Quaternion.identity;
		Quaternion syncRotationWRR = Quaternion.identity;

		stream.Serialize(ref syncRotationWFL);
		stream.Serialize(ref syncRotationWFR);
		stream.Serialize(ref syncRotationWRL);
		stream.Serialize(ref syncRotationWRR);

		_syncRotationWFLStart = trans.Find ("Wheels/FL").rotation;
		_syncRotationWFLEnd = syncRotationWFL;
		_syncRotationWFRStart = trans.Find ("Wheels/FR").rotation;
		_syncRotationWFREnd = syncRotationWFR;
		_syncRotationWRLStart = trans.Find ("Wheels/RL").rotation;
		_syncRotationWRLEnd = syncRotationWRL;
		_syncRotationWRRStart = trans.Find ("Wheels/RR").rotation;
		_syncRotationWRREnd = syncRotationWRR;
	}

}
