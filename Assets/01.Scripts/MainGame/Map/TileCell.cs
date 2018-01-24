﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileLayer
{
    GROUND,
    MIDDLE,
    GAME_UI,
    MAXCOUNT,
}

public class TileCell
{
    Vector2 _position;
    int _tileX;
    int _tileY;
    List<List<MapObject>> _mapObjectMap = new List<List<MapObject>>();

    public void Init()
    {
        for (int i = 0; i < (int)eTileLayer.MAXCOUNT; i++)
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

    public int GetTileX() { return _tileX; }
    public int GetTileY() { return _tileY; }

    public void SetTilePosition(int tileX, int tileY)
    {
        _tileX = tileX;
        _tileY = tileY;
    }

    public void AddObject(eTileLayer layer, MapObject mapObject)
    {
        List<MapObject> mapObjectList = _mapObjectMap[(int)layer];

        int sortingOrder = mapObjectList.Count;
        mapObject.SetSortingOrder(layer, sortingOrder);
        mapObject.SetPosition(_position);

        mapObjectList.Add(mapObject);
    }

    public void RemoveObject(MapObject mapObject)
    {
        List<MapObject> mapObjectList = _mapObjectMap[(int)mapObject.GetCurrentLayer()];
        mapObjectList.Remove(mapObject);
    }


    //Move

    public bool CanMove()
    {
        for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> mapObjectList = _mapObjectMap[layer];
            for (int i = 0; i < mapObjectList.Count; i++)
            {
                if (false == mapObjectList[i].CanMove())
                    return false;
            }
        }
        return true;
    }

    public List<MapObject> GetCollsionList()
    {
        List<MapObject> collisionList = new List<MapObject>();

        for (int layer = 0; layer < (int)eTileLayer.MAXCOUNT; layer++)
        {
            List<MapObject> mapObjectList = _mapObjectMap[layer];
            for (int i = 0; i < mapObjectList.Count; i++)
            {
                if (false == mapObjectList[i].CanMove())
                {
                    collisionList.Add(mapObjectList[i]);
                }
            }
        }
        return collisionList;
    }


    //pathfinding
    bool _isVisit = false;
    float _distanceFromStart = 0.0f;
    float _distanceWeight = 1.0f;

    public void Visit() { _isVisit = true; }
    public bool IsVisit() { return _isVisit; }
    public void ResetVisit() { _isVisit = false; }
    public void ResetHeuristic() { _distanceFromStart = 0; }
    public float GetDistanceFromStart() { return _distanceFromStart; }
    public float GetDistanceWeight() { return _distanceWeight; }

    public void DrawColor()
    {
        List<MapObject> mapObjectList = _mapObjectMap[(int)eTileLayer.GROUND];
        mapObjectList[0].transform.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}