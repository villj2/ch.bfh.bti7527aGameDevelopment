using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingsBehaviour : MonoBehaviour {

	public GameObject Car { get; set; }

	public Dictionary<string, int> Points { get; set; }

	void Start () {
		// Persist object
		Object.DontDestroyOnLoad (gameObject);

		// Player points
		Points = new Dictionary<string, int>();
		Points ["test"] = 33;
	}

	void Update()
	{
		/*var keys = Points.Keys;
		foreach (string key in keys) {
			Debug.Log (Points[key]);
		}*/
	}
}