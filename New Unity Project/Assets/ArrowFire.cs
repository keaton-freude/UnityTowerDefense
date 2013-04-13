using UnityEngine;
using System.Collections;

public class ArrowFire : MonoBehaviour {
	
	public GameObject arrowPrefab;
	public GameObject arrowTarget;
	
	public float attackCooldown;
	private float currentTime;
	
	private Mesh mesh;
	private Vector3 offset;

	// Use this for initialization
	void Start () 
	{
		mesh = this.GetComponent<MeshFilter>().mesh;
		offset = new Vector3(0, -6.5f, mesh.bounds.size.z / 2.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentTime += Time.deltaTime;
		
		if (currentTime >= attackCooldown)
		{
			currentTime -= attackCooldown;
			
			/* Create an arrow */
			GameObject go = Instantiate(Resources.Load ("ArrowPrefab"), transform.position - offset, Quaternion.identity) as GameObject;
			go.GetComponent<MoveArrow>().Target = arrowTarget;
			
			
			
			
		}
	}
}
