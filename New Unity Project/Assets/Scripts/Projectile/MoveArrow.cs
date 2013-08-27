using UnityEngine;
using System.Collections;

public class MoveArrow : MonoBehaviour {
	
	public GameObject Target;
	public float Speed;
	
	public ParticleSystem OnDeathEffect;
	
	public Transform myTransform;
	
	private Vector3 offset;

    public int damage;
	
	void Awake()
	{
		myTransform = this.transform;
	}
	
	// Use this for initialization
	void Start () 
	{
		if (Target == null)
			return; 
		
		Vector3 v1 = new Vector3 (Target.transform.position.x, myTransform.position.y, Target.transform.position.z);
		
		this.transform.LookAt (v1);
		
		offset = new Vector3(0, 6.5f, 0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Target == null)
		{
			if (this.gameObject != null)
				GameObject.Destroy (this.gameObject); //we don't have a target. lets just kill ourselves :(
			return;
		}
		
		if (this.gameObject != null)
		{
		
			myTransform.position = Vector3.MoveTowards(myTransform.position, Target.transform.position + offset, Time.deltaTime * Speed);
			
			Vector3 v1 = new Vector3 (Target.transform.position.x, this.transform.position.y, Target.transform.position.z);
			
			this.transform.LookAt (v1);
		}
		

			
	}
	
	void OnTriggerEnter(Collider other)
	{
		//Debug.Log ("hello");
		
		
		
		if (other.tag == "Enemy")
		{
            other.gameObject.GetComponent<MobStats>().TakeDamage(damage);
            var v = Camera.main.WorldToViewportPoint(other.transform.position);
            GameMaster.Instance.SpawnFloatingDamage(damage, v.x, v.y);
			if (this.gameObject != null)
				Network.Destroy (this.gameObject);
			
			
		}
	}
}
