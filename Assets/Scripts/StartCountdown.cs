using UnityEngine;
using System.Collections;

public class StartCountdown : MonoBehaviour {

	public GUIText GUICountdown;
	public int CountMax = 3; // sek
	public AudioClip audioClipBeep1;
	public AudioClip audioClipBeep2;

	private int _countdown; // sek
	private CarBehaviour _carScript;
	private AudioSource _audioSourceBeep1;
	private AudioSource _audioSourceBeep2;

	// Use this for initialization
	void Start () {

		_carScript = GameObject.Find ("COSWORTH").GetComponent<CarBehaviour> ();
		_carScript.enabled = false;

		_audioSourceBeep1 = (AudioSource)gameObject.AddComponent<AudioSource>();
		_audioSourceBeep1.clip = audioClipBeep1;
		_audioSourceBeep1.loop = false;
		_audioSourceBeep1.volume = 0.7f;
		_audioSourceBeep1.playOnAwake = true;

		_audioSourceBeep2 = (AudioSource)gameObject.AddComponent<AudioSource>();
		_audioSourceBeep2.clip = audioClipBeep2;
		_audioSourceBeep2.loop = false;
		_audioSourceBeep2.volume = 0.7f;
		_audioSourceBeep2.playOnAwake = true;

		// Start Countdown in another thread
		StartCoroutine (GameStart ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator GameStart()
	{
		for (_countdown = CountMax; _countdown >= 0; _countdown--)
		{
			if(_countdown > 0)
			{
				_audioSourceBeep1.Play();
				GUICountdown.text = _countdown.ToString("0");
				
				yield return new WaitForSeconds(1);
			}
			else
			{
				_audioSourceBeep2.Play();
				_carScript.enabled = true;

				GUICountdown.text = "GO!";

				yield return new WaitForSeconds(2);

				GUICountdown.enabled = false;
			}
		}
	}
}
