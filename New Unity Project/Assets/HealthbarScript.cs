using UnityEngine;
using System.Collections;

public class HealthbarScript : MonoBehaviour 
{
	
	public float percent = 1f;
	public float max_width = .6f;
	public float min_width = 0f;
	
	public Transform backgroundHealthBar;
	
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//this.transform.LookAt (Camera.main.transform);
		
		//this.transform.localRotation = Quaternion.Euler (this.transform.localRotation.x, this.transform.localRotation.y + 160f, 0f);
		
		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
		
		//backgroundHealthBar.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
          //  Camera.main.transform.rotation * Vector3.up);
		
		//backgroundHealthBar.localPosition = transform.localPosition;
		backgroundHealthBar.localRotation = transform.localRotation;
	}
	
	public void UpdateHealth(float new_percent)
	{
		percent = new_percent;
		float amt = Mathf.Lerp(min_width, max_width, percent);
		
		this.transform.localScale = new Vector3(amt, transform.localScale.y, transform.localScale.z);
		
		Debug.Log ("Setting ");
	}
}
