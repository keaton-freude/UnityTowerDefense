using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster: Pathfinding
{
	/* 0: We're in the process of creating the server
	 * 	  and waiting for players to connect. This state is valid
	 *    for only 10 seconds. Otherwise failure.
	 * 1: Player's are voting on mode, generic countdown to game
	 *    start.
	 * 2: Game is actively being played.
	 * 3: Game score is being shown.
	 */

	//public GAMESTATE gameState = GAMESTATE.UTD_GAMESTATE_NETWORKINIT;
	public Map map;
	public int NumberOfExpectedPlayers = 2;
	/* Server is always counted as 1 */
	public int NumberOfPlayersInGame = 1;
	private static GameMaster instance;
	public string HostIP = "127.0.0.1";
    public Transform floatingTextPrefab;
	
	public bool IsServer = true;
	
	public int Team1Lives = 30;
	public int Team2Lives = 30;

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
	
	public GameMaster()
	{
		map = new Map();
	}
	
	// Use this for initialization
	void Start () 
	{
		//GameObject.Find ("__NetworkManager").GetComponent<NetworkManager>().gameState =  NetworkManager.GAMESTATE.UTD_GAMESTATE_PREGAME;
	}
	
	public bool InitConnection = false;
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnConnectedToServer()
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