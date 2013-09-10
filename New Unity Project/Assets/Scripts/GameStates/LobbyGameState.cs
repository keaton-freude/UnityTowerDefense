using System;
using UnityEngine;
using System.Collections.Generic;

public class LobbyPlayerInfo
{
    public int SelectedID = 0;
    public string SelectedRace = "";
    public bool PlayerJoined = false;
    public string name = "< Empty >";
    public ComboBox RaceComboBox;
    public int RaceIndex = 0;

    public LobbyPlayerInfo()
    {
    }


}

public class LobbyGameState : GameState
{
    public GUISkin skin;
    int listEntry = 0;
    bool selected = false;
    public float PlayerNameRectOffset = .38f;
    public float PlayerNameHeightOffset = .21f;
    public float PlayerNameHeightPerID = .05f;
    public float PlayerNameWidth = .12f;
    public float PlayerNameHeight = .04f;
    public float TeamHeightOffset = .06f;
    GUIContent[] racesList;
    private ComboBox comboBoxControl;
    private GUIStyle listStyle = new GUIStyle();
	public ListBox playersInLobby;

    public LobbyPlayerInfo[] players;

    public void SetRaceFromIndex(int playerid, int index)
    {
        players[playerid].SelectedRace = racesList[index].text;
        players[playerid].RaceComboBox.SelectedItemIndex = index;
    }

    public LobbyGameState(NetworkManager manager)
    {
		this.networkManager = manager;
		playersInLobby = new ListBox();

        skin = GameObject.Find("__NetworkManager").GetComponent<NetworkManager>().InGamePregameStyle;
        racesList = new GUIContent[3];
        racesList[0] = (new GUIContent("Fire"));
        racesList[1] = (new GUIContent("Ice"));
        racesList[2] = (new GUIContent("Lightning"));

        listStyle.normal.textColor = Color.white;
        listStyle.onHover.background =
        listStyle.hover.background = new Texture2D(2, 2);
        listStyle.padding.left =
        listStyle.padding.right =
        listStyle.padding.top =
        listStyle.padding.bottom = 4;

        comboBoxControl = new ComboBox(new Rect(50, 100, 100, 20), racesList[0], racesList, "button", "box", listStyle);

        players = new LobbyPlayerInfo[6];
        for (int i = 0; i < 6; ++i)
        {
            players[i] = new LobbyPlayerInfo();
        }
        SetupPlayerInfo(0, 0);
		SetupPlayerInfo(0, 1);
		SetupPlayerInfo(0, 2);
		SetupPlayerInfo(1, 3);
		SetupPlayerInfo(1, 4);
		SetupPlayerInfo(1, 5);
		
		//add ourselves to the lobby and update everyone
		playersInLobby.AddEntry(networkManager.accountName);
		

    }

    public string GetLobbyState()
    {
        //PlayerNameInLobby1, PlayerNameInLobby2, ...PARAM_SPLITPlayerNameTeam1Slot1, PlayerNameteam1Slot2, ...PARAM_SPLITRaceChoiceTeam1Slot1, RaceChoiceTeam1Slot2
        string state = "";
		
		if (playersInLobby.entryList.Count != 0)
		{
	        for (int i = 0; i < playersInLobby.entryList.Count - 1; ++i)
	        {
	            state += playersInLobby.entryList[i].name + ",";
	        }
	
	        state += playersInLobby.entryList[playersInLobby.entryList.Count - 1].name + "PARAM_SPLIT";
		}
		else
		{
			//write token for no players in Lobby so client can tell
			state += "UTD_NOPLAYERSINLOBBYPARAM_SPLIT";
		}

        for (int i = 0; i < players.Length - 1; ++i)
        {
            state += players[i].name + ",";
        }

        state += players[players.Length - 1].name + "PARAM_SPLIT";

        for (int i = 0; i < players.Length - 1; ++i)
        {
            state += players[i].SelectedID.ToString() + ",";
        }

        state += players[players.Length - 1].SelectedID.ToString();

        

        return state;
    }

    public void SetupPlayerInfo(int team, int id)
    {
    	players[id].RaceComboBox = new ComboBox(new Rect(Screen.width * (PlayerNameRectOffset + PlayerNameWidth + .01f), Screen.height * (PlayerNameHeightOffset + (id * PlayerNameHeightPerID) + (team * TeamHeightOffset)), 100, 20), racesList[0], racesList, "button", "box", listStyle);
       	Debug.Log (Screen.height * (PlayerNameHeightOffset + (id * PlayerNameHeightPerID) + (team * TeamHeightOffset)));
    }


