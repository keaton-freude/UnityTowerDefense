using System;
using UnityEngine;

public class MainMenuGameState: GameState
{
    public MainMenuGameState(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
    }

    public override void OnGUI()
    {
        if (GUI.Button(new Rect(50, 90, 100, 45), "Create Server"))
        {
            Debug.Log("Registering Server");
            Network.InitializeServer(1, 25000, !Network.HavePublicAddress());
            MasterServer.RegisterHost("UnityTowerDefense_freudek", "Keaton's Game", "The default game");
            //AtMainMenu = false;
            UnityEngine.Object.DontDestroyOnLoad(GameObject.Find("__NetworkManager"));
            Application.LoadLevel("MainScene");
        }

        if (GUI.Button(new Rect(165, 90, 130, 45), "Refresh Server List"))
        {
            MasterServer.RequestHostList("UnityTowerDefense_freudek");
        }

        GUI.Label(new Rect(50, 140, 300, 25), "CURRENTLY CREATED SERVERS");

        Rect horiztonalRuleRect = new Rect(50, 150, Screen.width, 25);
        string horiztonalRuleString = "______________________________";

        GUI.Label(horiztonalRuleRect, horiztonalRuleString);

        HostData[] data = MasterServer.PollHostList();
        // Go through all the hosts in the host list
        foreach (HostData element in data)
        {
            GUILayout.BeginArea(new Rect(50, 175, 500, Screen.height));

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
                Debug.Log(e);
                UnityEngine.Object.DontDestroyOnLoad(GameObject.Find("__NetworkManager"));
                Application.LoadLevel("MainScene");

            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

        }
    }

    public override void Update()
    {
        
    }
}

