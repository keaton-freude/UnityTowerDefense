using UnityEngine;
using System.Collections;

public class TestParticles : MonoBehaviour {
	
	public ParticleSystem ps;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.B))
		{
			ps.transform.position = this.transform.position + new Vector3(0, 9, 0);
			ps.Play ();
		}
	}
	
	public void Go()
	{
		ps.transform.position = this.transform.position;
		ps.Play ();
	}
}
