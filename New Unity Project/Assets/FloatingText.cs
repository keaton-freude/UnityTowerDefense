using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour 
{
    public float scroll = 0.05f;
    public float duration = 1.5f;
    private float alpha = 1.0f;
	// Use this for initialization
	void Start () 
    {
        guiText.material.color = new Color(1f, 1f, 1f, 1f);
        alpha = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (alpha > 0f)
        {
            transform.Translate(0f, scroll * Time.deltaTime, 0f);
            alpha -= Time.deltaTime / duration;
            guiText.material.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            Destroy(transform.gameObject);
        }
	}
}
