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
	}
}