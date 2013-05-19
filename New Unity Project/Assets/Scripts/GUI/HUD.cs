using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System;
using System.Collections.Generic;

public class HUD : MonoBehaviour 
{
	private Rect GoldRect = new Rect(25, 25, 100, 25);
	private Rect IncomeRect = new Rect(25, 40, 100, 25);
	private Rect SelectedTowerRect = new Rect(400, 25, 200, 25);
	public Texture HUD_OVERLAY_TEXTURE;
 	public List<MonsterData> MonsterData;
    public List<TowerData> TowerData;
	
	public GUISkin gameGuiStyle;
	
	public GameObject MonsterManager;

    public bool MonsterBuildMenuSelected = true;
	/* Monster data left-hand-corner offsets */
	public Vector2 GridLocation = new Vector2(Screen.width * 0.7536458f, Screen.height * 0.768518f);
	
	public Vector2 IconDimensions = new Vector2(Screen.width * 0.0421875f, Screen.height * 0.06389f);
	
	/* @IconsPerRow.X = width, @IconsPerRow.Y = height */
	public Vector2 IconsPerRow = new Vector2(4, 3);
	
	public float GapBetweenIcons_Width = 0f;
	public float GapBetweenIcons_Height = 0f;
	
    //public float IconWidth = 64f;
    //public float IconHeight = 64f;
	
	public int ICONS_PER_ROW = 5;
	public int ICONS_PER_COLUMN = 3;
	
	/* Home / Sell / Build Buttons */
	public Rect HomeRect;
	public Rect SellRect;
	public Rect BuildRect;
	
	public string HomeString = "HOME";
	public string SellString = "MOBS";
	public string BuildString = "BUILD";
	
	/* Minimap Buttons */
	public Rect MinimapRect1;
	public Rect MinimapRect2;
	public Rect MinimapRect3;
	
	public string MinimapString1 = "M1";
	public string MinimapString2 = "M2";
	public string MinimapString3 = "M3";
	
	/* Selected Tower Rects */
	public Rect SelectedTowerLevelRect;
	public Rect SelectedTowerNameRect;
	public Rect SelectedTowerKillCounterRect;
	/* Seleted Tower Strings */
    public TowerInfo SelectedTower = null;

    public List<GameObject> Waypoints;

