using UnityEngine;
using System.Collections;

public class DaylightCycle : MonoBehaviour 
{
	public float RotateSpeed = 10.0f;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.Rotate (0f, 1f * Time.deltaTime * RotateSpeed, 0f);
		

		
		if (this.transform.eulerAngles.x >= 0f && this.transform.eulerAngles.x <= 90f)
		{
			if (this.light.enabled == false)
				this.light.enabled = true;
		}
		else
		{
			if (this.light.enabled)
				this.light.enabled = false;
		}
		
	
	}
}
