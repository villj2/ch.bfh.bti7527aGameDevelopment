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

	private AudioSource	_audioSourceExplosion;
	private AudioSource _audioSourceLaunch;

	// Use this for initialization
	void Start () {
		
		_audioSourceExplosion = (AudioSource)gameObject.GetComponent<AudioSource> ();

		_audioSourceLaunch = (AudioSource)gameObject.AddComponent<AudioSource>();
		_audioSourceLaunch.clip = AudioClipLaunch;
		_audioSourceLaunch.loop = false;
		_audioSourceLaunch.volume = 1f;
		_audioSourceLaunch.playOnAwake = true;
		_audioSourceLaunch.Play ();

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

		_audioSourceExplosion.Play ();
		GetComponentInChildren<MeshRenderer> ().enabled = false;

		yield return new WaitForSeconds(4);

		Destroy (this.gameObject);
	}
}
