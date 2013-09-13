using System;
using UnityEngine;

public class PlayGameGameState : GameState
{
    public int TopLives = 30;
    public int BotLives = 30;
    public PlayGameGameState(NetworkManager manager)
    {
        this.networkManager = manager;
    }

    public override void OnGUI()
    {
        GUI.Label(new Rect(Screen.width * .4f, 0, 150, 45), string.Format("Top Lives: {0}", TopLives));
        GUI.Label(new Rect(Screen.width * .6f, 0, 150, 45), string.Format("Bot Lives: {0}", BotLives));
    }

    public override void Update()
    {
        
    }
	
	public override void Cleanup ()
	{
		
	}
}