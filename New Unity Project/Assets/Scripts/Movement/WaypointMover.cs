using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointMover : Pathfinding
{
	public List<GameObject> Waypoints = new List<GameObject>();
	
	public GameObject currentWaypoint;
	
	public Camera camera;
	
	public bool DEBUG_STARTED = false;
	public bool DoMove = false;
	
	public Vector3 LastPosition;
	
	// Use this for initialization
	void Start () 
	{
		foreach (GameObject go in Waypoints)
		{
			go.transform.localPosition = new Vector3(go.transform.localPosition.x, 90, go.transform.localPosition.z);
		}
		if (Waypoints.Count != 0)
		{
			currentWaypoint = Waypoints[0];
			Waypoints.RemoveAt (0);
		}
	}
	
	public void StartMoving()
	{
		Pathfinder.Instance.CreateMap();
		FindPath (transform.localPosition, currentWaypoint.transform.localPosition);
		DoMove = false;
		Debug.Log ("I'm starting now");
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (networkView.isMine)
		{
			if (DoMove)
				StartMoving();
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
					Network.Destroy (this.gameObject);
					
				}
			}
		}
		else
		{
			
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (!stream.isWriting)
		{
			
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
