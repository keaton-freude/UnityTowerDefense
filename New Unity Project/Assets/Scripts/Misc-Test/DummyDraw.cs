using UnityEngine;
using System.Collections;

public class DummyDraw : MonoBehaviour {
	public int GridSize = 10;
	
	Vector3 StartPos = new Vector3(0, .2f, 0);
	Vector3 EndPos = new Vector3(0, .2f, 0);
	
	public int X_OFFSET = 20;
	
	public int Y_OFFSET = 23;
	
	// Use this for initialization
	void Start () {
		EndPos.z = 500 - Y_OFFSET - 4;
		
		StartPos.z = Y_OFFSET;
	}
	
	// Update is called once per frame
	void Update () {
		

	}
	
	void OnDrawGizmos()
	{
		for(int i = X_OFFSET; i <= 500 - X_OFFSET; i += GridSize)
		{
			StartPos.x = i;
			EndPos.x = i;
			//Gizmos.DrawLine (StartPos, EndPos);
			//Debug.DrawLine (StartPos, EndPos, Color.white, 1.0f, false);
		}
	}
}
