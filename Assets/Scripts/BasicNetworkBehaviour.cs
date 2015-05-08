using UnityEngine;
using System.Collections;

/**
 * Basic synchronisation for postion and rotation
 */
public class BasicNetworkBehaviour : MonoBehaviour {

	private float _syncTimeLast = 0f;
	private float _syncDelay = 0f;
	private float _syncTime = 0f;
	private Vector3 _syncPosStart = Vector3.zero;
	private Vector3 _syncPosEnd = Vector3.zero;
	private Quaternion _syncRotationStart = Quaternion.identity;
	private Quaternion _syncRotationEnd = Quaternion.identity;

	void Update()
	{ 
		if (Application.loadedLevel == 1 && !GetComponent<NetworkView> ().isMine) {
			SyncedMovement ();
			OnOpponentUpdate();
		}
	}

	protected virtual void OnOpponentUpdate() {
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Rigidbody rb = GetComponent<Rigidbody> ();
		// Outgoing sync
		Vector3 syncPos = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		if (stream.isWriting)
		{
			syncPos = rb.position;
			syncRotation = rb.rotation;
			stream.Serialize(ref syncPos);
			stream.Serialize(ref syncRotation);
		}
		else // Incomming sync
		{
			stream.Serialize(ref syncPos);
			stream.Serialize(ref syncRotation);
			_syncTime = 0f;
			_syncDelay = Time.time - _syncTimeLast;
			_syncTimeLast = Time.time;
			_syncPosStart = rb.position;
			_syncPosEnd = syncPos;
			_syncRotationStart = rb.rotation;
			_syncRotationEnd = syncRotation;
		}
	}
	
	private void SyncedMovement()
	{ 
		_syncTime += Time.deltaTime;
		Rigidbody rb = GetComponent<Rigidbody> ();
		float t = _syncTime / _syncDelay;
		rb.position = Vector3.Lerp(_syncPosStart, _syncPosEnd, t);
		rb.rotation = Quaternion.Lerp (_syncRotationStart, _syncRotationEnd, t);
	}
}
