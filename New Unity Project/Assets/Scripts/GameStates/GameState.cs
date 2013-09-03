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

    public NetworkManager networkManager;
}

