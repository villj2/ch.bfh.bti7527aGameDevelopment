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
		GameObject missile = (GameObject)Network.Instantiate(Resources.Load("PrefabMissile"),  Vector3.zero, Quaternion.identity, 0);
		missile.GetComponent<MissileBehaviour> ().Launch (transform);
	}
}
