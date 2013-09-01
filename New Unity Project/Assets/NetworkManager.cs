using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour 
{
	private bool InPregameState = true;
	private int NumberPlayersConnected = 1;
	private int NumberPlayersNeeded = 2;
	
	public enum GAMESTATE
	{
		UTD_GAMESTATE_NETWORKINIT = 0,
		UTD_GAMESTATE_PREGAME = 1,
		UTD_GAMESTATE_PLAYGAME = 2,
		UTD_GAMESTATE_POSTGAME = 3,
		UTD_GAMESTATE_MAINMENU = 4,
		UTD_GAMESTATE_COUNTDOWNTOPLAYGAME = 5
	}
	private float PreGameCountdown = 15.0f;
	private GAMESTATE gameState;
	
	private int TopLives = 30;
	private int BotLives = 30;
	
	public GUISkin InGamePregameStyle;
	// Use this for initialization
	void Start () 
	{
		gameState = GAMESTATE.UTD_GAMESTATE_MAINMENU;
		
		Debug.Log ("Network managered start()");
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(gameState)
		{
		case GAMESTATE.UTD_GAMESTATE_PREGAME:
			//if we're the server, check constantly if we're ready to start (everyones in)
			if (NumberPlayersConnected == NumberPlayersNeeded)
			{
				//Move onto the next gamestate!
				gameState = GAMESTATE.UTD_GAMESTATE_COUNTDOWNTOPLAYGAME;
			}
			break;
		case GAMESTATE.UTD_GAMESTATE_COUNTDOWNTOPLAYGAME:
			//To get some buffer room, we're going to start a 15 second countdown before players can start
			//building and sending
			PreGameCountdown -= Time.deltaTime;
			if (PreGameCountdown <= 0f)
				gameState = GAMESTATE.UTD_GAMESTATE_PLAYGAME;
			break;
		case GAMESTATE.UTD_GAMESTATE_PLAYGAME:
			break;
		case GAMESTATE.UTD_GAMESTATE_POSTGAME:
			break;
		default:
			break;
		}
	}
	
	public void MainMenuGUI()
	{
		
		if (GUI.Button (new Rect(50, 90, 100, 45), "Create Server"))
		{
			Debug.Log ("Registering Server");
			Network.InitializeServer(1, 25000, !Network.HavePublicAddress());
			MasterServer.RegisterHost("UnityTowerDefense_freudek", "Keaton's Game", "The default game");
//			AtMainMenu = false;
			DontDestroyOnLoad(this.gameObject);
			Application.LoadLevel ("MainScene");
		}
		
		if (GUI.Button (new Rect(165, 90, 130, 45), "Refresh Server List"))
		{
			MasterServer.RequestHostList("UnityTowerDefense_freudek");
		}
		
		GUI.Label (new Rect(50, 140, 300, 25), "CURRENTLY CREATED SERVERS");
		
		Rect horiztonalRuleRect = new Rect(50, 150, Screen.width, 25);
		string horiztonalRuleString = "______________________________";
		
		GUI.Label(horiztonalRuleRect, horiztonalRuleString);
	
		HostData[] data = MasterServer.PollHostList();
		// Go through all the hosts in the host list
		foreach (HostData element in data)
		{
			GUILayout.BeginArea (new Rect(50, 175, 500, Screen.height));
			
			GUILayout.BeginHorizontal();
			
			var name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
			GUILayout.Label(name);	
			//GUILayout.Space(5);
			string hostInfo = "";
			hostInfo = "[";
			foreach (var host in element.ip)
				hostInfo = hostInfo + host + ":" + element.port + " ";
			hostInfo = hostInfo + "]";
			GUILayout.Label(hostInfo);	
			//GUILayout.Space(5);
			GUILayout.Label(element.comment);
			//GUILayout.Space(5);
			if (GUILayout.Button("Connect"))
			{
				// Connect to HostData struct, internally the correct method is used (GUID when using NAT).
				NetworkConnectionError e = Network.Connect(element);	
				Debug.Log (e);
				DontDestroyOnLoad(this.gameObject);
				Application.LoadLevel ("MainScene");
				
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			
		}
	}
	
	public void PreGameGUI()
	{
		/* might be able to handle everything via Callbacks and RPCs, but we'll
		 * keep this function stubbed out in case we need to do something either through
		 * the GUI or in the update loop */
		
		/* ON GUI */
		//Display the current number of connected players over the number we need to start
		//Need to style this a bit
		
		GUI.skin = InGamePregameStyle;
		
		GUI.Label(new Rect(Screen.width * .25f, Screen.height * .3f, 800, 150), string.Format("CURRENTLY # CONNECTED PLAYERS {0}/{1}", NumberPlayersConnected, NumberPlayersNeeded), InGamePregameStyle.customStyles[0]);
		
		GUI.skin = null;
	}
	
	public void CountdownGUI()
	{
		GUI.skin = InGamePregameStyle;
		
		GUI.Label(new Rect(Screen.width * .25f, Screen.height * .3f, 800, 150), string.Format("Game Starts In: {0} seconds", Mathf.RoundToInt(PreGameCountdown)), InGamePregameStyle.customStyles[0]);
		
		GUI.skin = null;
	}
	
	public void PlayGameGUI()
	{
		GUI.Label (new Rect(Screen.width * .4f, 0, 150, 45), string.Format("Top Lives: {0}", TopLives));
		GUI.Label (new Rect(Screen.width * .6f, 0, 150, 45), string.Format("Bot Lives: {0}", BotLives));
	}
	
	public void OnGUI()
	{
		switch(gameState)
		{
		case GAMESTATE.UTD_GAMESTATE_MAINMENU:
			MainMenuGUI();
			break;
		case GAMESTATE.UTD_GAMESTATE_PREGAME:
			PreGameGUI();
			break;
		case GAMESTATE.UTD_GAMESTATE_COUNTDOWNTOPLAYGAME:
			CountdownGUI();
			break;
		case GAMESTATE.UTD_GAMESTATE_PLAYGAME:
			PlayGameGUI();
			break;
		default:
			break;
		}
		
		GUI.Label (new Rect(0, 0, 200, 45), gameState.ToString());
	}
	
	[RPC]
	public void DoDamageToMob(NetworkViewID id, int amount)
	{
		NetworkView.Find (id).gameObject.GetComponent<MobStats>().TakeDamage(amount);
	}
	
	public void OnFailedToConnect(NetworkConnectionError e)
	{
		Debug.Log (e);
	}
	
	public void OnPlayerConnected(NetworkPlayer player)
	{
		Debug.Log ("Player Connected");
		NumberPlayersConnected++;
		networkView.RPC("NumberOfPlayers", RPCMode.Others, NumberPlayersConnected);
	}
	
	[RPC]
	public void NumberOfPlayers(int NumberOfConnectedPlayers)
	{
		NumberPlayersConnected = NumberOfConnectedPlayers;
	}
	
	public void OnServerInitialized()
	{
		/* Server created, move this player to PREGAME state */
		gameState = GAMESTATE.UTD_GAMESTATE_PREGAME;
		
		Debug.Log ("server init");
	}
	
	public void OnConnectedToServer()
	{
		/* We joined the game, lets move ourselves into PREGAME */
		gameState = GAMESTATE.UTD_GAMESTATE_PREGAME;
	}
}
