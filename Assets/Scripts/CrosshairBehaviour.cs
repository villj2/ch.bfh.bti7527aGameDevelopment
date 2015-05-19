using UnityEngine;
using System.Collections;

public class CrosshairBehaviour : MonoBehaviour {

	public Sprite Crosshair { get; set; }

	// Use this for initialization
	void Start () {
		Object.DontDestroyOnLoad (gameObject);
	
	}
	

}
