using UnityEngine;
using System.Collections;

public class RTSCameraMove : MonoBehaviour 
{
	//We'll be accessing the transform that this script is attached to very frequently
	//so we should store that transform locally instead of looking it up each time
	private Transform myTransform;
	
	//This is the amount of area that one's mouse needs to be within to move the screen (compared to edges)
	public float scrollArea;
	
	//how fast to scroll
	public float scrollSpeed; 
	
	public float zoomSpeed;

	// Use this for initialization
	void Start () 
	{
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;
		
		if (x <= scrollArea)
		{
			//Move to the left
			myTransform.position += new Vector3(-scrollSpeed * Time.deltaTime, 0, 0);
		}
		if (x >= Screen.width - scrollArea)
		{
			//Move to the right
			myTransform.position += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
		}
		
		if (y <= scrollArea)
		{
			myTransform.position += new Vector3(0, 0, -scrollSpeed * Time.deltaTime);
		}
		
		if (y >= Screen.height - scrollArea)
		{
			myTransform.position += new Vector3(0, 0, scrollSpeed * Time.deltaTime);
		}
		
		myTransform.position += new Vector3(0, Input.GetAxis ("Mouse ScrollWheel") * -zoomSpeed * Time.deltaTime, 0);
	}
}
