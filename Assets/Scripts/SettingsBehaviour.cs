using UnityEngine;
using System.Collections;

public class SettingsBehaviour : MonoBehaviour {

	public GameObject Car { get; set; }

	void Start () {
		// Persist object
		Object.DontDestroyOnLoad (gameObject);
	}



}
