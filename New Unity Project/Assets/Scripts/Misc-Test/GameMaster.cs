using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour 
{
	public Map map;
	
	private static GameMaster instance;
	
	public static GameMaster Instance
	{
		get
		{
			if (instance == null)
				instance = new GameMaster();
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
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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
	
	public Cell()
	{
		TowerBuilt = false;
	}
}