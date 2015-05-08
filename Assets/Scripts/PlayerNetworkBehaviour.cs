using UnityEngine;
using System.Collections;

public class PlayerNetworkBehaviour : BasicNetworkBehaviour {

	protected override void OnOpponentUpdate() {
		GetComponent<CarBehaviour>().enabled = false;
		Transform trans = GetComponent<Rigidbody> ().transform;
		trans.Find("Turret").GetComponent<TurretBehavioiur>().enabled = false;
		trans.Find ("Wheels/FL").GetComponent<WheelBehaviour>().enabled = false;
		trans.Find ("Wheels/FR").GetComponent<WheelBehaviour>().enabled = false;
		trans.Find ("Wheels/RL").GetComponent<WheelBehaviour>().enabled = false;
		trans.Find ("Wheels/RR").GetComponent<WheelBehaviour>().enabled = false;
	}

}
