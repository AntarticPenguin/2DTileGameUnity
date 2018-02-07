using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner
{
    static ItemSpawner _instance;
    public static ItemSpawner Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = new ItemSpawner();
                _instance.Init();
            }
            return _instance;
        }
    }


    void Init()
    {

    }

    public void SpawnItem(int itemIndex, int tileX, int tileY)
    {
        Item item = CreateItem("itemName");

        GameManager.Instance.GetMap().SetObject(tileX, tileY, item, eTileLayer.GROUND);
    }

    Item CreateItem(string itemName)
    {
        string filePath = "Prefabs/Item/ItemFrame";

        GameObject itemPrefab = Resources.Load<GameObject>(filePath);
        GameObject itemObject = GameObject.Instantiate(itemPrefab);

        Sprite sprite = Resources.Load<Sprite>("Sprites/weapon/battle_axe1");
        itemObject.GetComponent<SpriteRenderer>().sprite = sprite;

        Item item = itemObject.AddComponent<Item>();

        return item;
    }
}
