using UnityEngine;
using System.Collections;

public class MenuBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonStartClick()
	{
		Application.LoadLevel (1);
	}

	public void OnButtonMenuClick()
	{
		Application.LoadLevel (0);
	}
}
