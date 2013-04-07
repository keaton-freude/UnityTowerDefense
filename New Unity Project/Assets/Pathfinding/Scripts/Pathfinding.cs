using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PathfinderType
{
    GridBased,
    WaypointBased
}

public class Pathfinding : MonoBehaviour 
{
    public List<Vector3> Path = new List<Vector3>();
    public PathfinderType PathType = PathfinderType.GridBased;
	public bool JS = false;

    public void FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        if (PathType == PathfinderType.GridBased)
        {
            Pathfinder.Instance.InsertInQueue(startPosition, endPosition, SetList);
        }
        else if (PathType == PathfinderType.WaypointBased)
        {
            WaypointPathfinder.Instance.InsertInQueue(startPosition, endPosition, SetList);
        }
    }
	
	public void FindJSPath(Vector3[] arr)
    {
        if(arr.Length > 1)
		{	
			if (PathType == PathfinderType.GridBased)
	        {
	            Pathfinder.Instance.InsertInQueue(arr[0], arr[1], SetList);
	        }
	        else if (PathType == PathfinderType.WaypointBased)
	        {
	            WaypointPathfinder.Instance.InsertInQueue(arr[0], arr[1], SetList);
	        }
		}
    }

    //A test move function, can easily be replaced
    public void Move()
    {
        if (Path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, Path[0], Time.deltaTime * 30F);
            if (Vector3.Distance(transform.position, Path[0]) < 0.4F)
            {
                Path.RemoveAt(0);
            }
        }
    }

    protected void SetList(List<Vector3> path)
    {
        if (path == null)
        {
            return;
        }
		
		if(!JS)
		{
	        Path.Clear();
	        Path = path;
		}
		else
		{
			Vector3[] arr = new Vector3[path.Count];
			for(int i = 0; i < path.Count; i++)
			{
				arr[i] = path[i];
			}			
			gameObject.SendMessage("GetJSPath", arr);
		}
    }
}
	
