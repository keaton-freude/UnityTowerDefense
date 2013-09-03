using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridDraw : MonoBehaviour {
	
	Vector3 StartPos = new Vector3(0, 90.2f, 0);
	Vector3 EndPos = new Vector3(0, 90.2f, 0);
	
	public Transform CurrentlySelectedAura = null;
	
	public GameObject CurrentlySelectedTower = null;
	
	public List<Vector2> InvalidTiles;
	
	public GameObject LastTower = null;
	public Vector2 LastTowerLocation = Vector2.zero;
	
	public List<GameObject> Waypoints;
	
	public GameObject towerobj;
	public GameObject cubeobj;
	
	public int GridSize = 10;
	
	public int X_OFFSET = 20;
	
	public int Y_OFFSET = 23;
	
	public new Camera camera;

	// Use this for initialization
	void Start () 
	{
		EndPos.z = 500 - Y_OFFSET;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
		{
			Ray ray = camera.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit = new RaycastHit();
			
			if (Physics.Raycast (ray, out hit))
			{
				GameMaster Instance = GameObject.Find("__GameMaster").GetComponent<GameMaster>();
				
				Vector3 transformedLocation = TransformGridToArray(hit.point);
				/* Check to see if a tower already exists */
				if (!Instance.map.IsTowerBuilt((int)transformedLocation.x, (int)transformedLocation.z) && !InvalidTiles.Contains(new Vector2(transformedLocation.x, transformedLocation.z)))
				{
					/* No tower exists, set the value in the map */
					Instance.map.AddTower((int)transformedLocation.x, (int)transformedLocation.z);
					
					Debug.Log (transformedLocation);
					
					/* Then instantiate the tower at the location */
					Vector3 finalLocation = TransformGridToWorldSpace(transformedLocation) + new Vector3(5.0f, 0, 5.5f);
					
					finalLocation += new Vector3(0f, 0, 0);
					string prefabString = "";
					
					//if (Random.value >.5f)
					//	prefabString = "OtherTowerPrefab";
					//else
					prefabString = "MagicTowerPrefab";
				
					GameObject go = Network.Instantiate(Resources.Load (prefabString), finalLocation, Quaternion.identity, 0) as GameObject;

					Instance.map.map[(int)transformedLocation.x, (int)transformedLocation.z].Tower = go;
					LastTower = go;
					LastTowerLocation = new Vector2(transformedLocation.x, transformedLocation.z);
					
					/* Now Check the Route */
					for(int i = 0; i < Waypoints.Count - 1; ++i)
					{
						Pathfinder.Instance.InsertInQueue(Waypoints[i].transform.position, Waypoints[i+1].transform.position, CheckRoute);
					}
				
				
				}
			}
		}
		else if (Input.GetMouseButtonDown (0))
		{
			Ray ray = camera.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
				GameMaster Instance = GameObject.Find("__GameMaster").GetComponent<GameMaster>();
                Vector3 transformedLocation = TransformGridToArray(hit.point);

                if (Instance.map.IsTowerBuilt((int)transformedLocation.x, (int)transformedLocation.z))
                {
                    TowerInfo towerInfo = Instance.map.map[(int)transformedLocation.x, (int)transformedLocation.z].Tower.GetComponent<TowerInfo>();

                    /* if a tower is built, lets grab the name and send it to the HUD */
                    GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().SelectedTower = towerInfo;

                    Transform[] childrenInThisObject = Instance.map.map[(int)transformedLocation.x, (int)transformedLocation.z].Tower.GetComponentsInChildren<Transform>();

                    if (CurrentlySelectedTower != null)
                    {
                        /* turn it off */
                        TurnOffOutline(CurrentlySelectedTower);
                    }

                    CurrentlySelectedTower = Instance.map.map[(int)transformedLocation.x, (int)transformedLocation.z].Tower;
                    TurnOnOutline(CurrentlySelectedTower);



                }
                else
                {
                    /* User selected the ground, clear the SelectedTower */
                    GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().SelectedTower = null;

                    if (CurrentlySelectedTower != null)
                    {
                        /* turn it off */
                        TurnOffOutline(CurrentlySelectedTower);
                    }
                }
            }
		}
		else if (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftShift))
		{
			Ray ray = camera.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit = new RaycastHit();
			
			if (Physics.Raycast (ray, out hit))
			{
				GameMaster Instance = GameObject.Find("__GameMaster").GetComponent<GameMaster>();
				Vector3 transformedLocation = TransformGridToArray(hit.point);
				/* Check to see if a tower already exists */
				if (Instance.map.IsTowerBuilt((int)transformedLocation.x, (int)transformedLocation.z))
				{
					
					Instance.map.RemoveTower((int)transformedLocation.x, (int)transformedLocation.z);
										/* Then instantiate the tower at the location */

					Network.Destroy (Instance.map.map[(int)transformedLocation.x, (int)transformedLocation.z].Tower.GetComponent<NetworkView>().viewID);
					
					
				}
			}
		}
	}
	
	public void TurnOnOutline(GameObject go)
	{
		Transform[] childrenInObject = go.GetComponentsInChildren<Transform>();
		
		foreach (Transform child in childrenInObject)
		{
			if (child.tag == "Selector")
			{
				child.GetComponent<MeshRenderer>().enabled = true;
			}

            if (child.tag == "Aura")
            {
                /* found it. Enable it */
                child.GetComponent<MeshRenderer>().enabled = true;
            }
		}
		
	}
	
	public void TurnOffOutline(GameObject go)
	{
		Transform[] childrenInObject = go.GetComponentsInChildren<Transform>();
		
		foreach (Transform child in childrenInObject)
		{
			if (child.tag == "Selector")
			{
				child.GetComponent<MeshRenderer>().enabled = false;
			}

            if (child.tag == "Aura")
            {
                /* found it. Disable it */
                child.GetComponent<MeshRenderer>().enabled = false;
            }
		}
	}
	
	private void CheckRoute(List<Vector3> list)
    {     
        //If we get a list that is empty there is no path, and we blocked the road
        //Then remove the last added tower!
        if (list.Count < 1 || list == null)
        {
			if (LastTower != null)
			{
				GameMaster Instance = GameObject.Find("__GameMaster").GetComponent<GameMaster>();
				Instance.map.RemoveTower((int)LastTowerLocation.x, (int)LastTowerLocation.y);
				Network.Destroy (LastTower);
				LastTower = null;
			}
        }
    }
	
	void OnDrawGizmos()
	{
		StartPos.x = X_OFFSET;
		EndPos.x = 500 - X_OFFSET;
		
		for(int i = Y_OFFSET; i < 500 - Y_OFFSET; i += GridSize)
		{
			StartPos.z = i;
			EndPos.z = i;
			//Gizmos.DrawLine (StartPos, EndPos);
		}
	}
	
	public Vector3 TransformGridToArray(Vector3 Coords)
	{
		Vector3 toReturn = Vector3.zero;
		
		toReturn.x = (((int)Coords.x - X_OFFSET) / (int)GridSize);
		toReturn.z = (((int)Coords.z - Y_OFFSET) / (int)GridSize);
		//toReturn.y = 90;
		
		return toReturn;
	}
	
	public Vector3 TransformCoordinatesToGrid(Vector3 MouseClickLocation)
	{
		Vector3 toReturn = Vector3.zero;
		
		toReturn.x = ((int)MouseClickLocation.x / (int)GridSize) * GridSize + X_OFFSET;
		toReturn.z = ((int)MouseClickLocation.z / (int)GridSize) * GridSize + Y_OFFSET;
		//toReturn.y = 90;
		
 		return toReturn;
	}
	
	public Vector3 TransformGridToWorldSpace(Vector3 Coords)
	{
		Vector3 toReturn = Coords * 10.0f + new Vector3(X_OFFSET, 90, Y_OFFSET);
		
		return toReturn;
	}
}
