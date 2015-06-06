﻿using UnityEngine;
using System.Collections;

public class PlayerNetworkBehaviour : BasicNetworkBehaviour {
	

	private WheelSynchronizer wfl;
	private WheelSynchronizer wfr;
	private WheelSynchronizer wrl;
	private WheelSynchronizer wrr;
	private TurretSynchronizer turretSynchronizer;

	protected override void initialize ()
	{
		base.initialize ();
		Transform trans = rigidbody.transform;
		wfl = new WheelSynchronizer (trans.Find ("Wheels/FL"));
		wfr = new WheelSynchronizer (trans.Find ("Wheels/FR"));
		wrl = new WheelSynchronizer (trans.Find ("Wheels/RL"));
		wrr = new WheelSynchronizer (trans.Find ("Wheels/RR"));
		turretSynchronizer = new TurretSynchronizer (trans.Find("Turret"));
	}

	protected override void initializeOpponent ()
	{
		base.initializeOpponent ();
		GetComponent<CarBehaviour>().enabled = false;
		wfl.DisableWheelScript ();
		wfr.DisableWheelScript ();
		wrl.DisableWheelScript ();
		wrr.DisableWheelScript ();
		turretSynchronizer.DisableTurretScript ();
	}

	protected override void SyncedMovement(float duration)
	{ 
		base.SyncedMovement (duration);

		wfl.SyncedMovement (duration);
		wfr.SyncedMovement (duration);
		wrl.SyncedMovement (duration);
		wrr.SyncedMovement (duration);
		turretSynchronizer.SyncedMovement (duration);

	}
	
	protected override void OnOutgonigSync(BitStream stream, NetworkMessageInfo info) {
		base.OnOutgonigSync (stream, info);

		wfl.OnOutgonigSync (stream, info);
		wfr.OnOutgonigSync (stream, info);
		wrl.OnOutgonigSync (stream, info);
		wrr.OnOutgonigSync (stream, info);
		turretSynchronizer.OnOutgonigSync (stream, info);
	}
	
	protected override void OnIncomingSync(BitStream stream, NetworkMessageInfo info) {
		base.OnIncomingSync (stream, info);

		wfl.OnIncomingSync (stream, info);
		wfr.OnIncomingSync (stream, info);
		wrl.OnIncomingSync (stream, info);
		wrr.OnIncomingSync (stream, info);
		turretSynchronizer.OnIncomingSync (stream, info);
	}

}
