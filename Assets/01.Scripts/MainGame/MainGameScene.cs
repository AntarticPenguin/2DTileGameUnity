using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameScene : MonoBehaviour
{
    public TileMap _tileMap;
    public Player _testPlayer;


    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void Init()
    {
        _tileMap.Init();
        GameManager.Instance.SetMap(_tileMap);

        _testPlayer.Init();
    }
}
