using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GametimeBehaviour : MonoBehaviour {

	public int TIME = 180; // seconds

	private int _gameTime = 0;
	private NetworkView _networkView;
	private GUIText _guiGametime;
	private GUIText _guiEnd;
	private Dictionary<string, int> _points;

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("GameTimeBehaviour start");

		_networkView = GetComponent<NetworkView> ();
		_guiGametime = GameObject.Find("GUIGametime").GetComponent<GUIText>();
		_guiEnd = GameObject.Find("GUIGameEnd").GetComponent<GUIText>();
		_points = GameObject.Find ("SettingsContainer").GetComponent<SettingsBehaviour> ().Points;

		// FIXME just4testing
		//_points [Network.player.ipAddress] = 333;
		//_points ["192.168.0.3"] = 111;

		if (Network.isServer)
		{
			_gameTime = TIME;

			StartCoroutine(Count ());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Count()
	{
		while (_gameTime >= 0)
		{
			yield return new WaitForSeconds(1);
			
			GameTimeUpdate (_gameTime, _gameTime <= 0);
			_networkView.RPC("GameTimeUpdate", RPCMode.AllBuffered, _gameTime, _gameTime <= 0);
			
			_gameTime--;
		}
	}

	[RPC] void GameTimeUpdate(int seconds, bool gameEnd = false)
	{
		if (gameEnd)
		{
			var points = GameObject.Find ("SettingsContainer").GetComponent<SettingsBehaviour> ().Points;
			var pointsList = _points.Values.ToList().OrderByDescending(x => x).ToList();
			var test = _points.OrderByDescending(x => x.Value).ToList();

			int i = 0;
			foreach(var e in test)
			{
				if(e.Key == Network.player.ipAddress) {
					break;
				}
				i++;
			}

			_guiEnd.text = (i+1).ToString() + ".";
			_guiGametime.text = "0 Sekunden";
		}
		else
		{
			_guiGametime.text = seconds.ToString () + " Sekunden";
		}
	}
}
