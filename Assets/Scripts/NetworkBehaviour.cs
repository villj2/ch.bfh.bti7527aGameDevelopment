using UnityEngine;
using System.Collections;

public class NetworkBehaviour : MonoBehaviour 
{
	private const string typeName = "ch.bfh.bti7527aGameDevelopment";
	private string gameName = "MyGame";
	private HostData[] hostList;
	public GameObject CarPrefab;
	private NetworkView _networkView;
	private int hostId = -1;
	private Vector3[] spawnPoints;

	private SettingsBehaviour settings;

	void Start () {
		_networkView = gameObject.AddComponent<NetworkView> ();
		settings = gameObject.GetComponent<SettingsBehaviour> ();

		spawnPoints = new Vector3[] {new Vector3 (85f, 20f, 12f), new Vector3 (96f, 20f, 317f), new Vector3 (368f, 24f, 449f)};
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
		_networkView.RPC("RequestSpawnPos", RPCMode.AllBuffered, Network.player);
	}
	
	private void StartGame()
	{   
		Application.LoadLevel (1);

	}

	void OnLevelWasLoaded (int level) {
		if (level == 1) {
			if (Network.isServer) {
				SpawnPlayer (0);
			} else if (!Network.isClient && hostId > -1) {
				Network.Connect (hostList [hostId]);
			}
		} else if (level == 0) {
			// reset cursor 
			Cursor.visible = true;
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}

	private void SpawnPlayer(int spawnIndex) {
		settings.Car = (GameObject) Network.Instantiate(CarPrefab, spawnPoints[spawnIndex], Quaternion.identity, 0);
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

	[RPC] void RequestSpawnPos(NetworkPlayer player)
	{
		if (Network.isServer) {
			NetworkPlayer p;
			for (int i = 0; i < Network.connections.Length; i++) {
				p = Network.connections[i];
				if (p == player) {
					_networkView.RPC("RespondSpawnPos", RPCMode.AllBuffered, (i+1) % 3, p);
					break;
				}
			}
		}
	}

	[RPC] void RespondSpawnPos(int spawnIndex, NetworkPlayer player)
	{
		if (Network.player == player) {
			SpawnPlayer(spawnIndex);
		}
	}
}
