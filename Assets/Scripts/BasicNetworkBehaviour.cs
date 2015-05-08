using UnityEngine;
using System.Collections;

/**
 * Basic synchronisation for postion and rotation
 */
public class BasicNetworkBehaviour : MonoBehaviour {

	protected Rigidbody rigidbody;

	private float _syncTimeLast = 0f;
	private float _syncDelay = 0f;
	private float _syncTime = 0f;
	private Vector3 _syncPosStart = Vector3.zero;
	private Vector3 _syncPosEnd = Vector3.zero;
	private Quaternion _syncRotationStart = Quaternion.identity;
	private Quaternion _syncRotationEnd = Quaternion.identity;

	void Start() {
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Update()
	{ 
		if (Application.loadedLevel == 1 && !GetComponent<NetworkView> ().isMine) {
			_syncTime += Time.deltaTime;
			SyncedMovement (_syncTime / _syncDelay);
			OnOpponentUpdate();
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// return early if the rigidbody has not been set
		if (!rigidbody) {
			return;
		}

		if (stream.isWriting)
		{
			OnOutgonigSync(stream, info);
		}
		else
		{
			OnIncomingSync(stream, info);
		}
	}
	
	protected virtual void SyncedMovement(float duration)
	{ 
		rigidbody.position = Vector3.Lerp(_syncPosStart, _syncPosEnd, duration);
		rigidbody.rotation = Quaternion.Lerp (_syncRotationStart, _syncRotationEnd, duration);
	}

	protected virtual void OnOpponentUpdate() {
	}

	protected virtual void OnOutgonigSync(BitStream stream, NetworkMessageInfo info) {
		Vector3 syncPos = rigidbody.position;
		Quaternion syncRotation = rigidbody.rotation;
		stream.Serialize(ref syncPos);
		stream.Serialize(ref syncRotation);
	}

	protected virtual void OnIncomingSync(BitStream stream, NetworkMessageInfo info) {
		Vector3 syncPos = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		stream.Serialize(ref syncPos);
		stream.Serialize(ref syncRotation);
		_syncTime = 0f;
		_syncDelay = Time.time - _syncTimeLast;
		_syncTimeLast = Time.time;
		_syncPosStart = rigidbody.position;
		_syncPosEnd = syncPos;
		_syncRotationStart = rigidbody.rotation;
		_syncRotationEnd = syncRotation;
	}
}
