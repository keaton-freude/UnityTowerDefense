using UnityEngine;
using System.Collections;

public class MoveAlong : Pathfinding 
{
	Vector3 endPosition = new Vector3(94, 0, 444);
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.P))
		{
			Pathfinder.Instance.CreateMap();
			FindPath(transform.position, endPosition);
		}
		
		if (Path.Count > 0)
			Move();
	}
	
	new public void Move()
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
