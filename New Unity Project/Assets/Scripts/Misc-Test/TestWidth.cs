using UnityEngine;
using System.Collections;

public class TestWidth : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.K))
		{
			Mesh mesh = GetComponent<MeshFilter>().mesh;
			Debug.Log(mesh.bounds.size.x * transform.localScale.x);
			Debug.Log(mesh.bounds.size.z * transform.localScale.z);
		}
		
	}
}
