using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileLayer
{
    GROUND,
    MIDDLE,
    MAXCOUNT,
}

public class TileCell
{
    Vector2 _position;
    List<List<MapObject>> _mapObjectMap = new List<List<MapObject>>();

    public void Init()
    {
        for(int i = 0; i < (int)eTileLayer.MAXCOUNT; i++)
        {
            List<MapObject> mapObjectlist = new List<MapObject>();
            _mapObjectMap.Add(mapObjectlist);
        }
    }

    public void SetPosition(float x, float y)
    {
        _position.x = x;
        _position.y = y;
    }

    public void AddObject(eTileLayer layer, MapObject mapObject)
    {
        List<MapObject> mapObjectList = _mapObjectMap[(int)layer];

        int sortingID = SortingLayer.NameToID(layer.ToString());
        int sortingOrder = mapObjectList.Count;
        mapObject.SetSortingOrder(sortingID, sortingOrder);
        mapObject.SetPosition(_position);

        mapObjectList.Add(mapObject);
    }
}
