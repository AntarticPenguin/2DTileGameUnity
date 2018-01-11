using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MapObject
{
    public GameObject _CharacterView;

    //Unity Functions
	
	void Start ()
    {
        
	}
	
	void Update ()
    {
		
	}


    //Init

    public void Init()
    {
        TileMap map = GameManager.Instance.GetMap();

        int x = Random.Range(1, map.GetWidth() - 2);
        int y = Random.Range(1, map.GetHeight() - 2);
        TileCell tileCell = map.GetTileCell(x, y);
        tileCell.AddObject(eTileLayer.MIDDLE, this);
    }

    override public void SetSortingOrder(int sortingID, int sortingOrder)
    {
        _CharacterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _CharacterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }
}
