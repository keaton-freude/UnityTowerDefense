using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System;

public class HUD : MonoBehaviour 
{
	private Rect GoldRect = new Rect(25, 25, 100, 25);
	private Rect IncomeRect = new Rect(25, 40, 100, 25);
	
	public GameObject MonsterManager;
	
    // Use this for initialization
    void Start () 
    {
    
    }
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnGUI()
	{
		GUI.Label(GoldRect, "Gold: 1000");
		GUI.Label(IncomeRect, "Income: 124");
		
		if (GUI.Button (new Rect(100, 25, 100, 25), "Spawn" ))
		{
			StartCoroutine("MyMethod");
		}
	}
	public int count = 0;
	
	IEnumerator MyMethod()
	{
		while (true)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			MonsterManager.GetComponent<MonsterManager>().CreateMonster("SkeletonPrefab");
			sw.Stop();
			
			
			UnityEngine.Debug.Log("Count: " + count + " - " +sw.ElapsedMilliseconds);
			count++;
			
			yield return new WaitForSeconds(2.25f);
		}
	}
}