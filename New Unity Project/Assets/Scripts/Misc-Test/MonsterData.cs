using UnityEngine;
using System.Collections;

[System.Serializable]
public class MonsterData 
{	
	//What prefab does this MonsterData represent?
	public GameObject prefab;
	//How long until users can start buying this guy?
	public float timeToUnlock;
	//Max # of this unit
	public int maxNumberInQueue;
	//How long does it take to add to the queue?
	public float timeToAdd;
	//How much does this guy cost?
	public int goldCost;
	//What image do we draw for its icon?
	public Texture2D iconTexture;
	//What is the description for this monster?
	public string description;

}
