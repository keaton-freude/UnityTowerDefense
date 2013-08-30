using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour 
{
	public bool AtMainMenu = true;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (AtMainMenu)
		{
			if (Input.GetKeyDown (KeyCode.K))
				MasterServer.RequestHostList("UnityTowerDefense_freudek");
		}
	}
	
	public void OnGUI()
	{
		if (AtMainMenu)
		{
			if (GUI.Button (new Rect(50, 90, 100, 45), "Create Server"))
			{
				Debug.Log ("Registering Server");
				Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
				MasterServer.RegisterHost("UnityTowerDefense_freudek", "Keaton's Game", "The default game");
				//Application.LoadLevel ("MainScene");
			}
			
			if (GUI.Button (new Rect(165, 90, 130, 45), "Refresh Server List"))
			{
				MasterServer.RequestHostList("UnityTowerDefense_freudek");
			}
			
			GUI.Label (new Rect(50, 140, 300, 25), "CURRENTLY CREATED SERVERS");
			
			Rect horiztonalRuleRect = new Rect(50, 150, Screen.width, 25);
			string horiztonalRuleString = "______________________________";
			
			GUI.Label(horiztonalRuleRect, horiztonalRuleString);
		
			HostData[] data = MasterServer.PollHostList();
			// Go through all the hosts in the host list
			foreach (HostData element in data)
			{
				GUILayout.BeginArea (new Rect(50, 175, 500, Screen.height));
				
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
					Debug.Log (e);
					Application.LoadLevel ("MainScene");
					
				}
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
				
			}
		}
	}
	
	[RPC]
	public void DoDamageToMob(NetworkViewID id, int amount)
	{
		if (!AtMainMenu)
			NetworkView.Find (id).gameObject.GetComponent<MobStats>().TakeDamage(amount);
	}
	
	public void OnFailedToConnect(NetworkConnectionError e)
	{
		Debug.Log (e);
	}
}
