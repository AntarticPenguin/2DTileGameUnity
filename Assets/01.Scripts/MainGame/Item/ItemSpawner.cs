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

    Sprite[] _spriteArray;

    void Init()
    {
        string filePath = "Sprites/weapon";
        _spriteArray = Resources.LoadAll<Sprite>(filePath);
    }

    public void SpawnItem(int itemIndex, int tileX, int tileY)
    {
        Item item = CreateItem(itemIndex);
        GameManager.Instance.GetMap().SetObject(tileX, tileY, item, eTileLayer.GROUND);
    }

    Item CreateItem(int itemIndex)
    {
        string filePath = "Prefabs/Item/ItemFrame";

        GameObject itemPrefab = Resources.Load<GameObject>(filePath);
        GameObject itemObject = GameObject.Instantiate(itemPrefab);
        itemObject.transform.SetParent(GameManager.Instance.GetMap().transform);
        itemObject.transform.localScale = Vector3.one;
        itemObject.transform.localPosition = Vector3.zero;

        itemObject.GetComponent<SpriteRenderer>().sprite = _spriteArray[itemIndex];

        Item item = itemObject.AddComponent<Item>();

        return item;
    }
}
