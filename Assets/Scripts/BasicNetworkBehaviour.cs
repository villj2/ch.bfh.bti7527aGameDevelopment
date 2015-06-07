using UnityEngine;
using System.Collections;

/**
 * Basic synchronisation for postion and rotation
 */
public class BasicNetworkBehaviour : MonoBehaviour {

	protected bool isInitialized = false;
	protected Rigidbody rigidbody;

	private bool isOpponent;
	private float _syncTimeLast = 0f;
	private float _syncDelay = 0f;
	private float _syncTime = 0f;
	private Vector3 _syncPosStart = Vector3.zero;
	private Vector3 _syncPosEnd = Vector3.zero;
	private Quaternion _syncRotationStart = Quaternion.identity;
	private Quaternion _syncRotationEnd = Quaternion.identity;

	void Start() {
		isOpponent = !GetComponent<NetworkView> ().isMine;
		initialize ();
		if (isOpponent) {
			initializeOpponent();
		}
		isInitialized = true;
	}

	void FixedUpdate()
	{ 
		if (Application.loadedLevel == 1 && isOpponent) {
			_syncTime += Time.deltaTime;
			SyncedMovement (_syncTime / _syncDelay);
			OnOpponentUpdate();
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// return early if not initialized
		if (!isInitialized) {
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

	protected virtual void initialize() {
		rigidbody = GetComponent<Rigidbody> ();
	}

	protected virtual void initializeOpponent() {
	}
	
	protected virtual void SyncedMovement(float duration)
	{ 

		rigidbody.position = BasicNetworkBehaviour.LerpVector3 (_syncPosStart, _syncPosEnd, duration);
		rigidbody.rotation = BasicNetworkBehaviour.LerpQuaternion (_syncRotationStart, _syncRotationEnd, duration);
	}

	public static Quaternion LerpQuaternion(Quaternion start, Quaternion end, float duration)
	{
		if (duration == 0f || float.IsNaN (duration) || float.IsInfinity(duration) || isQuaternionNAN (start) || isQuaternionZero (start) || isQuaternionNAN (end) || isQuaternionZero (end)) {
			if (float.IsInfinity(duration) && !isQuaternionNAN (start) && !isQuaternionZero (start)) {
				return start;
			} else if (!isQuaternionNAN (end) && !isQuaternionZero (end)) {
				return end;
			} else if (!isQuaternionNAN (start) && !isQuaternionZero (start)) {
				return start;
			}
			return Quaternion.identity;
		} else {
			return Quaternion.Lerp (start, end, duration);
		}
	}

	public static Vector3 LerpVector3(Vector3 start, Vector3 end, float duration)
	{
		if (float.IsInfinity (duration)) {
			return start;
		}else if (duration == 0f || float.IsNaN (duration)) {
			return end;
		} else {
			return Vector3.Lerp(start, end, duration);
		}
	}

	private static bool isQuaternionNAN(Quaternion q) {
		return float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);
	}

	private static bool isQuaternionZero(Quaternion q){
		return (q.x  == 0f) && (q.y  == 0f) && (q.z  == 0f) && (q.w  == 0f);
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
