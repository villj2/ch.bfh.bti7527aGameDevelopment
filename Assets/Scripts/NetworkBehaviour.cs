﻿using UnityEngine;
using System.Collections;

public class NetworkBehaviour : MonoBehaviour 
{
	private const string typeName = "ch.bfh.bti7527aGameDevelopment";
	private string gameName = "MyGame";
	private HostData[] hostList;
	public GameObject CarPrefab;
	private int hostId = -1;

	private SettingsBehaviour settings;

	void Start () {
		settings = gameObject.GetComponent<SettingsBehaviour> ();
	}
	
	private void StartServer()
	{   Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized()
	{   
		StartGame();
	}
	
	void OnGUI()
	{   if (!Network.isClient && !Network.isServer) {
			gameName = GUILayout.TextField(gameName, 25);

			if (GUILayout.Button ("Start Server"))
				StartServer ();
			
			if (GUILayout.Button ("Refresh Hosts"))
				MasterServer.RequestHostList (typeName);
			
			if (hostList != null) {
				for (int i = 0; i < hostList.Length; i++) {
					if (GUILayout.Button (hostList [i].gameName)) {
						// do not connect now because level needs to be loaded first or server will send to wrong level
						hostId = i;
						StartGame ();
					}

				}
			}
		} else {
			if (GUILayout.Button("Disconnect"))
			{
				Network.Disconnect(200);
				Application.LoadLevel (0);
			}
		}
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{   if (msEvent == MasterServerEvent.HostListReceived)
		hostList = MasterServer.PollHostList();
	}
	
	void OnConnectedToServer()
	{   
		SpawnPlayer ();
	}
	
	private void StartGame()
	{   
		Application.LoadLevel (1);

	}

	void OnLevelWasLoaded (int level) {
		if (level == 1) {
			if (Network.isServer) {
				SpawnPlayer ();
			} else if (!Network.isClient && hostId > -1) {
				Network.Connect (hostList [hostId]);
			}
		} else if (level == 0) {
			// reset cursor 
			Cursor.visible = true;
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}

	private void SpawnPlayer() {
		settings.Car = (GameObject) Network.Instantiate(CarPrefab, new Vector3(85f,20f,12f), Quaternion.identity, 0);
	}

	void OnPlayerDisconnected (NetworkPlayer player)
	{
		// Removing player from network and scene
		Network.RemoveRPCs(player, 0);
		Network.DestroyPlayerObjects(player);
	}

	void OnApplicationQuit() {
		OnPlayerDisconnected (Network.player);
	}
	
}
