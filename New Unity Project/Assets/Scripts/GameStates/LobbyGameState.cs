using System;
using UnityEngine;
using System.Collections.Generic;

public class LobbyPlayerInfo
{
    public int SelectedID = 0;
    public string SelectedRace = "";
    public bool Enabled = false;
    public string name = "Arctic Monkey";
    public ComboBox RaceComboBox;

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


    LobbyPlayerInfo[] players;

    public LobbyGameState(NetworkManager manager)
    {
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
        GUI.Box(new Rect(Screen.width * .35f, Screen.height * .15f, Screen.width * .3f, Screen.height * .55f), "Pre-Game Lobby");
        GUI.Box(new Rect(Screen.width * .375f, Screen.height * .18f, Screen.width * .25f, Screen.height * .19f), "Team 1 - Top", "window");
		GUI.depth = 0;

  		GUI.depth = 1;
        GUI.Box(new Rect(Screen.width * .375f, Screen.height * .39f, Screen.width * .25f, Screen.height * .19f), "Team 2 - Bottom", "window");
		GUI.depth = 0;
		PlayerGUI(0, 0);
        PlayerGUI(0, 1);
        PlayerGUI(0, 2);
        PlayerGUI(1, 3);
        PlayerGUI(1, 4);
        PlayerGUI(1, 5);
		
		GUI.enabled = false;
		if (GUI.Button (new Rect(Screen.width * (PlayerNameRectOffset + PlayerNameWidth + .01f) + 100 + Screen.width * .01f, 
			Screen.height * (PlayerNameHeightOffset + (0 * PlayerNameHeightPerID) + (0 * TeamHeightOffset)), 
			38, 20), "Join"))
		{
			
		}
		GUI.enabled = true;
		
		
		
		GUI.depth = 1;
        GUI.skin = null;
    }

    public void PlayerGUI(int team, int id)
    {
        GUI.Label(new Rect(Screen.width * PlayerNameRectOffset, Screen.height * (PlayerNameHeightOffset + (id * PlayerNameHeightPerID) + (team * TeamHeightOffset)), Screen.width * PlayerNameWidth, Screen.height * PlayerNameHeight), players[id].name, "textarea");
        //Debug.Log("ID: " + (PlayerNameHeightOffset + (id * PlayerNameHeightPerID
		
        players[id].RaceComboBox.Show();
    }

    public override void Update()
    {
        
    }
}