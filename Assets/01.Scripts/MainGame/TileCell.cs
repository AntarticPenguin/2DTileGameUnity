using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileLayer
{
    GROUND,
}

public class TileCell
{
    Vector2 _position;

    public void Init()
    {

    }

    public void SetPosition(float x, float y)
    {
        _position.x = x;
        _position.y = y;
    }

    public void AddObject(eTileLayer layer, TileObject tileObject)
    {
        tileObject.SetPosition(_position);
    }
}
