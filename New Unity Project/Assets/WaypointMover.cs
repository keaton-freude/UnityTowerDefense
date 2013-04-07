using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointMover : Pathfinding
{
	public List<GameObject> Waypoints = new List<GameObject>();
	
	public GameObject currentWaypoint;
	
	public bool DEBUG_STARTED = false;
	
	
	// Use this for initialization
	void Start () 
	{
		foreach (GameObject go in Waypoints)
		{
			go.transform.localPosition = new Vector3(go.transform.localPosition.x, 0, go.transform.localPosition.z);
		}
		currentWaypoint = Waypoints[0];
		Waypoints.RemoveAt (0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.P))
		{
			/* start it off */
			Pathfinder.Instance.CreateMap();
			FindPath (transform.localPosition, currentWaypoint.transform.localPosition);

			Debug.Log ("I'm starting now");
		}
		
		if (Path.Count > 0)
		{
			Move ();
			DEBUG_STARTED = true;
		}
		else if (DEBUG_STARTED)
		{
			/* we either get a new waypoint, or we're done */
			
			if (Waypoints.Count != 0)
			{
				Debug.Log ("popping new waypoint");
				currentWaypoint = Waypoints[0];
				Waypoints.RemoveAt (0);
				
				FindPath (transform.localPosition, currentWaypoint.transform.localPosition);
			}
			else
			{
				/* Done, remove this game object? lol */
				Debug.Log ("Killing self");
				Object.Destroy (this.gameObject);
			}
		}
	}
	
	public void Move()
	{
		if (Path.Count > 0)
		{
			transform.position = Vector3.MoveTowards (transform.position, Path[0], Time.deltaTime * 30F);
			
			transform.LookAt(Path[0]);
			
			animation.Play ("run");
			
			if (Vector3.Distance (transform.position, Path[0]) < 0.1f)
				Path.RemoveAt (0);
		}
		
	}
}
