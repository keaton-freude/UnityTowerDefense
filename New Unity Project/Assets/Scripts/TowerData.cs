using UnityEngine;
using System.Collections;

[System.Serializable]
public class TowerData
{
    //What prefab does this TowerData represent?
    public GameObject prefab;
    //How much does this tower cost?
    public int goldCost;
    //What image do we draw for its icon?
    public Texture2D iconTexture;
    //What is the description for this tower?
    public string description;
}

