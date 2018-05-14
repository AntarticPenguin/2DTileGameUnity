using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMapType
{
    TOWN,
    DUNGEON,
}

public class GameManager
{
    //Singleton

    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = new GameManager();
                _instance.Init();
            }
            return _instance;
        }
    }


    //Init

    void Init()
    {
        
    }

    #region MAP

    TileMap _tileMap;
    eMapType _mapType = eMapType.TOWN;

    public TileMap GetMap()
    {
        return _tileMap;
    }

    public void SetMap(TileMap map)
    {
        _tileMap = map;
    }

    public eMapType GetMapType()
    {
        return _mapType;
    }

    public void SetMapType(eMapType type)
    {
        _mapType = type;
    }

    #endregion
}
