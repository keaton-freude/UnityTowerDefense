using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/* The scoreboard can be thought of as a global
 * networked singleton (in a broad sense).
 * It will contain all meta information about a game including
 * score, players, gold, income, etc.
 * 
 * This isn't necessarily the absolutely correct state of things
 * at any given point, but rather the information one might glean from
 * a scoreboard */

public class Scoreboard : MonoBehaviour 
{
	public int Team1Lives = 0;
	public int Team2Lives = 0;
	
	public List<PlayerInfo> players = new List<PlayerInfo>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddNewPlayer(string name, int id, int gold, int income)
	{
		players.Add (new PlayerInfo(name, id, gold, income));
	}
}

public class PlayerInfo
{
	public string Name;
	public int Id;
	public int Gold;
	public int Income;
	
	public PlayerInfo()
	{
	}
	
	public PlayerInfo(string n, int i, int g, int inc)
	{
		Name = n;
		Id = i;
		Gold = g;
		Income = inc;
	}
}


