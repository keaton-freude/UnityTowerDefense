using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour 
{
	public List<GameObject> monsters;
	public GameObject Waypoints;
	
	
	
	// Use this for initialization
	void Start () 
	{
		monsters = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void CreateMonster(string prefabPath)
	{
		GameObject go = Instantiate(Resources.Load (prefabPath), new Vector3(250, 90, 250), Quaternion.identity) as GameObject;
		
		//List<GameObject> test = Waypoints.GetComponent<Waypoints>().Path1.Cop
		
		go.GetComponent<WaypointMover>().Waypoints = new List<GameObject>(Waypoints.GetComponent<Waypoints>().Path1);
		
		monsters.Add(go);
		
		

		/* Get them moving! */
		go.GetComponent<WaypointMover>().DoMove = true;
	}
}
