﻿using UnityEngine;
using System.Collections;

public class NetworkBehaviour : MonoBehaviour 
{
	private const string typeName = "ch.bfh.bti7527aGameDevelopment";
	private const string gameName = "Game1";
	private HostData[] hostList;
	public GameObject CarPrefab;

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
		SpawnPlayer();
	}
	
	void OnGUI()
	{   if (!Network.isClient && !Network.isServer)
		{   if (GUILayout.Button("Start Server"))
			StartServer();
			
			if (GUILayout.Button("Refresh Hosts"))
				MasterServer.RequestHostList(typeName);
			
			if (hostList != null)
			{   for (int i = 0; i < hostList.Length; i++)
				{   if (GUILayout.Button(hostList[i].gameName))
					Network.Connect(hostList[i]);
				}
			}
		}
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{   if (msEvent == MasterServerEvent.HostListReceived)
		hostList = MasterServer.PollHostList();
	}
	
	void OnConnectedToServer()
	{   
		SpawnPlayer();
	}
	
	private void SpawnPlayer()
	{   
		Application.LoadLevel (1);

	}

	void OnLevelWasLoaded (int level) {
		if (level == 1) {
			settings.Car = (GameObject) Network.Instantiate(CarPrefab, new Vector3(85f,20f,12f), Quaternion.identity, 0);
		}
	}
	
}
