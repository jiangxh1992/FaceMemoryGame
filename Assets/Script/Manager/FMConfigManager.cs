using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMConfigManager : Singleton<FMConfigManager> {

    public Sprite[] ItemSprite = new Sprite[6];
    public Sprite GetSpriteByType(JXH.ItemType type)
    {
        return ItemSprite[(int)type];
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
