using System;
using UnityEngine;

public class CountdownGameState : GameState
{
    GUISkin InGamePregameStyle;
    public float PreGameCountdown = 15f;
    public CountdownGameState(NetworkManager manager, GUISkin skin)
    {
        this.networkManager = manager;
        InGamePregameStyle = skin;
    }

    public override void OnGUI()
    {
        GUI.skin = InGamePregameStyle;

        GUI.Label(new Rect(Screen.width * .25f, Screen.height * .3f, 800, 150), string.Format("Game Starts In: {0} seconds", Mathf.RoundToInt(PreGameCountdown)), InGamePregameStyle.customStyles[0]);

        GUI.skin = null;
    }

    public override void Update()
    {
        PreGameCountdown -= Time.deltaTime;
        if (PreGameCountdown <= 0f)
            networkManager.StateStack.Push(new PlayGameGameState(networkManager));
    }
}