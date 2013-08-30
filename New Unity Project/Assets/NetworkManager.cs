using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[RPC]
	public void DoDamageToMob(NetworkViewID id, int amount)
	{
		NetworkView.Find (id).gameObject.GetComponent<MobStats>().TakeDamage(amount);
	}
}
