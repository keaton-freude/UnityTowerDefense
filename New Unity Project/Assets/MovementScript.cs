using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
	public Vector3 nextPosition = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!networkView.isMine)
		{
			if (nextPosition != Vector3.zero)
				this.transform.position = Vector3.MoveTowards(this.transform.position, nextPosition, Time.deltaTime * 30.0f);
			animation.Play ("run");
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 position = this.transform.position;
			Quaternion rotation = this.transform.rotation;
			stream.Serialize(ref position);
			stream.Serialize(ref rotation);
		}
		else
		{	
			Vector3 position = Vector3.zero;
			Quaternion rotation = Quaternion.identity;
			stream.Serialize(ref position);
			stream.Serialize(ref rotation);
			nextPosition = position;
			this.transform.rotation = rotation;
		}
	}
}
