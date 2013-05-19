using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster: Pathfinding
{
	public Map map;
	
	private static GameMaster instance;

    public Transform floatingTextPrefab;

    public void SpawnFloatingDamage(int damage, float x, float y)
    {


        var gui = Instantiate(Resources.Load("FloatingTextPrefab"), new Vector3(x, y, 0), Quaternion.identity) as GameObject;

        gui.guiText.text = damage.ToString();
    }

    public bool IsPathValid(List<GameObject> points)
    {
        /* Check the path between each point and return false if the PathFinder returns the empty list at any point */
        for (int i = 0; i < points.Count - 1; ++i)
        {
            FindPath(points[i].transform.position, points[i + 1].transform.position);

            if (Path.Count == 0)
            {
                return false;
            }
        }

        return true;
    }
	
	public static GameMaster Instance
	{
		get
		{
            if (instance == null)
                instance = new GameObject("GameMaster").AddComponent<GameMaster>();
			return instance;
		}
	}
	
	private GameMaster()
	{
		map = new Map();
	}
	
	// Use this for initialization
	void Start () 
	{
		if (GameObject.FindGameObjectWithTag("LoadParameters").GetComponent<LoadParameters>().CreateServer)
		{
			/* Create Server */
			Network.incomingPassword = "HolyMoly";
            Network.InitializeServer(32, 25000, true);
		}
		else
		{
			/* Join Server */
            NetworkConnectionError error = Network.Connect(GameObject.FindGameObjectWithTag("LoadParameters").GetComponent<LoadParameters>().IPAddress, 25000, "HolyMoly");

            Debug.Log("Joining Server: " + GameObject.FindGameObjectWithTag("LoadParameters").GetComponent<LoadParameters>().IPAddress);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log(error);
    }
	
	public GameObject GetNearestEnemy(Vector3 source, float distance)
	{
		/* Returns the GameObject that is closest to @source and within
		 * @distance game units */
		
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
		int nearest = -1;
		float temp_distance = 0.0f;
		float nearest_distance = -1f;
		
		for(int i = 0; i < gos.Length; ++i)
		{
			temp_distance = Vector3.Distance (gos[i].transform.position, source);
			
			if (temp_distance < distance)
			{
				if (nearest_distance == -1f || temp_distance < nearest_distance)
				{
					nearest_distance = temp_distance;
					nearest = i;
				}
			}
		}
		
		if (nearest == -1)
			return null;
		else
			return gos[nearest];
	    
    }
}

public class Map
{
	public Cell[,] map;
	
	public Map()
	{
		map = new Cell[50, 50];
		
		for(int i = 0; i < 50; ++i)
		{
			for(int j = 0; j < 50; ++j)
			{
				map[i, j] = new Cell();
			}
		}
	}
	
	public void AddTower(int x, int y)
	{
		this.map[x, y].TowerBuilt = true;
	}
	
	public void RemoveTower(int x, int y)
	{
		this.map[x, y].TowerBuilt = false;
	}
	
	public bool IsTowerBuilt(int x, int y)
	{
		return this.map[x, y].TowerBuilt;
	}
}

public class Cell
{
	public bool TowerBuilt;
	
	public GameObject Tower;
	
	public Cell()
	{
		TowerBuilt = false;
		Tower = null;
	}
}