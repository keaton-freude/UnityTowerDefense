using UnityEngine;
using System.Collections;

public class ArrowFire : MonoBehaviour {
	
	public GameObject arrowPrefab;
	public GameObject arrowTarget;
	public float AttackRadius;
	
	public float attackCooldown;
	private float currentTime;
	
	private Mesh mesh;
	private Vector3 offset;

	// Use this for initialization
	void Start () 
	{
		mesh = this.GetComponent<MeshFilter>().mesh;
		offset = new Vector3(0, -6.5f, mesh.bounds.size.z / 2.0f);
		
		/* Need to modify the radius of the cylinder that describes radius */
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentTime += Time.deltaTime;
		
		if (currentTime >= attackCooldown)
		{
			currentTime -= attackCooldown;
			
			/* Create an arrow */
			
			GameObject closest_target = GameMaster.Instance.GetNearestEnemy(transform.position, AttackRadius);
			
			if (closest_target != null)
			{
				GameObject go = Network.Instantiate(Resources.Load ("ArrowPrefab"), transform.position - offset, Quaternion.identity, 0) as GameObject;
				go.GetComponent<MoveArrow>().Target = closest_target;
                TowerInfo ti = GetComponent<TowerInfo>();
                go.GetComponent<MoveArrow>().damage = Random.Range(ti.GroundMinimumDamage, ti.GroundMaximumDamage);
			}
			
		}
	}
}
