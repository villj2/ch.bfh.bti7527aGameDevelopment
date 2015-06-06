using UnityEngine;
using System.Collections;

public class TurretBehavioiur : MonoBehaviour {

	public GameObject Missile;

	public float MissileCountDown;

	private Color _turretColor;
	private Color _turretColorReady;



	private bool  _canFire ;
	private float _missileCountDownHelper;

	// Use this for initialization
	void Start () {

		_canFire = false;
		_missileCountDownHelper = MissileCountDown;
	
		//_turretColor = GameObject.Find ("Turret").GetComponent<Renderer>.materials[0].color;
		//_turretColorReady = new Color(0f,204f,0f);
	
	}
	
	// Update is called once per frame
	void Update () {

		if (_canFire) {
			//_GUINextMissile.text = "Missile ready!";

		//	GameObject.Find("Turret").transform.renderer.materials[0].color = _turretColorReady;

		} else {
			//_GUINextMissile.text = string.Format("Missile ready in {0} s",MissileCountDown.ToString())  ;
		//	GameObject.Find("Turret").transform.renderer.materials[0].color = _turretColor;
		}

		MissileCountDown -= Time.deltaTime;
		if (MissileCountDown <= 0.0f) {
			_canFire = true;
			MissileCountDown = _missileCountDownHelper;

		}

		if (Input.GetMouseButtonDown(0) && _canFire )
		{
			LaunchMissile(transform);
			_canFire = false;
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			for(int i = 0; i < 36; i++){
				Transform c;
				c = transform;
				c.RotateAround(transform.position , Vector3.right,10f*i);
				LaunchMissile(c);

//				Debug.Log (string.Format ("dani {0}, {1}, {2}", c.position

			}
		}
	}

	private void LaunchMissile(Transform trans)
	{
		GameObject missile = (GameObject)Network.Instantiate(Resources.Load("PrefabMissile"),  trans.position, trans.rotation, 0);
		missile.GetComponent<MissileBehaviour> ().Launch ();
	}
}
