using UnityEngine;
using System.Collections;

public class TestGUI : MonoBehaviour {
	public Texture2D hpTexture;
	public float full_width = 90f;
	public float MaxHP = 100f;
	public float CurrentHP = 100f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void OnGUI()
	{
		float percent = CurrentHP / MaxHP;
		
		Vector3 screenPosition =
		
		   Camera.mainCamera.WorldToScreenPoint(transform.position + new Vector3(0, 18, 0));// gets screen position.
		
		screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
		
		Rect rect = new Rect(screenPosition.x - 50,
		
		   screenPosition.y + 40, 90 * percent, 10);// makes a rect centered at the player ( 100x24 )
		
		//GUI.Box(rect, "Enemy");
		GUI.DrawTexture(rect, hpTexture);
		
		if (CurrentHP <= 0)
			GameObject.Destroy(this.gameObject);
		
	}
}
