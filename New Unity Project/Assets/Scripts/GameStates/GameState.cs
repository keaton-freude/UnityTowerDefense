using System;
using UnityEngine;

public abstract class GameState
{
    public GameState()
    {

    }

    public UTDDatabase database;

    public abstract void OnGUI();
    public abstract void Update();
	
	/* require derived classes to clean themselves up to a "beginning" state */
	/* call this when you want to return to a screen */
	public abstract void Cleanup();

    public NetworkManager networkManager;
}