    public float TooltipWidth;
    public float TooltipHeight;
    public float TooltipY;


	
    // Use this for initialization
    void Start () 
    {
    	GridLocation = new Vector2(Screen.width * 0.7536458f, Screen.height * 0.768518f);
	
		IconDimensions = new Vector2(Screen.width * 0.0432291667f, Screen.height * 0.06389f);


        TooltipWidth = 0.23072916f * Screen.width;
        TooltipHeight = 0.1694444f * Screen.height;
        TooltipY = 0.5777777f * Screen.height;
		GapBetweenIcons_Width = Screen.width * 0.0041666f;
		
		GapBetweenIcons_Height = Screen.height * 0.0041667f;
		
		HomeRect = new Rect(Screen.width * 0.5666f, Screen.height * 0.91851f, Screen.width * 0.0328125f, Screen.height * 0.0583333f);
		SellRect = new Rect(Screen.width * 0.65520833f, Screen.height * 0.91851f, Screen.width * 0.0328125f, Screen.height * 0.0583333f);
		BuildRect = new Rect(Screen.width * 0.697395833f, Screen.height * 0.91851f, Screen.width * 0.03333333f, Screen.height * 0.0583333f);
		
		MinimapRect1 = new Rect(Screen.width * 0.15416666666667f, Screen.height * 0.72777f, Screen.width * 0.0166666666667f, Screen.height * 0.0287037037037037037037037037037f);
		MinimapRect2 = new Rect(Screen.width * 0.15416666666667f, Screen.height * 0.7620370f, Screen.width * 0.0166666666667f, Screen.height * 0.0287037037037037037037037037037f);
		MinimapRect3 = new Rect(Screen.width * 0.15416666666667f, Screen.height * 0.7962962f, Screen.width * 0.0166666666667f, Screen.height * 0.0287037037037037037037037037037f);
		
		SelectedTowerLevelRect = new Rect(0.3234375f * Screen.width, Screen.height * 0.796296f, Screen.width * 0.0114583f, Screen.height * 0.019444f);
		SelectedTowerNameRect = new Rect(Screen.width * 0.3390625f, Screen.height * 0.796296f, Screen.width * 0.1546875f, Screen.height * 0.019444f);
		SelectedTowerKillCounterRect = new Rect(Screen.width * 0.50625f, Screen.height * 0.796296f, Screen.width * 0.04270833f, Screen.height * 0.019444f);
		
		//SelectedTowerLevel = 99;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), HUD_OVERLAY_TEXTURE, ScaleMode.StretchToFill, true);
		
		/* Iterate through our monster data and draw the choices to the screen */
		int count = 0;
		int ycount = 0;
		GUI.skin = gameGuiStyle;
		
		#region MonsterGrid
        if (MonsterBuildMenuSelected)
        {
            foreach (MonsterData md in MonsterData)
            {
                gameGuiStyle.button.normal.background = md.iconTexture;
                gameGuiStyle.button.hover.background = md.iconTexture;

                if (GUI.Button(new Rect(GridLocation.x + (count * (IconDimensions.x + GapBetweenIcons_Width)),
                GridLocation.y + (ycount * (IconDimensions.y + GapBetweenIcons_Height)),
                    IconDimensions.x, IconDimensions.y), new GUIContent("", md.description), "button"))
                {
                    MonsterManager.GetComponent<MonsterManager>().CreateMonster(md.prefab.name);
                }
                GUI.Label(new Rect(GridLocation.x,TooltipY, TooltipWidth, TooltipHeight), GUI.tooltip);
                count++;
                if (count == ICONS_PER_ROW - 1)
                {
                    count = 0;
                    ycount++;
                }

            }
        }
        else
        {
            /* Draw build menu for given player */
            foreach (TowerData td in TowerData)
            {
                gameGuiStyle.button.normal.background = td.iconTexture;
                gameGuiStyle.button.hover.background = td.iconTexture;

                if (GUI.Button(new Rect(GridLocation.x + (count * (IconDimensions.x + GapBetweenIcons_Width)),
                    GridLocation.y + (ycount * (IconDimensions.y + GapBetweenIcons_Height)),
                    IconDimensions.x, IconDimensions.y), new GUIContent("", td.description), "button"))
                {
                    /* Set this tower as the tower to be built */
                }
                GUI.Label(new Rect(GridLocation.x, GridLocation.y - 100, 300, 80), GUI.tooltip);
                count++;
                if (count == ICONS_PER_ROW - 1)
                {
                    count = 0;
                    ycount++;
                }
            }
        }
		#endregion
		GUI.skin = null;
		#region HomeBuildSellButtons
		if (GUI.Button (HomeRect, HomeString) || Input.GetKeyDown(KeyCode.H))
		{
			Camera.main.GetComponent<RTSCameraMove>().MoveToHome();
		}
		
		if (GUI.Button (SellRect, SellString))
		{
            MonsterBuildMenuSelected = true;
		}
		
		if (GUI.Button (BuildRect, BuildString))
		{
            MonsterBuildMenuSelected = false;
		}
		#endregion
		
		#region MinimapButtons
		if (GUI.Button (MinimapRect1, MinimapString1))
		{
			
		}
		
		if (GUI.Button (MinimapRect2, MinimapString2))
		{
		}
		
		if (GUI.Button (MinimapRect3, MinimapString3))
		{
		}
		#endregion
		GUI.skin = gameGuiStyle;
		#region SelectedTower
        if (SelectedTower != null)
        {
            GUI.Label(SelectedTowerLevelRect, SelectedTower.TowerLevel.ToString());
            GUI.Label(SelectedTowerNameRect, SelectedTower.TowerName);
            GUI.Label(SelectedTowerKillCounterRect, SelectedTower.KillCounter.ToString());
        }
        else
        {
            /* Display Default Values */
            GUI.Label(SelectedTowerLevelRect, "");
            GUI.Label(SelectedTowerNameRect, "");
            GUI.Label(SelectedTowerKillCounterRect, "");
        }
		#endregion
		
		
		if (GUI.Button (new Rect(500, 25, 100, 25), "Create Server"))
		{
			Network.incomingPassword = "HolyMoly";
			Network.InitializeServer(32, 25000, true);
		}
		if (GUI.Button (new Rect(500, 50, 100, 25), "Join Server"))
		{
			Network.Connect ("71.237.249.213", 25000, "HolyMoly");
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