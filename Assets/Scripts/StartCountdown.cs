using UnityEngine;
using System.Collections;

public class StartCountdown : MonoBehaviour {

	public GUIText GUITime;
	public int CountMax = 3; // sek

	private int _countdown; // sek
	private CarBehaviour _carScript;

	// Use this for initialization
	void Start () {

		_carScript = GameObject.Find ("COSWORTH").GetComponent<CarBehaviour> ();
		_carScript.enabled = false;

		print ("Begin Start " + Time.time);

		StartCoroutine (GameStart ());

		print ("End Start " + Time.time);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator GameStart()
	{
		for (_countdown = CountMax; _countdown > 0; _countdown--)
		{
			
			GUITime.text = _countdown.ToString("0");
			yield return new WaitForSeconds(1);
		}

		_carScript.enabled = true;
	}
}
