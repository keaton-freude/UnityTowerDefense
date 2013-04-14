using UnityEngine;
using System.Collections;

public class GridDraw : MonoBehaviour {
	
	Vector3 StartPos = new Vector3(0, .2f, 0);
	Vector3 EndPos = new Vector3(0, .2f, 0);
	
	public GameObject towerobj;
	public GameObject cubeobj;
	
	public int GridSize = 10;
	
	public int X_OFFSET = 20;
	
	public int Y_OFFSET = 23;
	
	public Camera camera;

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
				Debug.Log (TransformCoordinatesToGrid(hit.point));
				
				Vector3 transformedLocation = TransformGridToArray(hit.point);
				/* Check to see if a tower already exists */
				if (!GameMaster.Instance.map.IsTowerBuilt((int)transformedLocation.x, (int)transformedLocation.z))
				{
					/* No tower exists, set the value in the map */
					GameMaster.Instance.map.AddTower((int)transformedLocation.x, (int)transformedLocation.z);
					
					Debug.Log (transformedLocation);
					
					/* Then instantiate the tower at the location */
					Vector3 finalLocation = TransformGridToWorldSpace(transformedLocation) + new Vector3(5f, 0, 5.5f);
					
					finalLocation += new Vector3(0f, 0, 0);
					
					GameObject go = Instantiate(Resources.Load ("MagicTowerPrefab"), finalLocation, Quaternion.identity) as GameObject;
					
					//go.GetComponent<ArrowFire>().arrowTarget = GameObject.FindGameObjectWithTag ("Enemy");
					//go.GetComponent<ArrowFire>().arrowPrefab = Resources.Load ("ArrowPrefab") as GameObject;
					
				}
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
			//Debug.DrawLine (StartPos, EndPos, Color.white, 1.0f, false);
		}
	}
	
	public Vector3 TransformGridToArray(Vector3 Coords)
	{
		Vector3 toReturn = Vector3.zero;
		
		toReturn.x = (((int)Coords.x - X_OFFSET) / (int)GridSize);
		toReturn.z = (((int)Coords.z - Y_OFFSET) / (int)GridSize);
		
		return toReturn;
	}
	
	public Vector3 TransformCoordinatesToGrid(Vector3 MouseClickLocation)
	{
		Vector3 toReturn = Vector3.zero;
		
		toReturn.x = ((int)MouseClickLocation.x / (int)GridSize) * GridSize + X_OFFSET;
		toReturn.z = ((int)MouseClickLocation.z / (int)GridSize) * GridSize + Y_OFFSET;
		
 		return toReturn;
	}
	
	public Vector3 TransformGridToWorldSpace(Vector3 Coords)
	{
		Vector3 toReturn = Coords * 10.0f + new Vector3(X_OFFSET, 0, Y_OFFSET);
		
		return toReturn;
	}
}
