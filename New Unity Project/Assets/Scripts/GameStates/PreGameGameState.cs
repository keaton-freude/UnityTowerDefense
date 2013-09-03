using System;
using UnityEngine;


public class PreGameGameState: GameState
{
    public int NumberPlayersConnected = 1;
    public int NumberPlayersNeeded = 1;
    public GUISkin InGamePregameStyle;

    public PreGameGameState(NetworkManager manager, GUISkin skin)
    {
        this.networkManager = manager;
        InGamePregameStyle = skin;
    }

    public override void OnGUI()
    {
        GUI.skin = InGamePregameStyle;

        GUI.Label(new Rect(Screen.width * .25f, Screen.height * .3f, 800, 150), string.Format("CURRENTLY # CONNECTED PLAYERS {0}/{1}", NumberPlayersConnected, NumberPlayersNeeded), InGamePregameStyle.customStyles[0]);

        GUI.skin = null;
    }

    public override void Update()
    {
        if (NumberPlayersConnected == NumberPlayersNeeded)
        {
            //Move onto the next gamestate!
            //gameState = GAMESTATE.UTD_GAMESTATE_COUNTDOWNTOPLAYGAME;

            networkManager.StateStack.Push(new CountdownGameState(networkManager, InGamePregameStyle));
        }
    }

    public void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player Connected");
        NumberPlayersConnected++;
    }
}

