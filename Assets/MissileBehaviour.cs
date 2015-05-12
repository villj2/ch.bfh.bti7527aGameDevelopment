using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

	// TODO
	/*
	 * 1) Detonate (particle effect) after x seconds
	 * 2) Explosion -> Force to Car
	 * 
	 * */

	public float LaunchForce = 0;
	public AudioClip AudioClipLaunch;
	public AudioClip AudioClipExplosion1;
	public AudioClip AudioClipExplosion2;
	public AudioClip AudioClipExplosion3;
	
	private AudioSource _audioSourceLaunch;
	private AudioSource	_audioSourceExplosion1;
	private AudioSource	_audioSourceExplosion2;
	private AudioSource	_audioSourceExplosion3;

	// Use this for initialization
	void Start () {
		
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

		GetComponentInChildren<MeshRenderer> ().enabled = false;

		yield return new WaitForSeconds(4);

		Destroy (this.gameObject);
	}
}
