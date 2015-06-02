using UnityEngine;
using System.Collections;

public class GUIBehaviour : MonoBehaviour {

	public GUIText GUITime;
	public GUIText GUICountdown;
	public WheelCollider WheelColFL;
	public GUIText GUInextMissile;
	
	private float _pastTime = 0;
	private bool _isFinished;
	private CarBehaviour _carScript;

	// Use this for initialization
	void Start () {

		_carScript = GameObject.Find ("COSWORTH").GetComponent<CarBehaviour> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_carScript.enabled)
		{
			WheelHit hit;
			if (WheelColFL.GetGroundHit (out hit))
			{
				if(hit.collider.gameObject.tag == "Finish")
				{
					_isFinished = true;

					GUICountdown.text = "WIN!";
					GUICountdown.enabled = true;
				}
			}
			
			if (!_isFinished)
			{
				_pastTime += Time.deltaTime;
			}
			
			GUITime.text = _pastTime.ToString ("0.0") + " sec";
		}
	}
}
