using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using JXH;

//[InitializeOnLoad]
public class ItemBase : MonoBehaviour {
    public ItemType itemType = ItemType.Item_Empty;

    public Image image;
    
    public void SetItemType()
    {
        image.color = Color.white;
        Sprite sprite = FMConfigManager.Ins.GetSpriteByType(itemType);
        if (sprite != null)
        {
            image.sprite = sprite;
            return;
        }
        image.sprite = null;
        switch (itemType)
        {
            case ItemType.Item_Empty:
                image.color = Color.white;
                break;
            case ItemType.Item_Exit:
                image.color = Color.green;
                break;
            case ItemType.Item_EmptyExit:
                image.color = Color.green;
                break;
            case ItemType.Item_Ice:
                image.color = Color.blue;
                break;
            case ItemType.Item_Player:
                image.color = Color.black;
                //Sprite sprit = Resources.Load<Sprite>("child");
                //image.sprite = levelManager.GetSpriteByType(JXH.ItemType.Item_Player);
                break;
            case ItemType.Item_Trap:
                image.color = Color.red;
                break;
            case ItemType.Item_Wall:
                image.color = Color.yellow;
                break;
        }
    }
	// Use this for initialization
	void Start () {
        SetItemType();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
