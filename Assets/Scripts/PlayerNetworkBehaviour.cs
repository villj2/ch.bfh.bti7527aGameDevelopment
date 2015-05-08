using UnityEngine;
using System.Collections;

public class PlayerNetworkBehaviour : BasicNetworkBehaviour {

	protected override void OnOpponentUpdate() {
		GetComponent<CarBehaviour>().enabled = false;
		GetComponent<Rigidbody> ().transform.Find("Turret").GetComponent<TurretBehavioiur>().enabled = false;
	}

}
