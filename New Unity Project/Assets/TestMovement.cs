using UnityEngine;
using System.Collections;

public class TestMovement : Pathfinding {
	public Vector3 endPosition = new Vector3(94, 0, 444);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P))
		{
			FindPath (transform.position, endPosition);
		}
		
		if (Path.Count > 0)
		{
			Move ();
		}
	}
	
	void Move()
	{
		if (Path.Count > 0)
		{
			transform.position = Vector3.MoveTowards (transform.position, Path[0], Time.deltaTime * 30f);
			if (Vector3.Distance (transform.position, Path[0]) < 0.1f)
			{
				Path.RemoveAt (0);
			}
		}
	}
}
