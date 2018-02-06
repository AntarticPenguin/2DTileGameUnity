using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MapObject
{
    public void Init(int itemIndex, int tileX, int tileY)
    {
        string filePath = "Prefabs/Item/ItemFrame";
        GameObject ItemPrefabs = Resources.Load<GameObject>(filePath);
        GameObject itemGameObject = GameObject.Instantiate(ItemPrefabs);

        TileCell tileCell = GameManager.Instance.GetMap().GetTileCell(tileX, tileY);

        tileCell.AddObject(eTileLayer.MIDDLE, this);
    }
}
