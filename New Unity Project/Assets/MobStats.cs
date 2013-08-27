using UnityEngine;
using System.Collections;

public class MobStats: MonoBehaviour {
	public float MaxHP = 100f;
	public float CurrentHP = 100f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (CurrentHP <= 0)
			GameObject.Destroy(this.gameObject);
	}
	
	void OnGUI()
	{

	}
	
	public void TakeDamage(float amt)
	{
		CurrentHP -= amt;
		/* Update the healthbar */
		this.GetComponentInChildren<HealthbarScript>().UpdateHealth(CurrentHP / MaxHP);
	}
}