    public override void OnGUI()
    {
        GUI.skin = skin;
		GUI.depth = 1;
        GUI.Box(new Rect(Screen.width * .1f, Screen.height * .14f, Screen.width * .55f, Screen.height * .55f), "Pre-Game Lobby - Players Needed in Slots: " + GetNumberPlayersInSlots() + "/" + (Network.maxConnections + 1));
		
		GUI.Label (new Rect(Screen.width * .12f, Screen.height * .17f, Screen.width * .1f, 20), "Players in Lobby");
		
		
        GUI.Box(new Rect(Screen.width * .375f, Screen.height * .18f, Screen.width * .265f, Screen.height * .19f), "Team 1 - Top", "window");
		GUI.depth = 0;

  		GUI.depth = 1;
        GUI.Box(new Rect(Screen.width * .375f, Screen.height * .39f, Screen.width * .265f, Screen.height * .19f), "Team 2 - Bottom", "window");
		GUI.depth = 0;
		PlayerGUI(0, 0);
        PlayerGUI(0, 1);
        PlayerGUI(0, 2);
        PlayerGUI(1, 3);
        PlayerGUI(1, 4);
        PlayerGUI(1, 5);
		

		playersInLobby.Draw (new Rect(Screen.width * .12f, Screen.height * .2f, Screen.width * .1f, Screen.height * .2f), 20f, Color.black, Color.white);
		
		/* Draw Start Game Button */
		if (Network.isServer)
		{
			//Debug.Log ("Connections: " + Network.connections.Length + " | Max Connections: " + Network.maxConnections);
			GUI.enabled = GetNumberPlayersInSlots() == Network.maxConnections + 1;
			if (GUI.Button(new Rect(Screen.width * .375f, Screen.height * .59f, Screen.width * .265f, Screen.height * .05f), "Start Game"))
			{
				//game is ready, lets start it up
				networkManager.networkView.RPC ("BeginGameFromLobby", RPCMode.All);
			}
			GUI.enabled = true;
		}
		else
		{
			/* print the message that host can start when game is full */
			GUI.enabled = false;
			GUI.Button(new Rect(Screen.width * .375f, Screen.height * .59f, Screen.width * .25f, Screen.height * .05f), "Host may start when all players have joined slots.");
			GUI.enabled = true;
		}
		
		GUI.depth = 1;
        GUI.skin = null;
    }
	
	public int GetNumberPlayersInSlots()
	{
		int amt = 0;
		
		foreach (LobbyPlayerInfo pi in players)
		{
			if (pi.PlayerJoined)
			{
				amt++;
			}
		}
		
		return amt;
	}

    public void PlayerGUI(int team, int id)
    {
        GUI.Label(new Rect(Screen.width * PlayerNameRectOffset, Screen.height * (PlayerNameHeightOffset + (id * PlayerNameHeightPerID) + (team * TeamHeightOffset)), Screen.width * PlayerNameWidth, Screen.height * PlayerNameHeight), players[id].name, "textarea");
		
		players[id].SelectedID = players[id].RaceComboBox.SelectedItemIndex;
		
		GUI.enabled = !players[id].PlayerJoined;
		if (GUI.Button (new Rect(Screen.width * (PlayerNameRectOffset + PlayerNameWidth + .01f) + 100 + Screen.width * .01f, 
			Screen.height * (PlayerNameHeightOffset + (id * PlayerNameHeightPerID) + (team * TeamHeightOffset)), 
			38, 20), "Join"))
		{
			/* check if we're already in a slot... */
			if (AlreadyInSlot(networkManager.accountName))
			{
				/* clean that slot up... */
				ResetSlot(networkManager.accountName);
			}
			
			Entry e = PlayerInLobby(networkManager.accountName);
			if (e != null)
				RemovePlayerInLobby(e);
			
			players[id].name = networkManager.accountName;
			players[id].PlayerJoined = true;
			networkManager.networkView.RPC("UpdateLobbyInfo", RPCMode.Others, GetLobbyState());
		}
		//Reset our state back so we don'tbreak subsequent calls
		GUI.enabled = true;
		
		players[id].RaceComboBox.Show();
    }
	
	float time = 0f;
	
	public Entry PlayerInLobby(string name)
	{
		foreach (Entry e in playersInLobby.entryList)
		{
			if (e.name == name)
			{
				return e;
			}
		}
		
		return null;
	}
	
	public void RemovePlayerInLobby(Entry entry)
	{
		playersInLobby.RemoveEntry(entry);
	}
	
	public bool AlreadyInSlot(string name)
	{
		foreach (LobbyPlayerInfo pi in players)
		{
			if (pi.name == name)
				return true;
		}
		return false;
	}
	
	public void ResetSlot(string name)
	{
		foreach (LobbyPlayerInfo pi in players)
		{
			if (pi.name == name)
			{
				pi.PlayerJoined = false;
				pi.name = "< Empty >";
			}
		}
	}

    public override void Update()
    {
       	time += Time.deltaTime;
		
		if (time > 1f)
		{
			Debug.Log ("1 second has passed");
			time -= 1f;
			networkManager.networkView.RPC("UpdateLobbyInfo", RPCMode.Others, GetLobbyState());
		}
		
    }
}