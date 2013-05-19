using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class MainMenuHUD : MonoBehaviour 
{
	public Texture HUD_OVERLAY_TEXTURE;
	
	public GUISkin customSkin;
	Rect mainMenuLabelRect = new Rect(25, 20, Screen.width, 25);
	string mainMenuLabelString = "Unity Tower Defense - Main Menu";
	
	Rect horiztonalRuleRect = new Rect(25, 30, Screen.width, 25);
	string horiztonalRuleString = "______________________________";
	
	Rect createServerButtonRect = new Rect(25, 65, 200, 25);
	string createServerButtonString = "Create Server";
	
	Rect joinServerButtonRect = new Rect(25, 100, 200, 25);
	string joinServerButtonString = "Join Server (IP)";
	
	Rect ipStringRect = new Rect(25, 130, 30, 25);
	string ipLabelString = "IP: ";
	
	Rect ipTextFieldRect = new Rect(50, 130, 110, 25);
	string ipString = "xxx.xxx.xxx.xxx";
	
	Rect quitGameButtonRect = new Rect(25, 160, 200, 25);
	string quitGameString = "Exit Game";
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnGUI()
	{
		GUI.skin = customSkin;
		
		
		GUI.Label(mainMenuLabelRect, mainMenuLabelString, "Title");
		
		GUI.Label(horiztonalRuleRect, horiztonalRuleString, "Title");
		
		if (GUI.Button(createServerButtonRect, createServerButtonString))
		{
			GameObject.FindGameObjectWithTag ("LoadParameters").GetComponent<LoadParameters>().CreateServer = true;
			DontDestroyOnLoad(GameObject.FindGameObjectWithTag("LoadParameters"));
			Application.LoadLevel ("MainScene");
		}
		
		if (GUI.Button(joinServerButtonRect, joinServerButtonString))
		{
			Match match = Regex.Match (ipString, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
			if (match.Success)
			{
				GameObject.FindGameObjectWithTag ("LoadParameters").GetComponent<LoadParameters>().CreateServer = false;
				GameObject.FindGameObjectWithTag ("LoadParameters").GetComponent<LoadParameters>().IPAddress = ipString;
				DontDestroyOnLoad(GameObject.FindGameObjectWithTag("LoadParameters"));
				Application.LoadLevel ("MainScene");
			}
			else
			{
				//UnityEditor.EditorUtility.DisplayDialog("Error!", "Incorrect format for IP Address!", "OK");
			}
		}
		
		GUI.Label(ipStringRect, ipLabelString);
		
		if (GUI.Button(quitGameButtonRect, quitGameString))
		{
			Application.Quit();
		}
		
		ipString = GUI.TextField(ipTextFieldRect, ipString);
	}
}
