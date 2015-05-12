using UnityEngine;
using System.Collections;

public class TurretBehavioiur : MonoBehaviour {

	public GameObject Missile;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftControl))
		{
			LaunchMissile();
		}
	}

	private void LaunchMissile()
	{
		GameObject missile = (GameObject)Instantiate(Resources.Load("PrefabMissile"));
		missile.GetComponent<MissileBehaviour> ().Launch (transform);
	}
}
