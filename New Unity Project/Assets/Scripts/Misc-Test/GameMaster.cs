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
	public enum GAMESTATE
	{
		UTD_GAMESTATE_NETWORKINIT = 0,
		UTD_GAMESTATE_PREGAME = 1,
		UTD_GAMESTATE_PLAYGAME = 2,
		UTD_GAMESTATE_POSTGAME = 3
	}
	public GAMESTATE gameState = GAMESTATE.UTD_GAMESTATE_NETWORKINIT;
	public Map map;
	public int NumberOfExpectedPlayers = 2;
	/* Server is always counted as 1 */
	public int NumberOfPlayersInGame = 1;
	private static GameMaster instance;
	public string HostIP = "127.0.0.1";
    public Transform floatingTextPrefab;
	
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
		gameState = GAMESTATE.UTD_GAMESTATE_NETWORKINIT;
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
	
	public bool CRASH = false;
	
	// Update is called once per frame
	void Update () 
	{
		if (!CRASH)
		{
			/* The gamemaster will continually poll and watch the game to determine whats going on */
			string DebugInfo = "";
			/* DEBUG */
			//First, lets keep track of our current state...
			DebugInfo += "GameState: " + gameState.ToString();
			
			/* Next, switch on the current state, and we'll see whats up */
			switch (gameState)
			{
				case GAMESTATE.UTD_GAMESTATE_NETWORKINIT:
					if (false)
					{
						/* We're the server, also we're guaranteed to be connected at this point */
						/* Lets output the # of connected players */
						DebugInfo += " | # CONNECTIONS: " + Network.connections.Length.ToString();
					
						//Next, check that number against the expected #
						if (Network.connections.Length == NumberOfExpectedPlayers)
						{
							//Everyone is in! Begin pre-game
							gameState = GAMESTATE.UTD_GAMESTATE_PREGAME;
						
							//Note, we likely have a lot to do here, but for now this is ok
						}
					}
					else
					{
						//Now this part is substantially more interesting
						//I should know if I'm the host for this game or not, this else
						//branch is entered when i *know* i'm NOT the host. Therefore, i should try and
					    //connect to the provided IP
					
						NetworkConnectionError nce = Network.Connect(HostIP, 25000, "HolyMoly");
						
						if (nce == NetworkConnectionError.ConnectionFailed)
						{
							/* Something went wrong, put message on screen, do not try again */
							DebugInfo += " CONNECTION FAILED, ABORTING. RESTART TO TRY AGAIN";
							CRASH = true;
						}
						else if (nce == NetworkConnectionError.NoError)
						{
							Debug.Log ("Connection success");
							gameState = GAMESTATE.UTD_GAMESTATE_PREGAME;
						}
						else
						{
							Debug.Log ("Network Connection Error: " + nce.ToString());
						}
						
					}
					break;
				case GAMESTATE.UTD_GAMESTATE_PREGAME:
					break;
				case GAMESTATE.UTD_GAMESTATE_PLAYGAME:
					break;
				case GAMESTATE.UTD_GAMESTATE_POSTGAME:
					break;
				default:
					break;
			}
			GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().DebugString = DebugInfo;
		}
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