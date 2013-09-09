using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour 
{
    public UTDDatabase database;

    public Stack<GameState> StateStack = new Stack<GameState>();

    public GUITexture backgroundTexture;
	public GUISkin InGamePregameStyle;
	
	public string accountName = "NOT LOGGED IN";

	void Start () 
	{
        database = new UTDDatabase();
        StateStack.Push(new LogInGameState(this.GetComponent<NetworkManager>()));
        ((LogInGameState)StateStack.Peek()).backgroundTexture = backgroundTexture;
		
		//StateStack.Push (new LobbyGameState(this));
	}

	void Update () 
	{
        StateStack.Peek().Update();
	}
	
	public void OnGUI()
	{
        StateStack.Peek().OnGUI();
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
        /* if we're in PreGameGameState */
        if (StateStack.Peek() is PreGameGameState)
        {
            PreGameGameState state = ((PreGameGameState)StateStack.Peek());
            state.NumberPlayersConnected++;
            networkView.RPC("NumberOfPlayers", RPCMode.Others, state.NumberPlayersConnected);
        }
	}
	
	[RPC]
	public void NumberOfPlayers(int NumberOfConnectedPlayers)
	{
        if (StateStack.Peek() is PreGameGameState)
        {
            PreGameGameState state = ((PreGameGameState)StateStack.Peek());
            state.NumberPlayersConnected = NumberOfConnectedPlayers;
        }
	}
	
	public void OnServerInitialized()
	{
        //StateStack.Push(new PreGameGameState(this.GetComponent<NetworkManager>(), InGamePregameStyle));
	}
	
	public void OnConnectedToServer()
	{
        //StateStack.Push(new PreGameGameState(this.GetComponent<NetworkManager>(), InGamePregameStyle));
	}

    [RPC]
    public void UpdateLobbyInfo(string state)
    {
        //we need to update the Lobby Screen with the info found in state
        //state will parse down to a string to describes the state of the lobby
        //following this protocol:

        //PlayerNameInLobby1, PlayerNameInLobby2, ...PARAM_SPLITPlayerNameTeam1Slot1, PlayerNameteam1Slot2, ...PARAM_SPLITRaceChoiceTeam1Slot1, RaceChoiceTeam1Slot2
		Debug.Log (state);
        string str_state = state;

        string[] Sections = str_state.Split(new string[] { "PARAM_SPLIT" }, System.StringSplitOptions.None);

        string[] PlayersInLobby = Sections[0].Split(',');
        string[] PlayerNameInTeamSlots = Sections[1].Split(',');
        string[] RaceChoices = Sections[2].Split(',');

        if (StateStack.Peek() is LobbyGameState)
        {
            /* ok this person is in Lobby, lets update the lobbies info */
			if (PlayersInLobby[0] != "UTD_NOPLAYERSINLOBBY")
			{
				((LobbyGameState)StateStack.Peek ()).playersInLobby.Clear ();
	            foreach (string str in PlayersInLobby)
	            {
	                ((LobbyGameState)StateStack.Peek()).playersInLobby.AddEntry(str);
	            }
			}
			else
			{
				/* clear out the list, that way we can reflect a change in the lobby (such
				as everyone leaving)*/
				((LobbyGameState)StateStack.Peek ()).playersInLobby.Clear ();
				
			}
			
            int i = 0;
            foreach (string str in PlayerNameInTeamSlots)
            {
                ((LobbyGameState)StateStack.Peek()).players[i].name = str;
				if (str != "< Empty >")
				{
					//someones in this slot, set their info
					((LobbyGameState)StateStack.Peek()).players[i].PlayerJoined = true;
				}
				else
					((LobbyGameState)StateStack.Peek()).players[i].PlayerJoined = false;
				
                i++;
            }

            i = 0;
            foreach (string str in RaceChoices)
            {
                int index = System.Convert.ToInt32(str);
				Debug.Log("Setting Race Index: " + index);
                ((LobbyGameState)StateStack.Peek()).SetRaceFromIndex(i, index);
                i++;
            }
        }

    }
}