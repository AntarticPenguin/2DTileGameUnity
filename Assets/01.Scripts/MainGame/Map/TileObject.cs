using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MapObject
{

    //Unity Functions

	void Start ()
    {
        _type = eMapObjectType.TILE_OBJECT;
    }

	void Update ()
    {
		
	}


    //Init

    public void Init(Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetTilePosition(int tileX, int tileY)
    {
        _tileX = tileX;
        _tileY = tileY;
    }
}
