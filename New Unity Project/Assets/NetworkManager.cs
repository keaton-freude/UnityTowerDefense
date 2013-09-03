using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour 
{
    public UTDDatabase database;

    public Stack<GameState> StateStack = new Stack<GameState>();

    public GUITexture backgroundTexture;
	public GUISkin InGamePregameStyle;

	void Start () 
	{
        database = new UTDDatabase();
        StateStack.Push(new LogInGameState(this.GetComponent<NetworkManager>()));
        ((LogInGameState)StateStack.Peek()).backgroundTexture = backgroundTexture;
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
        StateStack.Push(new PreGameGameState(this.GetComponent<NetworkManager>(), InGamePregameStyle));
	}
	
	public void OnConnectedToServer()
	{
        StateStack.Push(new PreGameGameState(this.GetComponent<NetworkManager>(), InGamePregameStyle));
	}
}