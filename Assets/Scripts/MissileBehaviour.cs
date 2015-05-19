using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissileBehaviour : MonoBehaviour {

	public float LaunchForce = 0;
	public AudioClip AudioClipLaunch;
	public AudioClip AudioClipExplosion1;
	public AudioClip AudioClipExplosion2;
	public AudioClip AudioClipExplosion3;
	
	private AudioSource _audioSourceLaunch;
	private AudioSource	_audioSourceExplosion1;
	private AudioSource	_audioSourceExplosion2;
	private AudioSource	_audioSourceExplosion3;

	private Dictionary<string, int> _points;
	private GUIText _guiPoints;
	private NetworkView _networkView;

	// Use this for initialization
	void Start () {

		_points = GameObject.Find ("SettingsContainer").GetComponent<SettingsBehaviour> ().Points;
		_guiPoints = GameObject.Find ("GUIPoints").GetComponent<GUIText> ();
		_networkView = GetComponent<NetworkView> ();
		
		//_audioSourceExplosion = (AudioSource)gameObject.GetComponent<AudioSource> ();

		_audioSourceLaunch = (AudioSource)gameObject.AddComponent<AudioSource>();
		_audioSourceLaunch.clip = AudioClipLaunch;
		_audioSourceLaunch.loop = false;
		_audioSourceLaunch.volume = 0.7f;
		_audioSourceLaunch.playOnAwake = true;
		_audioSourceLaunch.Play ();

		_audioSourceExplosion1 = (AudioSource)gameObject.AddComponent<AudioSource>();
		_audioSourceExplosion1.clip = AudioClipExplosion1;
		_audioSourceExplosion1.loop = false;
		_audioSourceExplosion1.volume = 1f;
		_audioSourceExplosion1.playOnAwake = false;

		_audioSourceExplosion2 = (AudioSource)gameObject.AddComponent<AudioSource>();
		_audioSourceExplosion2.clip = AudioClipExplosion2;
		_audioSourceExplosion2.loop = false;
		_audioSourceExplosion2.volume = 1f;
		_audioSourceExplosion2.playOnAwake = false;

		_audioSourceExplosion3 = (AudioSource)gameObject.AddComponent<AudioSource>();
		_audioSourceExplosion3.clip = AudioClipExplosion3;
		_audioSourceExplosion3.loop = false;
		_audioSourceExplosion3.volume = 1f;
		_audioSourceExplosion3.playOnAwake = false;

		StartCoroutine (Detonate ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Launch(Transform transformTurret)
	{
		transform.position = transformTurret.position;
		transform.rotation = transformTurret.rotation;
		
		transform.Rotate (Vector3.forward * 180);
		transform.Translate (Vector3.up * -0.5f, Space.Self);
		
		GetComponent<Rigidbody> ().AddForce (transform.forward * LaunchForce);
	}

	IEnumerator Detonate()
	{
		yield return new WaitForSeconds(1);

		GameObject explosion = (GameObject)Instantiate(Resources.Load("PrefabExplosion"));
		explosion.transform.position = transform.position;

		switch (Random.Range (0, 3)) {
		case 1:
			_audioSourceExplosion1.Play ();
			break;

		case 2:
			_audioSourceExplosion2.Play ();
			break;

		default:
			_audioSourceExplosion3.Play ();
			break;
		}

		// make sure only own missiles gather points
		if (GetComponent<NetworkView> ().isMine) 
		{
			GameObject[] cars = GameObject.FindGameObjectsWithTag("PlayerCar");
			
			foreach (GameObject car in cars) {
				float distanceToCar = Vector3.Distance (this.gameObject.transform.position, car.gameObject.transform.position);
				bool isOpponent = !car.GetComponent<NetworkView> ().isMine;
				
				//if(true)
				if(isOpponent)
				{
					int pointsGathered = Mathf.Max((int)(-100 / 12 * distanceToCar + 100), 0);
					
					// check if key exists
					int item;
					_points.TryGetValue(Network.player.ipAddress, out item);
					if(item == 0)_points[Network.player.ipAddress] = 0;
					
					// add points
					_points[Network.player.ipAddress] += pointsGathered;
					
					DisplayPoints();
					
					// sync Points-Dictionary
					_networkView.RPC("SharePoints", RPCMode.AllBuffered, Network.AllocateViewID(), Network.player.ipAddress, _points[Network.player.ipAddress]);
				}
			}
		}

		GetComponentInChildren<MeshRenderer> ().enabled = false;

		yield return new WaitForSeconds(4);

		Destroy (explosion);
		Destroy (this.gameObject);
	}

	private void DisplayPoints()
	{
		// loop through all keys (all players)
		string displayPoints = string.Empty;
		var keys = _points.Keys;
		foreach (string key in keys) {
			displayPoints += key + ": " + _points[key] + "\n";
		}
		
		// display points
		//_guiPoints.text = _points[Network.player.ipAddress] + " Points";
		_guiPoints.text = displayPoints;
	}

	[RPC] void SharePoints(NetworkViewID viewID, string playerId, int points)
	{
		_points[playerId] = points;
		DisplayPoints();
	}
}
