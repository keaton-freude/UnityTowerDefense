using UnityEngine;
using System.Collections;

public class ShootFireball : MonoBehaviour {
	public GameObject fireballPrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = camera.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit = new RaycastHit();
			
			if (Physics.Raycast (ray, out hit))
			{
				GameObject go = GameObject.Instantiate(fireballPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				
				Vector3 Target = new Vector3(hit.point.x, 2f, hit.point.z);
				
				Vector3 Direction = Target - Vector3.zero;
				Direction.Normalize();
				
				go.rigidbody.velocity = Direction * 40f;
			}
			
		}
	}
}
