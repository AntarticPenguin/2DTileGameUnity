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
        Item item = new Item();
        item.Init(itemIndex, tileX, tileY);
    }
}
