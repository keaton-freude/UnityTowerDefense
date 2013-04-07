using UnityEngine;
using System.Collections;

public class DebugTest:MonoBehaviour {

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
		GUI.Button (new Rect(25, 25, 100, 30), "Spawn");
	}
}
